using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 200f)]
    private float health = 50f;

    [SerializeField] 
    bool destroyOnDeath = true;

    [SerializeField]
    SFXManager.SFX deathSound;

    [ReadOnly]
    public bool damaged = false;

    public void TakeDamage(float damage)
    {
        if ((health - damage) < 0)
        {
            health = 0;
        }
        else
        {
            health -= damage;
        }
        if (health <= 0 && destroyOnDeath)
        {
            Die();
        }
        StartCoroutine(ShowDamagedForSeconds(0.5f));
    }

    private void Die()
    {
        SFXManager.instance.PlaySFX(deathSound);
        Destroy(gameObject);
    }

    IEnumerator ShowDamagedForSeconds(float seconds)
    {
        damaged = true;
        yield return new WaitForSeconds(seconds);
        damaged = false;
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float health)
    {
        this.health = health;
    }

    public bool IsDead()
    {
        return health <= 0;
    }
}
