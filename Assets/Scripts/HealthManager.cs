using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 200f)]
    private float maxHealth = 50f;

    [SerializeField] 
    bool destroyOnDeath = true;

    [SerializeField, EnableIf("destroyOnDeath")]
    bool isObject = true;

    [SerializeField, EnableIf("isObject")]
    Transform centrePoint;

    [SerializeField]
    SFXManager.SFX deathSound;

    [ReadOnly, SerializeField]
    private float health;

    [ReadOnly]
    public bool damaged = false;


    private void Start()
    {
        health = maxHealth;
    }

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
        if (isObject)
        {
            PlacementSystem.instance.DestroyObject(centrePoint.position);
        }
        else
        {
            Destroy(gameObject);
        }
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

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public bool IsDead()
    {
        return health <= 0;
    }
}
