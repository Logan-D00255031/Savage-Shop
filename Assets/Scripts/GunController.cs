using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GunController : MonoBehaviour
{
    [Required]
    public Camera playerCamera;
    [Required]
    public ParticleSystem muzzleFlash;

    public float damage = 10f;
    public float range = 100f;
    [Range(1f, 100f)]
    public float rateOfFire = 50f;
    public float ammo = 12f;

    [ReadOnly]
    private float fireCooldown = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= fireCooldown)   // When Fire1 button is activated and cooldown is over (Mouse 1)
        {
            fireCooldown = Time.time + 1f / rateOfFire; // Start cooldown
            Fire();
        }

        void Fire()
        {
            muzzleFlash.Play(); // Activate muzzle flash

            RaycastHit target;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out target, range))    // Check if gun hit something
            {
                Debug.Log("Hit " + target.transform.name);

                Target targetComponent = target.transform.GetComponent<Target>();   // Look for Target Component
                if(targetComponent != null) // If it has Target Component
                {
                    targetComponent.Damaged(damage);    // Deal damage to target
                }
            }
        }
    }
}
