using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class PrefabInventorySO : ScriptableObject
{
    public List<InventoryData> storedItemData = new();

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
                if (storedItemData[i].PrefabDatabaseIndex == prefabIndex)
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
                if (storedItemData[i].PrefabDatabaseIndex == prefabIndex)
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

}

[Serializable]
public class InventoryData
{
    [field: SerializeField]
    public int PrefabDatabaseIndex { get; private set; }
    [field: SerializeField]
    public int AmountStored { get; private set; }

    public InventoryData(int PrefabDatabaseIndex, int amountStored)
    {
        this.PrefabDatabaseIndex = PrefabDatabaseIndex;
        this.AmountStored = amountStored;
    }

    public InventoryData(int PrefabDatabaseIndex)
    {
        this.PrefabDatabaseIndex = PrefabDatabaseIndex;
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
