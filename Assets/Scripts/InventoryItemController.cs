using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    InventoryData inventoryData;

    public void RemoveItem()
    {
        PrefabInventoryManager.instance.RemoveItem(inventoryData.PrefabDatabaseID);
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
}
