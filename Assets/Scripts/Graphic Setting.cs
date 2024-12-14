using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;

    void Start()
    {
        // Populate Resolutions
        PopulateResolutions();

        // Populate Quality Levels
        PopulateQualityLevels();

        // Set Fullscreen Toggle
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    private void PopulateResolutions()
    {
        if (resolutionDropdown == null)
        {
            Debug.LogError("Resolution Dropdown is not assigned!");
            return;
        }

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolutions[i].width + " x " + resolutions[i].height));
        }

        resolutionDropdown.value = resolutions.Length - 1;
        resolutionDropdown.RefreshShownValue();
    }

    private void PopulateQualityLevels()
    {
        if (qualityDropdown == null)
        {
            Debug.LogError("Quality Dropdown is not assigned!");
            return;
        }

        qualityDropdown.ClearOptions();

        string[] qualityNames = QualitySettings.names;
        foreach (string name in qualityNames)
        {
            qualityDropdown.options.Add(new Dropdown.OptionData(name));
        }

        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    public void ApplyGraphicsSettings()
    {
        if (resolutionDropdown != null)
        {
            Resolution resolution = resolutions[resolutionDropdown.value];
            Screen.SetResolution(resolution.width, resolution.height, fullscreenToggle.isOn);
        }

        if (qualityDropdown != null)
        {
            QualitySettings.SetQualityLevel(qualityDropdown.value);
        }

        Debug.Log("Graphics settings applied.");
    }
}


