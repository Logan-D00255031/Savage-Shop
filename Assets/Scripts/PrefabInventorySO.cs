using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class PrefabInventorySO : ScriptableObject
{
    public List<InventoryData> storedItemData = new();

    public int AddItem(PrefabData data)
    {
        // If list empty
        if (storedItemData.Count == 0)
        {
            // Add data to list
            storedItemData.Add(new InventoryData(data));
            return storedItemData.Count;
        }
        else
        {
            // Loop through list to find matching prefab data
            for (int i = 0; i < storedItemData.Count; i++)
            {
                // If found, increment amount stored and end method
                if (storedItemData[i].PrefabData == data)
                {
                    storedItemData[i].AddItem();
                    return i;
                }
            }
            // Otherwise add new inventory data to list
            storedItemData.Add(new InventoryData(data));
            return storedItemData.Count;
        }
    }

    public int RemoveItem(PrefabData data)
    {
        // If list empty
        if (storedItemData.Count == 0)
        {
            throw new System.Exception("List is empty");
        }
        else
        {
            // Loop through list to find matching prefab data
            for (int i = 0; i < storedItemData.Count; i++)
            {
                // If found, decrease amount stored
                if (storedItemData[i].PrefabData == data)
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
            throw new System.Exception("Prefab data not found in list");
        }
    }

}

[Serializable]
public class InventoryData
{
    [field: SerializeField]
    public PrefabData PrefabData { get; private set; }
    [field: SerializeField]
    public int AmountStored { get; private set; }

    public InventoryData(PrefabData prefabData, int amountStored)
    {
        this.PrefabData = prefabData;
        this.AmountStored = amountStored;
    }

    public InventoryData(PrefabData prefabData)
    {
        this.PrefabData = prefabData;
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
