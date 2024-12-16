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

    public bool isPlayer = false;

    [SerializeField, EnableIf("isPlayer")]
    private LayerMask nonPlayerMasks;

    public float damage = 10f;
    public float range = 100f;
    [Range(1f, 100f)]
    public float rateOfFire = 5f;
    public int magSize = 12;
    public float reloadTime = 2f;

    [ReadOnly, SerializeField]
    private int ammo;
    [ReadOnly, SerializeField]
    private float fireCooldown = 0f;

    private void Start()
    {
        ammo = magSize;
    }

    // Update is called once per frame
    void Update()
    {
        // Only allow to activate if set to player
        if (isPlayer)
        {
            // Cannot fire if there's no ammo
            if (ammo > 0)
            {
                if (Input.GetButton("Fire1") && Time.time >= fireCooldown)   // When Fire1 button is activated and cooldown is over (Mouse 1)
                {
                    fireCooldown = Time.time + (1f / rateOfFire); // Start cooldown
                    //Debug.Log("Firing...");
                    //Debug.Log($"{Time.time} - {fireCooldown}");
                    FireFromPlayer();
                }
            }
        }
        
    }

    private IEnumerator Reload(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        ReloadMag();
        Debug.Log("Reloaded!");
    }

    public void ReloadMag()
    {
        ammo = magSize;
    }

    void FireFromPlayer()
    {
        muzzleFlash.Play(); // Activate muzzle flash
        SFXManager.instance.PlaySFX(SFXManager.SFX.GunShot);

        // Bullet expended
        ammo--;

        RaycastHit target;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out target, range, nonPlayerMasks))    // Check if gun hit something
        {
            Debug.Log("Hit " + target.transform.name);

            Target targetComponent = target.transform.GetComponent<Target>();   // Look for Target Component
            if (targetComponent != null) // If it has Target Component
            {
                targetComponent.Damaged(damage);    // Deal damage to target
            }
        }

        // Reload when out of ammo
        if (ammo <= 0)
        {
            StartCoroutine(Reload(reloadTime));
            Debug.Log("Reloading...");
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
        SFXManager.instance.PlaySFX(SFXManager.SFX.GunShot);

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
