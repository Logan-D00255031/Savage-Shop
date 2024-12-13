using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 200f)]
    private float health = 50f;

    [ReadOnly]
    public bool damaged = false;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        StartCoroutine(ShowDamagedForSeconds(0.5f));
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator ShowDamagedForSeconds(float seconds)
    {
        damaged = true;
        yield return new WaitForSeconds(seconds);
        damaged = false;
    }
}
