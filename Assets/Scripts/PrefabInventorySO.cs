using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class PrefabInventorySO : ScriptableObject
{
    public Dictionary<int, int> storedItemData;

    public void AddItem(int prefabDatabaseIndex)
    {
        if (storedItemData.ContainsKey(prefabDatabaseIndex))
        {
            storedItemData[prefabDatabaseIndex]++;
        }
        else
        {
            storedItemData.Add(prefabDatabaseIndex, 1);
        }
    }

    public int RemoveItem(int prefabDatabaseIndex)
    {
        if (storedItemData.ContainsKey(prefabDatabaseIndex))
        {
            storedItemData[prefabDatabaseIndex]--;
            return storedItemData[prefabDatabaseIndex];
        }
        throw new System.Exception($"No object index stored {prefabDatabaseIndex}");
    }

}

//[Serializable]
//public class InventoryData
//{
//    [field: SerializeField]
//    public ItemData itemData { get; private set; }
//    [field: SerializeField]
//    public int amountStored { get; private set; }

//    public void AddItem()
//    {
//        amountStored++;
//    }
//}
