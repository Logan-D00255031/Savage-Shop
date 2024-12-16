using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceSetter : MonoBehaviour
{
    [SerializeField]
    private float newSellPrice;

    [ReadOnly]
    public int itemId;

    [SerializeField]
    private ItemSellPriceDataBase priceData;

    public void SetPriceFromInputField(string newPrice)
    {
        newSellPrice = float.Parse(newPrice);

        foreach (ItemSellPriceData priceData in priceData.itemSellPrices)
        {
            if (priceData.ID == itemId)
            {
                priceData.SetSellPrice(newSellPrice);
            }
        }
    }
}
