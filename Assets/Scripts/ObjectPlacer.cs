using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    [Sirenix.OdinInspector.ReadOnly]
    private List<GameObject> placedGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position) // Places Object Prefab at desired position
    {
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity);
        Debug.Log($"New {newObject.name} placed at {newObject.transform.position}");
        placedGameObjects.Add(newObject); // Add to List of placed objects

        return placedGameObjects.Count - 1; // Return new placed object's index in List
    }
}
