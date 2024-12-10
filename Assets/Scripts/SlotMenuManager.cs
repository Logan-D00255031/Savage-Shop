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
    public PrefabInventoryManager inventoryManager;

    private Dictionary<Transform, ItemSlotController> slotData = new();

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
        foreach (Transform itemSlot in anchorPoints)
        {
            // Add slot to menu
            GameObject obj = Instantiate(slotMenuItem, slotMenuContainer);
            var slotName = obj.transform.Find("SlotName").GetComponent<TMP_Text>();

            slotName.text = itemSlot.gameObject.name;

            // Set up the slotcontroller for the slot
            ItemSlotController slotController = obj.GetComponent<ItemSlotController>();
            slotController.slotMenuManager = this;
            slotController.SetItemSlot(itemSlot);
            // Debug.Log(slotController.GetItemSlot().position);

            // Check if slot is occupied
            if (!currentItemHolder.IsSlotOccupied(itemSlot))
            {
                // Allow item placement if empty
                slotController.GetComponent<Button>().onClick.AddListener(slotController.ChooseItemFromMenu);
            }
            else
            {
                // Enable delete button for slot controller if it contains an item
                slotController.EnableDeleteButton();
                SetItemData(itemSlot, obj);
            }

            // Add to dictionary so we can access them later
            slotData.Add(itemSlot, slotController);
        }

    }

    private void SetItemData(Transform itemSlot, GameObject obj)
    {
        int ID = currentItemHolder.GetSlotItemPrefabID(itemSlot);
        PrefabDatabaseSO prefabDatabase = inventoryManager.PrefabDatabase;
        int prefabIndex = prefabDatabase.objectsData.FindIndex(data => data.ID == ID);

        Image slotIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
        var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();

        slotIcon.sprite = prefabDatabase.objectsData[prefabIndex].ItemData.Icon;
        itemName.text = prefabDatabase.objectsData[prefabIndex].ItemData.Name;
    }

    private void SetDefaultData(GameObject obj)
    {
        Image slotIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
        var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();

        slotIcon.sprite = null;
        itemName.text = "None";
    }

    public void CleanMenu()
    {
        foreach (Transform item in slotMenuContainer)
        {
            Destroy(item.gameObject);
        }
        slotData.Clear();
    }

    public void ShowMenu()
    {
        Debug.Log("ShowMenu Called");

        RectTransform menuTransform = slotMenu.GetComponent<RectTransform>();
        Vector3 mousePos = Input.mousePosition;
        Debug.Log(mousePos);

        // Determine menu pivot from mouse to prevent it from going off screen
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
    }

    // These methods are used to hide/show the menu without resetting its data
    public void CloseMenu()
    {
        slotMenu.SetActive(false);
    }

    public void OpenMenu()
    {
        slotMenu.SetActive(true);
    }

    public void PlaceItem(GameObject prefab, Transform itemSlot, int prefabDatabaseID)
    {
        currentItemHolder.PlaceItemAt(prefab, itemSlot, prefabDatabaseID);
        // The slot is filled so the action is unavailable until the item is removed
        if (!slotData.ContainsKey(itemSlot))
        {
            ListItemSlots(currentItemHolder);
        }
        SetItemData(itemSlot, slotData[itemSlot].gameObject);
        slotData[itemSlot].GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void RemoveFromSlot(Transform itemSlot)
    {
        currentItemHolder.RemoveItemIn(itemSlot);

        ItemSlotController slotController = slotData[itemSlot];
        SetDefaultData(slotData[itemSlot].gameObject);
        // Item has been removed from slot, so it's now free for another item to choose
        slotController.GetComponent<Button>().onClick.AddListener(slotController.ChooseItemFromMenu);
        slotData.Remove(itemSlot);
    }
}
