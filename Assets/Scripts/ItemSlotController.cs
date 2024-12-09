using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotController : MonoBehaviour
{
    public SlotMenuManager slotMenuManager;
    private int slotIndex;

    public void ChooseItemFromMenu()
    {
        InventoryToggle inventoryToggle = slotMenuManager.itemInventoryToggle;
        inventoryToggle.Activate();

        PrefabInventoryManager inventoryManager = inventoryToggle.inventoryManager;
        List<InventoryItemController> items = inventoryManager.GetInventoryItems();

        foreach (InventoryItemController item in items)
        {
            item.itemSlot = this;
            item.GetComponent<Button>().onClick.AddListener(item.PlaceItem);
        }
    }

    public int GetSlotIndex()
    {
        return slotIndex;
    }

    public void SetSlotIndex(int slotIndex)
    {
        this.slotIndex = slotIndex;
    }

    public void PlaceItem(GameObject prefab)
    {
        slotMenuManager.PlaceItem(prefab, slotIndex);
    }
}
