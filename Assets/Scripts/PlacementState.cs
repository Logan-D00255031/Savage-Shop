using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacementState : IBuildState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    PrefabDatabaseSO prefabDatabase;
    GridData gridData;
    ObjectPlacer objectPlacer;
<<<<<<< HEAD
=======
    private float objectRotation = 0f;
    private Vector2Int objectSize;
    private Vector3Int objectGridPosition = Vector3Int.zero;
>>>>>>> 22029faa9cdd267d6597953fe70efb830d936e82

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          PrefabDatabaseSO prefabDatabase,
                          GridData gridData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.prefabDatabase = prefabDatabase;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = prefabDatabase.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
<<<<<<< HEAD
            previewSystem.BeginPreview(
=======
            this.objectSize = prefabDatabase.objectsData[selectedObjectIndex].Size;
            previewSystem.BeginPlacementPreview(
>>>>>>> 22029faa9cdd267d6597953fe70efb830d936e82
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
        previewSystem.EndPreview();
    }

    public void OnAction(Vector3Int mouseGridPosition)
    {
        CalculateObjectGridPosition(mouseGridPosition); // Get new Object grid position

<<<<<<< HEAD
        if (!CheckValidPlacement(gridPosition, selectedObjectIndex))    // If the position to place is not valid
=======
        Vector3 worldPosition = grid.CellToWorld(objectGridPosition);

        if (!CheckValidPlacement(objectGridPosition, selectedObjectIndex, objectRotation))    // If the position to place is not valid
>>>>>>> 22029faa9cdd267d6597953fe70efb830d936e82
        {
            // Invalid sound can be added here
            return;
        }

        // Valid sound can be added here

        int index = objectPlacer.PlaceObject(prefabDatabase.objectsData[selectedObjectIndex].Prefab, worldPosition);

        // Add the placed object's data to the grid data
        gridData.AddObjectAt(objectGridPosition,
            prefabDatabase.objectsData[selectedObjectIndex].Size,
            prefabDatabase.objectsData[selectedObjectIndex].ID,
            index);
        previewSystem.UpdatePosition(worldPosition, false);  // Update placed position to be invalid
    }

    private bool CheckValidPlacement(Vector3Int gridPosition, int selectedObjectIndex)
    {
        Vector2Int objectSize = prefabDatabase.objectsData[selectedObjectIndex].Size;
        return gridData.ObjectCanBePlacedAt(gridPosition, objectSize);
    }

    public void UpdateState(Vector3Int mouseGridPosition)
    {
<<<<<<< HEAD
        bool validPlacement = CheckValidPlacement(gridPosition, selectedObjectIndex);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validPlacement);
=======
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

        bool validPlacement = CheckValidPlacement(objectGridPosition, selectedObjectIndex, objectRotation);
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
>>>>>>> 22029faa9cdd267d6597953fe70efb830d936e82
    }
}
