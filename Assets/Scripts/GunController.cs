using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GunController : MonoBehaviour
{
    [Required]
    public Camera playerCamera;

    public float damage = 10f;
    public float range = 100f;
    public float ammo = 12f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))   // When Fire1 button is hit (Mouse 1)
        {
            Fire();
        }

        void Fire()
        {
            RaycastHit target;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out target, range))    // Check if gun hit something
            {
                Debug.Log("Hit " + target.transform.name);


            }
        }
    }
}
