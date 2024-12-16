using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public float delay = 5f; // Time to wait before transitioning

    void Start()
    {
        // Load Main Menu after a delay
        Invoke("LoadMainMenu", delay);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

