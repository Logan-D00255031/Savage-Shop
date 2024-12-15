using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopToggle : MonoBehaviour, IButtonToggle
{
    public ShopMenuManager shopMenuManager;
    public GameObject shopContainer;

    public bool isMainToggle;

    [EnableIf("isMainToggle")]
    public KeyCode keyBind;
    // The tab that the main toggle will open
    [EnableIf("isMainToggle")]
    public GameObject ActiveTabButton;
    // Other tabs the shop contains
    [EnableIf("isMainToggle")]
    public List<GameObject> OtherTabButtons;
    [EnableIf("isMainToggle")]
    public GameObject inventoryButtonsContainer;
    private List<InventoryToggle> inventoryToggles = new();

    [ReadOnly, SerializeField]
    private bool active = false;

    public void Awake()
    {
        active = shopContainer.activeSelf;

        // Grab all inventory toggles
        InventoryToggle[] toggles = inventoryButtonsContainer.GetComponentsInChildren<InventoryToggle>();
        foreach (InventoryToggle toggle in toggles)
        {
                inventoryToggles.Add(toggle);
        }

        // Grab all shop toggles except this one to the list
        //ShopToggle[] shopToggles = inventoryButtonsContainer.GetComponentsInChildren<ShopToggle>();
        //foreach (InventoryToggle toggle in toggles)
        //{
        //    if (toggle != this)
        //    {
        //        shopToggles.Add(toggle);
        //    }
        //}
    }

    public void Update()
    {
        active = shopContainer.activeSelf;
        if (isMainToggle)
        {
            if (Input.GetKeyDown(keyBind))
            {
                ToggleState();
            }
        }

        // Close inventory if slot menu is opened
        if (SlotMenuManager.instance.slotMenu.activeSelf && shopContainer.activeSelf)
        {
            Deactivate();
        }
    }

    public void ToggleState()
    {
        if (!shopContainer.activeSelf)
        {
            Activate();
            // Deactivate any other active inventories to prevent overlapping
            foreach (InventoryToggle toggle in inventoryToggles)
            {
                if (toggle.inventoryContainer.activeSelf)
                {
                    toggle.Deactivate();
                }
            }
        }
        else
        {
            Deactivate();
        }
    }

    public void Activate()
    {
        SFXManager.instance.PlaySFX(SFXManager.SFX.MenuClick);

        shopContainer.SetActive(true);
        shopMenuManager.ListItems();

        // Slot menu should be closed if inventory is opened
        if (SlotMenuManager.instance.slotMenu.activeSelf)
        {
            SlotMenuManager.instance.CloseMenu();
        }

        // Exit build mode if currently active
        if (PlacementSystem.instance.ActiveBuildState())
        {
            PlacementSystem.instance.ExitBuildMode();
        }

        // Set tab buttons state
        if (isMainToggle)
        {
            ActiveTabButton.SetActive(false);

            foreach (GameObject tab in OtherTabButtons)
            {
                tab.SetActive(true);
            }
        }

        //active = true;
        Debug.Log("Activated Inventory");
    }

    public void Deactivate()
    {
        shopContainer.SetActive(false);
        shopMenuManager.CleanInventory();

        //if (isMainToggle)
        //{
        //    foreach (ShopToggle toggle in shopToggles)
        //    {
        //        toggle.gameObject.SetActive(false);
        //    }
        //}

        //active = false;
        Debug.Log("Deactivated Inventory");
    }

    public bool isActive()
    {
        return shopContainer.activeSelf;
    }
}
