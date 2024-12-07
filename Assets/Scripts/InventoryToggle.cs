using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour, IButtonToggle
{
    public PrefabInventoryManager inventoryManager;
    public GameObject inventoryContainer;

    public GameObject inventoryButtonsContainer;
    private List<InventoryToggle> inventoryToggles = new();

    public KeyCode keyBind;

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
    }

    public void ToggleState()
    {
        if (!inventoryContainer.activeSelf)
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
        inventoryContainer.SetActive(true);
        inventoryManager.ListItems();
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
}
