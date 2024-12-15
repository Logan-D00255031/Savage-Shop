using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObjectToggle : MonoBehaviour, IButtonToggle
{
    [SerializeField]
    private PlacementSystem placementSystem;

    public Transform inventoryButtonsContainer;
    private List<InventoryToggle> inventoryToggles = new();

    public ShopToggle mainShopToggle;

    public KeyCode keyBind;

    public void ToggleState()
    {
        if (placementSystem.isRemoveState)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(keyBind))
        {
            ToggleState();
        }
    }

    public void Awake()
    {
        // Grab all inventory toggles
        foreach (Transform button in inventoryButtonsContainer)
        {
            InventoryToggle toggle = button.GetComponent<InventoryToggle>();
            inventoryToggles.Add(toggle);
        }
    }

    public void Activate()
    {
        placementSystem.StartRemoval();
        SFXManager.instance.PlaySFX(SFXManager.SFX.MenuClick);

        // Inventories should not be open when remove mode is active
        foreach (InventoryToggle toggle in inventoryToggles)
        {
            if (toggle.inventoryContainer.activeSelf)
            {
                toggle.Deactivate();
            }
        }

        if (mainShopToggle.isActive())
        {
            mainShopToggle.Deactivate();
        }
    }

    public void Deactivate()
    {
        placementSystem.ExitBuildMode();
    }
}
