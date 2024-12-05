using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : MonoBehaviour
{
    public GameObject creditsPanel; // Assign in Unity

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
}
