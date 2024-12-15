using GD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveToggle : MonoBehaviour, IButtonToggle
{
    public GameEvent firstPerson;
    public GameEvent isometric;

    public KeyCode keyBind;

    public void ToggleState()
    {
        SFXManager.instance.PlaySFX(SFXManager.SFX.MenuClick);

        if (IsometricCamera.IsActive())
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

    public void Activate()
    {
        isometric?.Raise();
    }

    public void Deactivate()
    {
        firstPerson?.Raise();
    }
}
