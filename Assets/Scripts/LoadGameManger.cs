using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameManager : MonoBehaviour
{
    public void LoadGame()
    {
        // Replace "GameScene" with the name of the scene where the game loads
        SceneManager.LoadScene("GameScene");
    }
}

