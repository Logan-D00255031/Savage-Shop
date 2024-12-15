using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public GameObject volumePanel; // The main settings panel (also acting as the Volume panel)
    public GameObject controlsPanel; // Assign in Unity
    public GameObject graphicsPanel; // Assign in Unity
    public GameObject creditsPanel; // Assign in Unity

    // Master Volume
    public TMP_Text masterVolumeText;
    public UnityEngine.UI.Slider masterVolumeSlider;

    // Music Volume
    public TMP_Text musicVolumeText;
    public UnityEngine.UI.Slider musicVolumeSlider;

    // UI Volume
    public TMP_Text uiVolumeText;
    public UnityEngine.UI.Slider uiVolumeSlider;

    // Open and Close Settings (Volume Panel acts as main settings panel)
    public void OpenSettings()
    {
        volumePanel.SetActive(true);
        ShowVolumePanel(); // Default view on opening
    }

    public void CloseSettings()
    {
        volumePanel.SetActive(false);
        controlsPanel.SetActive(false);
        graphicsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    // Open and Close Credits Panel
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        HideAllPanelsExcept(creditsPanel);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    // Switch Panels
    public void ShowVolumePanel()
    {
        HideAllPanelsExcept(volumePanel);
    }

    public void ShowControlsPanel()
    {
        HideAllPanelsExcept(controlsPanel);
    }

    public void ShowGraphicsPanel()
    {
        HideAllPanelsExcept(graphicsPanel);
    }

    // Hide all panels except the one passed as a parameter
    private void HideAllPanelsExcept(GameObject activePanel)
    {
        if (volumePanel != activePanel) volumePanel.SetActive(false);
        if (controlsPanel != activePanel) controlsPanel.SetActive(false);
        if (graphicsPanel != activePanel) graphicsPanel.SetActive(false);
        if (creditsPanel != activePanel) creditsPanel.SetActive(false);

        if (activePanel != null)
        {
            activePanel.SetActive(true);
        }
    }

    // Adjust Master Volume
    public void AdjustMasterVolume(float value)
    {
        AudioListener.volume = value; // Updates global volume
        if (masterVolumeText != null)
        {
            masterVolumeText.text = Mathf.RoundToInt(value * 100).ToString();
        }
    }

    // Adjust Music Volume
    public void AdjustMusicVolume(float value)
    {
        if (musicVolumeText != null)
        {
            musicVolumeText.text = Mathf.RoundToInt(value * 100).ToString();
        }
    }

    // Adjust UI Volume
    public void AdjustUIVolume(float value)
    {
        if (uiVolumeText != null)
        {
            uiVolumeText.text = Mathf.RoundToInt(value * 100).ToString();
        }
    }
}







