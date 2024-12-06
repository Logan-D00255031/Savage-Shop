using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour, IButtonToggle
{
    public PrefabInventoryManager inventoryManager;
    public GameObject inventoryContainer;

    [ReadOnly, SerializeField]
    private bool active = false;

    public void Awake()
    {
        active = inventoryContainer.activeSelf;
    }

    public void Update()
    {
        active = inventoryContainer.activeSelf;
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleState();
        }
    }

    public void ToggleState()
    {
        if (!inventoryContainer.activeSelf)
        {
            Activate();
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
