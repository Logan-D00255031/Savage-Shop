using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Learned from Tutorial: https://www.youtube.com/watch?v=l0emsAHIBjU&list=PLcRSafycjWFepsLiAHxxi8D_5GGvu6arf

public class RemovalState : IBuildState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData gridData;
    ObjectPlacer objectPlacer;
    PrefabInventoryManager prefabInventory;

    public RemovalState(Grid grid,
                        PreviewSystem previewSystem,
                        GridData gridData,
                        ObjectPlacer objectPlacer,
                        PrefabInventoryManager prefabInventory)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;
        this.prefabInventory = prefabInventory;

        previewSystem.BeginRemovalPreview();

    }

    public void EndState()
    {
        previewSystem.EndPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if (!gridData.ObjectCanBePlacedAt(gridPosition, Vector2Int.one)) // If an object cannot be placed at selected cell
        {
            selectedData = gridData;
        }

        if (selectedData == null)
        {
            // Invalid sound can go here
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1)
            {
                return;
            }
            // Add Object prefab ID to inventory
            int prefabDatabaseID = selectedData.GetRepresentationID(gridPosition);
            prefabInventory.AddItem(prefabDatabaseID);

            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }
        Vector3 worldPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(worldPosition, CheckIfSelectionIsValid(gridPosition)); // Update removal position to be invalid
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !gridData.ObjectCanBePlacedAt(gridPosition, Vector2Int.one); // Return false if nothing can be removed in cell
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        previewSystem.EndRemovalPreview();
        bool valid = CheckIfSelectionIsValid(gridPosition);
        if (valid)
        {
            int selectedObjectIndex = gridData.GetRepresentationIndex(gridPosition);
            if (selectedObjectIndex > -1)
            {
                GameObject selectedObject = objectPlacer.GetObjectAtIndex(selectedObjectIndex);
                previewSystem.BeginRemovalPreview(selectedObject);
            }
        }
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), valid);
    }
}
