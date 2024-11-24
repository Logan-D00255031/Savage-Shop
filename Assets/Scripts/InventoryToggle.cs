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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleState();
        }
    }

    public void ToggleState()
    {
        if (!active)
        {
            active = true;
            Activate();
        }
        else
        {
            active = false;
            Deactivate();
        }
    }

    public void Activate()
    {
        inventoryContainer.SetActive(active);
        inventoryManager.ListItems();
    }

    public void Deactivate()
    {
        inventoryContainer.SetActive(active);
        inventoryManager.CleanInventory();
    }
}
