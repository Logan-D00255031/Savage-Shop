using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class PrefabInventoryManager : MonoBehaviour
{
    //public static PrefabInventoryManager instance;
    public PrefabDatabaseSO PrefabDatabase;
    public List<InventoryData> storedItemData = new();

    public Transform InventoryContainer;
    public GameObject InventoryItem;

    [SerializeField]
    private bool canSetPriceFromMenu = false;

    [EnableIf("canSetPriceFromMenu")]
    public ItemSellPriceDataBase sellPriceDataBase;

    [ReadOnly, SerializeField]
    private InventoryItemController[] InventoryItems;

    //private void Awake()
    //{
    //    instance = this;
    //}

    public int AddItem(int prefabID)
    {
        // If list empty
        if (storedItemData.Count == 0)
        {
            // Add index to list
            storedItemData.Add(new InventoryData(prefabID));
            return storedItemData.Count - 1;
        }
        else
        {
            // Loop through list to find matching prefab index
            for (int i = 0; i < storedItemData.Count; i++)
            {
                // If found, increment amount stored and end method
                if (storedItemData[i].PrefabDatabaseID == prefabID)
                {
                    storedItemData[i].AddItem();
                    return i;
                }
            }
            // Otherwise add new inventory data to list
            storedItemData.Add(new InventoryData(prefabID));
            return storedItemData.Count - 1;
        }
    }

    public int RemoveItem(int prefabID)
    {
        // If list empty
        if (storedItemData.Count == 0)
        {
            throw new System.Exception("List is empty");
        }
        else
        {
            // Loop through list to find matching prefab intex
            for (int i = 0; i < storedItemData.Count; i++)
            {
                // If found, decrease amount stored
                if (storedItemData[i].PrefabDatabaseID == prefabID)
                {
                    storedItemData[i].RemoveItem();
                    // if last of that item removed, remove from list
                    if (storedItemData[i].AmountStored <= 0)
                    {
                        storedItemData.RemoveAt(i);
                        return -1;
                    }
                    return i;
                }
            }
            // Otherwise throw exception
            throw new System.Exception("PrefabDatabase Index not found in list");
        }
    }

    public void ListItems()
    {
        //CleanInventory();
        // Instantiate items into the inventory UI
        foreach (InventoryData itemData in storedItemData)
        {
            GameObject obj = Instantiate(InventoryItem, InventoryContainer);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            Image itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemAmount = obj.transform.Find("ItemAmount").GetComponent<TMP_Text>();

            // Use the stored prefab ID to get the Index in the prefab database so we can get the item's information to display
            int prefabIndex = PrefabDatabase.objectsData.FindIndex(data => data.ID == itemData.PrefabDatabaseID);
            itemName.text = PrefabDatabase.objectsData[prefabIndex].ItemData.Name;
            itemIcon.sprite = PrefabDatabase.objectsData[prefabIndex].ItemData.Icon;
            itemAmount.text = itemData.AmountStored.ToString();

            if (canSetPriceFromMenu)
            {
                var textField = obj.transform.Find("PriceSetter").GetComponent<TMP_InputField>();

                if (textField != null)
                {
                    float sellPrice = 0;
                    bool priceSet = false;
                    foreach (ItemSellPriceData sellPriceData in sellPriceDataBase.itemSellPrices)
                    {
                        if (sellPriceData.ID == itemData.PrefabDatabaseID)
                        {
                            sellPrice = sellPriceData.SellPrice;
                            priceSet = true;
                        }
                    }
                    if (!priceSet)
                    {
                        sellPrice = PrefabDatabase.objectsData[prefabIndex].ItemData.BuyPrice;
                        sellPriceDataBase.itemSellPrices.Add(new ItemSellPriceData(itemData.PrefabDatabaseID, sellPrice));
                    }

                    textField.text = sellPrice.ToString("F2");

                    PriceSetter priceSetter = obj.transform.Find("PriceSetter").GetComponent<PriceSetter>();
                    priceSetter.itemId = itemData.PrefabDatabaseID;
                }
            }
        }

        SetInventoryItems();
    }

    public void CleanInventory()
    {
        // Remove the previous instantiated items from the inventory UI to prevent duplicates
        foreach (Transform item in InventoryContainer)
        {
            Destroy(item.gameObject);
        }
        //Debug.Log($"Items: {InventoryItems.Length}");
    }

    public void SetInventoryItems()
    {
        InventoryItemController[] newItems = InventoryContainer.GetComponentsInChildren<InventoryItemController>();

        for (int i = 0; i < storedItemData.Count; i++)
        {
            newItems[i].SetItem(storedItemData[i]);
            newItems[i].inventoryManager = this;
        }
        InventoryItems = newItems;
        //Debug.Log($"Items: {InventoryItems.Length}");
    }

    public List<Button> GetInventoryItemButtons()
    {
        List<Button> buttons = new();
        foreach (Transform item in InventoryContainer)
        {
            buttons.Add(item.GetComponent<UnityEngine.UI.Button>());
        }

        return buttons;
    }
    public List<InventoryItemController> GetInventoryItems()
    {
        List<InventoryItemController> items = new();
        foreach (Transform item in InventoryContainer)
        {
            items.Add(item.GetComponent<InventoryItemController>());
        }

        return items;
    }

    public void AllowObjectPlacement()
    {
        foreach(Transform item in InventoryContainer)
        {
            InventoryItemController itemController = item.GetComponent<InventoryItemController>();
            item.GetComponent<Button>().onClick.AddListener(itemController.PlaceObject);
        }
        Debug.Log("PlaceObject Listener Added");
    }

    public bool ContainsItemWithID(int ID)
    {
        // No items in inventory, so it's not there
        if (storedItemData.Count == 0)
        {
            return false;
        }

        foreach (InventoryData data in storedItemData)
        {
            if (data.PrefabDatabaseID == ID)
            {
                return true;
            }
        }
        return false;
    }
}

[Serializable]
public class InventoryData
{
    [field: SerializeField]
    public int PrefabDatabaseID { get; private set; }
    [field: SerializeField]
    public int AmountStored { get; private set; }

    public InventoryData(int PrefabDatabaseIndex, int amountStored)
    {
        this.PrefabDatabaseID = PrefabDatabaseIndex;
        this.AmountStored = amountStored;
    }

    public InventoryData(int PrefabDatabaseIndex)
    {
        this.PrefabDatabaseID = PrefabDatabaseIndex;
        this.AmountStored = 1;
    }

    public void AddItem()
    {
        AmountStored++;
    }

    public void AddItem(int amount)
    {
        AmountStored += amount;
    }
    public void RemoveItem()
    {
        AmountStored--;
    }

    public void RemoveItem(int amount)
    {
        AmountStored -= amount;
    }

}
