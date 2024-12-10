using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotController : MonoBehaviour
{
    public SlotMenuManager slotMenuManager;
    private Transform itemSlot;

    [SerializeField]
    private GameObject deleteButtonObj;

    public void ChooseItemFromMenu()
    {
        InventoryToggle inventoryToggle = slotMenuManager.itemInventoryToggle;
        inventoryToggle.Activate();

        PrefabInventoryManager inventoryManager = inventoryToggle.inventoryManager;
        List<InventoryItemController> items = inventoryManager.GetInventoryItems();

        // Allow items in inventory be placed in slot when clicked
        foreach (InventoryItemController item in items)
        {
            item.itemSlot = this;
            item.GetComponent<Button>().onClick.AddListener(item.PlaceItem);
        }
        // Close the slot menu to prevent repeat actions before completing the current action
        slotMenuManager.CloseMenu();
    }

    public Transform GetItemSlot()
    {
        return itemSlot;
    }

    public void SetItemSlot(Transform itemSlot)
    {
        this.itemSlot = itemSlot;
    }

    public void PlaceItem(GameObject prefab, int prefabDatabaseID)
    {
        slotMenuManager.PlaceItem(prefab, itemSlot, prefabDatabaseID);
        slotMenuManager.itemInventoryToggle.Deactivate();
        slotMenuManager.OpenMenu();

        // Activate remove button in menu UI
        EnableDeleteButton();
        
    }

    private void RemoveItem()
    {
        slotMenuManager.RemoveFromSlot(itemSlot);

        // Disable remove button in menu UI
        DisableDeleteButton();
    }

    public void DisableDeleteButton()
    {
        Debug.Log("Delete Disabled");
        Button deleteButton = deleteButtonObj.GetComponent<Button>();
        deleteButton.onClick.RemoveAllListeners();
        deleteButtonObj.SetActive(false);
    }

    public void EnableDeleteButton()
    {
        Debug.Log("Delete Enabled");
        deleteButtonObj.SetActive(true);
        Button deleteButton = deleteButtonObj.GetComponent<Button>();
        deleteButton.onClick.AddListener(RemoveItem);
    }
}
