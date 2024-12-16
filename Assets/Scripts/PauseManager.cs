using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // Assign your pause menu GameObject in the Inspector
    public GameObject settingsPanel; // Assign your settings panel here (optional)

    private bool isPaused = false;

    void Start()
    {
        // Ensure the pause menu is hidden when the game starts
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true); // Show the pause menu
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game by setting time scale to 1
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false); // Hide the pause menu
        }

        // Ensure settings panel is hidden when resuming the game
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true); // Show settings panel
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false); // Hide pause menu
        }

        Debug.Log("Opened Settings Panel");
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // Hide the settings panel
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit(); // Exits the application (only works in a built executable)
    }

    public void GoToMainMenu()
    {
        Debug.Log("Returning to the main menu...");
        Time.timeScale = 1f; // Ensure time scale is reset to normal
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your main menu scene name
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

}





