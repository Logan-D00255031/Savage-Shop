using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
    public List<Transform> anchorPoints;

    public GameObject slotMenu;
    public Transform slotMenuContainer;
    public GameObject slotMenuItem;

    [ReadOnly, SerializeField]
    private List<GameObject> storedItems = new();
    public void PlaceItemAt(GameObject itemPrefab, Transform anchorPoint)
    {
        GameObject item = Instantiate(itemPrefab, anchorPoint);
        for (int i = 0; i < anchorPoints.Count; i++)
        {
            if (anchorPoints[i] == anchorPoint)
            {
                storedItems[i] = item;
            }
        }
    }

    public void RemoveItem(Transform anchorPoint)
    {
        for (int i = 0; i < anchorPoints.Count; i++)
        {
            if (anchorPoints[i] == anchorPoint)
            {
                GameObject item = storedItems[i];
                Destroy(item);
                storedItems.Remove(item);
            }
        }
    }

    private void Start()
    {
        
    }

    public void OnMouseDown()
    {
        SlotMenuManager.instance.ShowMenu();
        SlotMenuManager.instance.ListItemSlots(this);
    }
}
