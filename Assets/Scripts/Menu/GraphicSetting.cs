using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicSetting : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private List<resItem> resolutions = new List<resItem>();

    private bool fullscreen;
    private int width, height;

    public void Awake() {
        fullscreen = Screen.fullScreen;
        width = Screen.width;
        height = Screen.height;
    }
    public void toggleFullscreen(bool t) {
        fullscreen = t;
    }
    public void getDropdownValue() {
        int val = dropdown.value;
        width = resolutions[val].width;
        height = resolutions[val].height;
    }
    public void applySettings() {
        Screen.SetResolution(width, height, fullscreen);
    }
}

[System.Serializable]
public class resItem {
    public int width, height;
}