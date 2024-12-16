using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunInfoHandler : MonoBehaviour
{
    [SerializeField, Required]
    private GunController controller;

    [SerializeField, Required]
    private TMP_Text text;

    private int ammo;

    // Start is called before the first frame update
    void Start()
    {
        ammo = controller.GetAmmo();
        text.text = string.Format("{1}/{0}", controller.magSize, ammo);
    }

    // Update is called once per frame
    void Update()
    {
        if (ammo != controller.GetAmmo())
        {
            ammo = controller.GetAmmo();
            text.text = string.Format("{1}/{0}", controller.magSize, ammo);
        }
    }
}
