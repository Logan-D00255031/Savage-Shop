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

    //public GameObject slotMenu;
    //public Transform slotMenuContainer;
    //public GameObject slotMenuItem;

    private Dictionary<Transform, GameObject> storedItems = new();
    private Dictionary<GameObject, int> storedItemIDs = new();

    private bool canOpenMenu = false;

    public void PlaceItemAt(GameObject itemPrefab, Transform anchorPoint, int prefabDatabaseID)
    {
        GameObject item = Instantiate(itemPrefab, anchorPoint);
        storedItems.Add(anchorPoint, item);
        storedItemIDs.Add(item, prefabDatabaseID);
    }

    public void RemoveItemIn(Transform anchorPoint)
    {
        GameObject item = storedItems[anchorPoint];
        // Add item back into inventory
        int ID = storedItemIDs[item];
        SlotMenuManager.instance.inventoryManager.AddItem(ID);
        storedItemIDs.Remove(item);
        // Remove item from slot
        Destroy(item);
        storedItems.Remove(anchorPoint);
    }

    private void Start()
    {
        // Disable opening the menu temporarily when created to prevent it being opened immediately
        StartCoroutine(AllowMenuToOpenAfterDelay(0.5f));
    }

    private IEnumerator AllowMenuToOpenAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canOpenMenu = true;
    }

    public void OnMouseDown()
    {
        // TODO - Add Mouse Over UI Check

        // Menu should not open if in object build mode
        bool noBuildState = !PlacementSystem.instance.isBuildState;
        if (canOpenMenu && noBuildState)
        {
            SlotMenuManager.instance.ShowMenu();
            SlotMenuManager.instance.ListItemSlots(this);
        }
    }

    public void OnDestroy()
    {
        // Add items in slots back to inventory
        foreach (GameObject item in storedItems.Values) {
            int ID = storedItemIDs[item];
            SlotMenuManager.instance.inventoryManager.AddItem(ID);
        }
    }

    public bool IsSlotOccupied(Transform itemSlot)
    {
        return storedItems.ContainsKey(itemSlot);
    }

    public int GetSlotItemPrefabID(Transform itemSlot)
    {
        return storedItemIDs[storedItems[itemSlot]];
    }
}
