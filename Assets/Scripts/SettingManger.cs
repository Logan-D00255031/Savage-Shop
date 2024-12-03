using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel; // The settings panel

    // Master Volume
    public TMP_Text masterVolumeText;
    public UnityEngine.UI.Slider masterVolumeSlider;

    // Music Volume
    public TMP_Text musicVolumeText;
    public UnityEngine.UI.Slider musicVolumeSlider;

    // UI Volume
    public TMP_Text uiVolumeText;
    public UnityEngine.UI.Slider uiVolumeSlider;

    // Called when opening the settings panel
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // Called when closing the settings panel
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
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
        // Placeholder for handling music-specific volume
        // Replace this with actual audio source management if applicable
        if (musicVolumeText != null)
        {
            musicVolumeText.text = Mathf.RoundToInt(value * 100).ToString();
        }
    }

    // Adjust UI Volume
    public void AdjustUIVolume(float value)
    {
        // Placeholder for handling UI-specific volume
        // Replace this with actual audio source management if applicable
        if (uiVolumeText != null)
        {
            uiVolumeText.text = Mathf.RoundToInt(value * 100).ToString();
        }
    }
}
