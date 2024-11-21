using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Learned from Tutorial: https://www.youtube.com/watch?v=l0emsAHIBjU&list=PLcRSafycjWFepsLiAHxxi8D_5GGvu6arf

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

    public void AddObjectAt(Vector3Int objectPosition, Vector2Int objectSize, int ID, int placedObjectIndex, float objectRotation)
    {
        List<Vector3Int> desiredPositions = CalculatePositions(objectPosition, objectSize, objectRotation);
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

    public int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if(!placedObjects.ContainsKey(gridPosition)) // If no data stored at grid position
        { 
            return -1; 
        }
        return placedObjects[gridPosition].PlacedObjectIndex;
    }

    public int GetRepresentationID(Vector3Int gridPosition)
    {
        if (!placedObjects.ContainsKey(gridPosition)) // If no data stored at grid position
        {
            return -1;
        }
        return placedObjects[gridPosition].ID;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (Vector3Int pos in placedObjects[gridPosition].positionsOccupied)
        {
            placedObjects.Remove(pos);
        }
    }

    internal bool ObjectCanBePlacedAt(Vector3Int objectPosition, Vector2Int objectSize, float objectRotation)
    {
        List<Vector3Int> desiredPositions = CalculatePositions(objectPosition, objectSize, objectRotation); // Get Positions
        foreach (Vector3Int p in desiredPositions)
        {
            if (placedObjects.ContainsKey(p))   // If position is already contained in Dictionary
            {
                return false;
            }
        }
        return true;
    }

    private List<Vector3Int> CalculatePositions(Vector3Int position, Vector2Int objectSize, float objectRotation)
    {
        // Get rotated size to be used to properly calculate positions
        Vector2Int rotatedObjectSize = CalculateRotatedSize(objectSize, objectRotation);
        //Debug.Log($"New size: {rotatedObjectSize}");

        // Find range for the loop from rotatedSize
        int startX, endX, startY, endY;

        // Size X
        bool positiveX = rotatedObjectSize.x >= 0;  // Check if positive
        startX = positiveX ? 0 : rotatedObjectSize.x;
        endX = positiveX ? rotatedObjectSize.x : 0;

        // Size Y
        bool positiveY = rotatedObjectSize.y >= 0;  // Check if positive
        startY = positiveY ? 0 : rotatedObjectSize.y;
        endY = positiveY ? rotatedObjectSize.y : 0;

        List<Vector3Int> values = new();
        for (int i = startX; i < endX; i++)
        {
            for (int j = startY; j < endY; j++)
            {
                values.Add(position + new Vector3Int(i, j, 0));
            }
        }
        return values;
    }

    public Vector2Int CalculateRotatedSize(Vector2Int objectSize, float objectRotation)
    {
        if (objectRotation == 90f)
        {
            return new Vector2Int(objectSize.y, -objectSize.x);
        }
        else if (objectRotation == 180f)
        {
            return new Vector2Int(-objectSize.x, -objectSize.y);
        }
        else if (objectRotation == 270f)
        {
            return new Vector2Int(-objectSize.y, objectSize.x);
        }
        // Return original if no Rotation
        return objectSize;
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
