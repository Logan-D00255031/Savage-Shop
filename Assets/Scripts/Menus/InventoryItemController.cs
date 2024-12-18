using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InventoryItemController : MonoBehaviour
{
    public PrefabInventoryManager inventoryManager;
    public InventoryData inventoryData;

    // Used when adding an item to a slot
    public ItemSlotController itemSlot;

    public void RemoveItem(bool sell)
    {
        if (sell)
        {
            SFXManager.instance.PlaySFX(SFXManager.SFX.BuyItem);

            // Return 90% of the buy price of the item back to wallet
            int prefabIndex = inventoryManager.PrefabDatabase.objectsData.FindIndex(data => data.ID == inventoryData.PrefabDatabaseID);
            float buyPrice = inventoryManager.PrefabDatabase.objectsData[prefabIndex].ItemData.BuyPrice;
            WalletManager.instance.AddToWallet(buyPrice * 0.9f);
        }

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

        SFXManager.instance.PlaySFX(SFXManager.SFX.PlaceItem);

        itemSlot.PlaceItem(prefab, inventoryData.PrefabDatabaseID);
        RemoveItem(false);
    }

    public void PlaceObject()
    {
        PlacementSystem.instance.StartPlacement(inventoryData.PrefabDatabaseID);
        SFXManager.instance.PlaySFX(SFXManager.SFX.MenuClick);
    }
}
