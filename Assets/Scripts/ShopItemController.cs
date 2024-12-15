using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemController : MonoBehaviour
{
    public PrefabInventoryManager inventoryManager;
    
    public int prefabID;
    public int amountStored;
    public float itemPrice;

    public void BuyItem()
    {
        if ((WalletManager.instance.balance - itemPrice) <= 0)
        {
            SFXManager.instance.PlaySFX(SFXManager.SFX.Invalid);
            return;
        }
        // Deduct price from wallet
        WalletManager.instance.RemoveFromWallet(itemPrice);

        SFXManager.instance.PlaySFX(SFXManager.SFX.BuyItem);

        // Add item to inventory
        int invDataIndex = inventoryManager.AddItem(prefabID);
        // Get new amount stored
        InventoryData data = inventoryManager.storedItemData[invDataIndex];
        if (data != null)
        {
            amountStored = data.AmountStored;
        }
        else
        {
            amountStored = 0;
        }
        //Debug.Log($"Amount: {amountStored}");

        // Display new amount in inventory
        var itemAmount = gameObject.transform.Find("ItemAmount").GetComponent<TMP_Text>();
        itemAmount.text = amountStored.ToString();
    }
}
