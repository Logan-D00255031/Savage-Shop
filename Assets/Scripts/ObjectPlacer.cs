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
<<<<<<< HEAD
=======

    public void RemoveObjectAt(int gameObjectIndex)   // Removes desired GameObject stored in list from scene
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null) // If index is out of bounds or null
        {
            return;
        }
        Destroy(placedGameObjects[gameObjectIndex]);    // Remove game object from scene
        placedGameObjects[gameObjectIndex] = null;  // Set game object from list to null
    }

    public GameObject GetObjectAtIndex(int gameObjectIndex) 
    {
        return placedGameObjects[gameObjectIndex];
    }
>>>>>>> 22029faa9cdd267d6597953fe70efb830d936e82
}
