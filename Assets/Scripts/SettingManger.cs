using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel; // The settings panel
    public GameObject volumePanel; // Assign in Unity
    public GameObject controlsPanel; // Assign in Unity
    public GameObject graphicsPanel; // Assign in Unity

    // Master Volume
    public TMP_Text masterVolumeText;
    public UnityEngine.UI.Slider masterVolumeSlider;

    // Music Volume
    public TMP_Text musicVolumeText;
    public UnityEngine.UI.Slider musicVolumeSlider;

    // UI Volume
    public TMP_Text uiVolumeText;
    public UnityEngine.UI.Slider uiVolumeSlider;

    public GameObject creditsPanel; // Assign in Unity

    // Open and Close Settings Panel
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        ShowVolumePanel(); // Default panel to open
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    // Open and Close Credits Panel
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    // Switch Panels
    public void ShowVolumePanel()
    {
        HideAllPanels();
        volumePanel.SetActive(true);
    }

    public void ShowControlsPanel()
    {
        HideAllPanels();
        controlsPanel.SetActive(true);
    }

    public void ShowGraphicsPanel()
    {
        HideAllPanels();
        graphicsPanel.SetActive(true);
    }

    private void HideAllPanels()
    {
        if (volumePanel != null) volumePanel.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (graphicsPanel != null) graphicsPanel.SetActive(false);
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




