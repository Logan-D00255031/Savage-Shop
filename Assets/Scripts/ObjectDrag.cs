using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Learned from Tutorial: https://youtu.be/rKp9fWvmIww?si=6ueW9PHdiFnlvi5h

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown()
    {
        offset = transform.position - PlacementSystem.GetMouseInWorld(); // Gets mouse offset from object's centre
    }

    private void OnMouseDrag()
    {
        Vector3 position = PlacementSystem.GetMouseInWorld() + offset;
        transform.position = PlacementSystem.instance.SnapToGrid(position);
    }
}
