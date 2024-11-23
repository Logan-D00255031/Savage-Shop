using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
    public List<Transform> anchorPoints;
    public GameObject slotMenu;
    public Transform slotMenuContainer;
    public GameObject slotMenuItem;

    public void PlaceItemAt(GameObject itemPrefab, int anchorPointIndex)
    {
        Transform selectedAnchor = anchorPoints[anchorPointIndex];
        GameObject item = Instantiate(itemPrefab, selectedAnchor);
    }

    public void ListItemSlots()
    {
        foreach (Transform itemSlot in anchorPoints)
        {
            GameObject obj = Instantiate(slotMenuItem, slotMenuContainer);
            var slotName = obj.transform.Find("SlotName").GetComponent<TMP_Text>();
            //Image slotIcon = obj.transform.Find("SlotIcon").GetComponent<Image>();

            slotName.text = itemSlot.gameObject.name;
        }

    }
}
