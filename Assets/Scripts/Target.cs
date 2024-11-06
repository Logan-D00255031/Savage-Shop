using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    [Required]
    private HealthManager healthManager;

    public void Damaged(float damage)
    {
        healthManager.TakeDamage(damage);
    }
}
