using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSellPriceDataBase : ScriptableObject
{
    public List<ItemSellPriceData> itemSellPrices;
}

[Serializable]
public class ItemSellPriceData
{
    public ItemSellPriceData(int prefabDatabaseID, float sellPrice)
    {
        this.ID = prefabDatabaseID;
        this.SellPrice = sellPrice;
    }

    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public float SellPrice { get; private set; }

    public void SetSellPrice(float sellPrice)
    {
        this.SellPrice = sellPrice;
    }
}