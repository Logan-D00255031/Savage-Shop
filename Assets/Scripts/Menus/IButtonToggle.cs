using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IButtonToggle
{
    public void ToggleState();

    public void Activate();

    public void Deactivate();
}
