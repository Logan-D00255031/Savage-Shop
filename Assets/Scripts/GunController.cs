using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GunController : MonoBehaviour
{
    [EnableIf("isPlayer")]
    public Camera playerCamera;
    [DisableIf("isPlayer")]
    public GameObject objectView;
    [Required]
    public ParticleSystem muzzleFlash;

    public bool isPlayer = true;

    public float damage = 10f;
    public float range = 100f;
    [Range(1f, 100f)]
    public float rateOfFire = 50f;
    public float ammo = 12f;

    [ReadOnly, SerializeField]
    private float fireCooldown = 0f;

    // Update is called once per frame
    void Update()
    {
        // Only allow to activate if set to player
        if (isPlayer)
        {
            if (Input.GetButton("Fire1") && Time.time >= fireCooldown)   // When Fire1 button is activated and cooldown is over (Mouse 1)
            {
                fireCooldown = Time.time + 1f / rateOfFire; // Start cooldown
                //Debug.Log("Firing...");
                FireFromPlayer();
            }
        }
        
    }
    void FireFromPlayer()
    {
        muzzleFlash.Play(); // Activate muzzle flash

        RaycastHit target;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out target, range))    // Check if gun hit something
        {
            Debug.Log("Hit " + target.transform.name);

            Target targetComponent = target.transform.GetComponent<Target>();   // Look for Target Component
            if (targetComponent != null) // If it has Target Component
            {
                targetComponent.Damaged(damage);    // Deal damage to target
            }
        }
    }

    public void FireFromObject()
    {
        if (Time.time <= fireCooldown)
        {
            return;
        }
        fireCooldown = Time.time + 1f / rateOfFire;

        muzzleFlash.Play(); // Activate muzzle flash

        RaycastHit target;
        if (Physics.Raycast(objectView.transform.position, objectView.transform.forward, out target, range))    // Check if gun hit something
        {
            Debug.Log("Hit " + target.transform.name);

            Target targetComponent = target.transform.GetComponent<Target>();   // Look for Target Component
            if (targetComponent != null) // If it has Target Component
            {
                targetComponent.Damaged(damage);    // Deal damage to target
            }
        }
    }
}
