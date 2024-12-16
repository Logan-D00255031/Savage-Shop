using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplayHandler : MonoBehaviour
{
    [SerializeField, Required]
    private HealthManager healthManager;

    [SerializeField, Required]
    private TMP_Text healthText;

    private float health;

    // Start is called before the first frame update
    void Start()
    {
        health = healthManager.GetHealth();
        healthText.text = string.Format("{1}/{0}", healthManager.GetMaxHealth(), health);
    }

    // Update is called once per frame
    void Update()
    {
        if (health != healthManager.GetHealth())
        {
            health = healthManager.GetHealth();
            healthText.text = string.Format("{1}/{0}", healthManager.GetMaxHealth(), health);
        }
    }
}
