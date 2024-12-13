using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicSetting : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private List<resItem> resolutions = new List<resItem>();
    [SerializeField] private Toggle fullscreenTog, vsyncTog;

    public void applySettings() {
        Screen.SetResolution(resolutions[dropdown.value].width, resolutions[dropdown.value].height, fullscreenTog.isOn);
        if (vsyncTog.isOn)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
    }
    
    public void saveSettings(ref Settings _Settings) {
        _Settings.fullscreen = fullscreenTog.isOn;
        _Settings.vsync = vsyncTog.isOn;
        _Settings.resolutionIndex = dropdown.value;
    }
    public void loadSettings(Settings _Settings) {
        fullscreenTog.isOn = _Settings.fullscreen;
        vsyncTog.isOn = _Settings.vsync;
        dropdown.value = _Settings.resolutionIndex;
        
        applySettings();
    }
}

[System.Serializable]
public class resItem {
    public int width, height;
}