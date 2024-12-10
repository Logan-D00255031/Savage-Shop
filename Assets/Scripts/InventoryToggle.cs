using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour, IButtonToggle
{
    public PrefabInventoryManager inventoryManager;
    public GameObject inventoryContainer;

    public GameObject inventoryButtonsContainer;
    private List<InventoryToggle> inventoryToggles = new();

    public KeyCode keyBind;

    [SerializeField]    // Used to choose if it should place an item from the inventory when it's clicked
    private bool canPlaceFromMenu = false;

    [ReadOnly, SerializeField]
    private bool active = false;

    public void Awake()
    {
        active = inventoryContainer.activeSelf;

        // Grab all inventory toggles except this one to the list
        InventoryToggle[] toggles = inventoryButtonsContainer.GetComponentsInChildren<InventoryToggle>();
        foreach (InventoryToggle toggle in toggles)
        {
            if (toggle != this)
            { 
                inventoryToggles.Add(toggle); 
            }
        }
    }

    public void Update()
    {
        active = inventoryContainer.activeSelf;
        if (Input.GetKeyDown(keyBind))
        {
            ToggleState();
        }

        // Close inventory if slot menu is opened
        if (SlotMenuManager.instance.slotMenu.activeSelf)
        {
            Deactivate();
        }
    }

    public void ToggleState()
    {
        if (!inventoryContainer.activeSelf)
        {
            Activate();
            if (canPlaceFromMenu)
            {
                EnableObjectPlacement();
            }
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
        inventoryContainer.SetActive(true);
        inventoryManager.ListItems();
        
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
        //active = true;
        Debug.Log("Activated Inventory");
    }

    public void Deactivate()
    {
        inventoryContainer.SetActive(false);
        inventoryManager.CleanInventory();
        //active = false;
        Debug.Log("Deactivated Inventory");
    }

    public void EnableObjectPlacement()
    {
        inventoryManager.AllowObjectPlacement();

        List<Button> buttons = inventoryManager.GetInventoryItemButtons();
        foreach (Button button in buttons)
        {
            // Close menu to prevent repeat actions
            button.onClick.AddListener(Deactivate);
        }
        Debug.Log("Deactivate Listener Added");
    }
}
