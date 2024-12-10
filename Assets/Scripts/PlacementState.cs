using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;

// Learned from Tutorial: https://www.youtube.com/watch?v=l0emsAHIBjU&list=PLcRSafycjWFepsLiAHxxi8D_5GGvu6arf

public class PlacementState : IBuildState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    PrefabDatabaseSO prefabDatabase;
    GridData gridData;
    ObjectPlacer objectPlacer;
    PrefabInventoryManager prefabInventory;
    private float objectRotation = 0f;
    private Vector2Int objectSize;
    private Vector3Int objectGridPosition = Vector3Int.zero;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          PrefabDatabaseSO prefabDatabase,
                          GridData gridData,
                          ObjectPlacer objectPlacer,
                          PrefabInventoryManager prefabInventory)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.prefabDatabase = prefabDatabase;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;
        this.prefabInventory = prefabInventory;

        selectedObjectIndex = prefabDatabase.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            this.objectSize = prefabDatabase.objectsData[selectedObjectIndex].Size;
            previewSystem.BeginPlacementPreview(
                prefabDatabase.objectsData[selectedObjectIndex].Prefab,
                prefabDatabase.objectsData[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No object with ID {iD}");
        }

    }

    public void EndState()
    {
        previewSystem.UpdatePosition(Vector3.zero, false, 0f);  // Reset preview rotation
        previewSystem.EndPreview();
    }

    public void OnAction(Vector3Int mouseGridPosition)
    {
        CalculateObjectGridPosition(mouseGridPosition); // Get new Object grid position

        Vector3 worldPosition = grid.CellToWorld(objectGridPosition);

        // Determine if the position to place is not valid or there is none of the item in the inventory
        if (!CheckValidPlacement(objectGridPosition, selectedObjectIndex, objectRotation) ||
            !prefabInventory.ContainsItemWithID(ID))
        {
            // Invalid sound can be added here
            return;
        }

        // Valid sound can be added here

        int index = objectPlacer.PlaceObject(prefabDatabase.objectsData[selectedObjectIndex].Prefab, worldPosition, objectRotation);

        // Add the placed object's data to the grid data
        gridData.AddObjectAt(objectGridPosition,
            prefabDatabase.objectsData[selectedObjectIndex].Size,
            prefabDatabase.objectsData[selectedObjectIndex].ID,
            index, objectRotation);
        previewSystem.UpdatePosition(worldPosition, false);  // Update placed position to be invalid

        // Remove object from inventory
        prefabInventory.RemoveItem(ID);
    }

    //private bool CheckValidPlacement(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    Vector2Int objectSize = prefabDatabase.objectsData[selectedObjectIndex].Size;
    //    return gridData.ObjectCanBePlacedAt(gridPosition, objectSize);
    //}

    private bool CheckValidPlacement(Vector3Int gridPosition, int selectedObjectIndex, float objectRotation)
    {
        Vector2Int objectSize = prefabDatabase.objectsData[selectedObjectIndex].Size;
        return gridData.ObjectCanBePlacedAt(gridPosition, objectSize, objectRotation);
    }

    public void UpdateState(Vector3Int mouseGridPosition)
    {
        float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");

        // Check if the wheel is scrolled up or down
        if (mouseScrollWheel > 0f)
        {
            Debug.Log("Mouse wheel scrolled up");
            RotateClockwise();
        }
        else if (mouseScrollWheel < 0f)
        {
            Debug.Log("Mouse wheel scrolled down");
            RotateCounterClockwise();
        }

        CalculateObjectGridPosition(mouseGridPosition); // Get new Object grid position

        bool validPlacement = 
            CheckValidPlacement(objectGridPosition, selectedObjectIndex, objectRotation) && 
            prefabInventory.ContainsItemWithID(ID);
        previewSystem.UpdatePosition(grid.CellToWorld(objectGridPosition), validPlacement, objectRotation);
    }

    private void CalculateObjectGridPosition(Vector3Int mouseGridPosition)
    {
        Vector2Int mouseGridOffset = (gridData.CalculateRotatedSize(objectSize, objectRotation) / 2); // Get mouse position offset from half of the rotated size
        objectGridPosition = mouseGridPosition - new Vector3Int(mouseGridOffset.x, mouseGridOffset.y, 0);    // Apply offset to get new object grid position
        //Debug.Log($"Offset: {mouseGridOffset} , Object Pos: {objectGridPosition}");
    }

    private void RotateClockwise()
    {
        objectRotation += 90f;
        // Keep value within 360 degrees
        if (objectRotation >= 360f)
        {
            objectRotation -= 360f;
        }
        Debug.Log($"New rotation: {objectRotation} degrees");
    }

    private void RotateCounterClockwise()
    {
        objectRotation -= 90f;
        // Keep value within 360 degrees
        if (objectRotation < 0f)
        {
            objectRotation += 360f;
        }
        Debug.Log($"New rotation: {objectRotation} degrees");
    }
}
