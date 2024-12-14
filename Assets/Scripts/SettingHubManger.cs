using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SettingHubManager : MonoBehaviour
{
    public GameObject volumePanel;
    public GameObject controlsPanel;
    public GameObject graphicsPanel;

    public void ShowVolumePanel()
    {
        HideAllPanels();
        volumePanel.SetActive(true);
        Debug.Log("Volume Panel Activated");
    }

    public void ShowControlsPanel()
    {
        HideAllPanels();
        controlsPanel.SetActive(true);
        Debug.Log("Controls Panel Activated");
    }

    public void ShowGraphicsPanel()
    {
        HideAllPanels();
        graphicsPanel.SetActive(true);
        Debug.Log("Graphics Panel Activated");
    }

    private void HideAllPanels()
    {
        if (volumePanel != null) volumePanel.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (graphicsPanel != null) graphicsPanel.SetActive(false);
    }
}
