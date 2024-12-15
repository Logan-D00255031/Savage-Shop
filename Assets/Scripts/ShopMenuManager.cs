using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuManager : MonoBehaviour
{
    public PrefabDatabaseSO PrefabDatabase;
    public PrefabInventoryManager InventoryManager;

    public Transform ShopContainer;
    public GameObject ShopItem;

    [ReadOnly, SerializeField]
    private List<ShopItemController> shopItemControllers;

    public void ListItems()
    {
        // Instantiate items into the inventory UI
        foreach (PrefabData prefabData in PrefabDatabase.objectsData)
        {
            GameObject obj = Instantiate(ShopItem, ShopContainer);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            Image itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemAmount = obj.transform.Find("ItemAmount").GetComponent<TMP_Text>();
            var price = obj.transform.Find("Price").GetComponent<TMP_Text>();

            // Display Information
            itemName.text = prefabData.ItemData.Name;
            itemIcon.sprite = prefabData.ItemData.Icon;
            price.text = prefabData.ItemData.BuyPrice.ToString();

            // Show amount of that item in the inventory
            string amountInInventory = "0";
            bool notInInventory = true;
            ShopItemController controller = obj.GetComponent<ShopItemController>();

            List<InventoryData> inventoryData = InventoryManager.storedItemData;
            foreach (InventoryData item in inventoryData)
            {
                if (item.PrefabDatabaseID == prefabData.ID)
                {
                    notInInventory = false;
                    amountInInventory = item.AmountStored.ToString();
                    controller.amountStored = item.AmountStored;
                }
            }
            itemAmount.text = amountInInventory;

            if (notInInventory)
            {
                controller.amountStored = 0;
            }

            controller.inventoryManager = InventoryManager;
            controller.prefabID = prefabData.ID;
            controller.itemPrice = prefabData.ItemData.BuyPrice;

            
        }

        SetShopItems();
    }

    public void CleanInventory()
    {
        foreach (Transform inventoryItem in ShopContainer)
        {
            Destroy(inventoryItem.gameObject);
        }
        shopItemControllers.Clear();
    }

    public void SetShopItems()
    {
        foreach (Transform shopItem in ShopContainer)
        {
            ShopItemController controller = shopItem.GetComponent<ShopItemController>();
            shopItemControllers.Add(controller);
        }
    }
}
