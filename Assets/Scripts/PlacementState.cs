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
            previewSystem.BeginPreview(
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

    public void OnAction(Vector3Int gridPosition)
    {
        Vector3 worldPosition = grid.CellToWorld(gridPosition);

        if (!CheckValidPlacement(gridPosition, selectedObjectIndex))    // If the position to place is not valid
        {
            // Invalid sound can be added here
            return;
        }

        // Valid sound can be added here

        int index = objectPlacer.PlaceObject(prefabDatabase.objectsData[selectedObjectIndex].Prefab, worldPosition);

        // Add the placed object's data to the grid data
        gridData.AddObjectAt(gridPosition,
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

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validPlacement = CheckValidPlacement(gridPosition, selectedObjectIndex);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validPlacement);
    }
}
