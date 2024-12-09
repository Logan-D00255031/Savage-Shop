using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotMenuManager : MonoBehaviour
{
    public static SlotMenuManager instance;

    public GameObject slotMenu;
    public Transform slotMenuContainer;
    public GameObject slotMenuItem;

    public InventoryToggle itemInventoryToggle;

    [ReadOnly]
    public ItemHolder currentItemHolder;

    private void Awake()
    {
        instance = this;
    }

    public void ListItemSlots(ItemHolder itemHolder)
    {
        currentItemHolder = itemHolder;

        CleanMenu();

        List<Transform> anchorPoints = currentItemHolder.anchorPoints;
        int count = 0;
        foreach (Transform itemSlot in anchorPoints)
        {
            GameObject obj = Instantiate(slotMenuItem, slotMenuContainer);
            var slotName = obj.transform.Find("SlotName").GetComponent<TMP_Text>();
            //Image slotIcon = obj.transform.Find("SlotIcon").GetComponent<Image>();

            slotName.text = itemSlot.gameObject.name;

            ItemSlotController slotController = obj.GetComponent<ItemSlotController>();
            slotController.slotMenuManager = this;
            slotController.SetSlotIndex(count);
            slotController.GetComponent<Button>().onClick.AddListener(slotController.ChooseItemFromMenu);
            count++;
        }

    }

    public void CleanMenu()
    {
        foreach (Transform item in slotMenuContainer)
        {
            Destroy(item.gameObject);
        }
    }

    public void ShowMenu()
    {
        Debug.Log("ShowMenu Called");

        RectTransform menuTransform = slotMenu.GetComponent<RectTransform>();
        Vector3 mousePos = Input.mousePosition;
        Debug.Log(mousePos);

        float pivotX = ((mousePos.x / Screen.width) <= 0.70) ? -0.3f : 1.3f;
        float pivotY = mousePos.y / Screen.height;

        menuTransform.pivot = new Vector2(pivotX, pivotY);

        Canvas canvas = slotMenu.GetComponentInParent<Canvas>();
        // Convert screen point to local point in canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            mousePos,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        slotMenu.transform.localPosition = localPoint;

        slotMenu.SetActive(true);
        //ListItemSlots(itemHolder.anchorPoints);
    }

    public void PlaceItem(GameObject prefab, int slotIndex)
    {
        currentItemHolder.PlaceItemAt(prefab, currentItemHolder.anchorPoints[slotIndex]);
    }
}
