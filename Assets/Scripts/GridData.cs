using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int objectPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> desiredPositions = CalculatePositions(objectPosition, objectSize);
        PlacementData data = new PlacementData(desiredPositions, ID, placedObjectIndex);
        foreach (Vector3Int p in desiredPositions)
        {
            if (placedObjects.ContainsKey(p)) 
            {
                throw new Exception($"Dictionary already contains cell position {p}");
            }
            placedObjects.Add(p, data);
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int position, Vector2Int objectSize) // ONLY WORKS IF POSITION IS THE BOTTOM LEFT CORNER OF THE OBJECT AND NOT ROTATED
    {
        List<Vector3Int> values = new List<Vector3Int>();
        for (int i = 0; i < objectSize.x; i++)
        {
            for (int j = 0; j < objectSize.y; j++) 
            { 
                values.Add(position + new Vector3Int(i, j, 0));
            }
        }
        return values;
    }

    public bool ObjectCanBePlacedAt(Vector3Int objectPosition, Vector2Int objectSize) 
    {
        List<Vector3Int> desiredPositions = CalculatePositions(objectPosition, objectSize); // Get Positions
        foreach (Vector3Int p in desiredPositions)
        {
            if (placedObjects.ContainsKey(p))   // If position is already contained in Dictionary
            {
                return false;
            }
        }
        return true;
    }
}

public class PlacementData
{
    public List<Vector3Int> positionsOccupied;

    public int ID { get; private set; }

    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> positionsOccupied, int ID, int placedObjectIndex)
    {
        this.positionsOccupied = positionsOccupied;
        this.ID = ID;
        PlacedObjectIndex = placedObjectIndex;
    }
}
