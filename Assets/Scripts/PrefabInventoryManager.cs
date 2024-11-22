using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PrefabInventoryManager : MonoBehaviour
{
    public static PrefabInventoryManager instance;
    public PrefabDatabaseSO PrefabDatabase;
    public List<InventoryData> storedItemData = new();

    public Transform InventoryContainer;
    public GameObject InventoryItem;

    private void Awake()
    {
        instance = this;
    }

    public int AddItem(int prefabIndex)
    {
        // If list empty
        if (storedItemData.Count == 0)
        {
            // Add index to list
            storedItemData.Add(new InventoryData(prefabIndex));
            return storedItemData.Count;
        }
        else
        {
            // Loop through list to find matching prefab index
            for (int i = 0; i < storedItemData.Count; i++)
            {
                // If found, increment amount stored and end method
                if (storedItemData[i].PrefabDatabaseID == prefabIndex)
                {
                    storedItemData[i].AddItem();
                    return i;
                }
            }
            // Otherwise add new inventory data to list
            storedItemData.Add(new InventoryData(prefabIndex));
            return storedItemData.Count;
        }
    }

    public int RemoveItem(int prefabIndex)
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
                if (storedItemData[i].PrefabDatabaseID == prefabIndex)
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
        // Remove the previous instantiated items from the inventory UI to prevent duplicates
        foreach(Transform item in InventoryContainer)
        {
            Destroy(item.gameObject);
        }
        // Instantiate items into the inventory UI
        foreach (InventoryData itemData in storedItemData)
        {
            GameObject obj = Instantiate(InventoryItem, InventoryContainer);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            Image itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            // Use the stored prefab ID to get the Index in the prefab database so we can get the item's information to display
            int prefabIndex = PrefabDatabase.objectsData.FindIndex(data => data.ID == itemData.PrefabDatabaseID);
            itemName.text = PrefabDatabase.objectsData[prefabIndex].ItemData.Name;
            itemIcon.sprite = PrefabDatabase.objectsData[prefabIndex].ItemData.Icon;
        }
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
