using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public GridLayout gridLayout;
    public bool Placed {  get; private set; }
    public Vector3Int Size { get; private set; }

    public Vector3 GetStartPosition()
    {
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        Vector3 startPosition = gameObject.transform.position + new Vector3(-boxCollider.size.x, 0, -boxCollider.size.z) * 0.5f;
        Debug.Log($"Position: {startPosition}");
        return startPosition;
    }

    private void Start()
    {
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        gridLayout = PlacementSystem.instance.GetGridLayout();
        Size = gridLayout.WorldToCell(boxCollider.size);
    }

    public void Place()
    {
        ObjectDrag objectDrag = gameObject.GetComponent<ObjectDrag>();
        Destroy(objectDrag);

        Placed = true;
    }



}
