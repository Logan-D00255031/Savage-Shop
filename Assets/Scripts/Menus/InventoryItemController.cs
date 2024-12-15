using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    public PrefabInventoryManager inventoryManager;
    public InventoryData inventoryData;

    // Used when adding an item to a slot
    public ItemSlotController itemSlot;

    public void RemoveItem()
    {
        inventoryManager.RemoveItem(inventoryData.PrefabDatabaseID);
        //Debug.Log($"Amount: {inventoryData.AmountStored}");
        if (inventoryData.AmountStored <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            GameObject obj = this.gameObject;
            var itemAmount = obj.transform.Find("ItemAmount").GetComponent<TMP_Text>();
            itemAmount.text = inventoryData.AmountStored.ToString();
        }
    }

    public void SetItem(InventoryData inventoryData)
    {
        this.inventoryData = inventoryData;
    }

    public void PlaceItem()
    {
        PrefabDatabaseSO prefabDatabase = inventoryManager.PrefabDatabase;
        int prefabIndex = prefabDatabase.objectsData.FindIndex(data => data.ID == inventoryData.PrefabDatabaseID);
        GameObject prefab = prefabDatabase.objectsData[prefabIndex].Prefab;

        itemSlot.PlaceItem(prefab, inventoryData.PrefabDatabaseID);
        RemoveItem();
    }

    public void PlaceObject()
    {
        PlacementSystem.instance.StartPlacement(inventoryData.PrefabDatabaseID);
        SFXManager.instance.PlaySFX(SFXManager.SFX.MenuClick);
    }
}
