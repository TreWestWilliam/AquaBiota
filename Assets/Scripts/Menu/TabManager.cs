using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    private GameObject activeTab;
    private Dictionary<string, GameObject> tabs;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        tabs = new Dictionary<string, GameObject>();
        foreach(Transform child in transform) {
            tabs.Add(child.name, child.gameObject);
            child.gameObject.SetActive(false);
        }
        //defaults to camera tab
        activeTab = tabs["CameraTab"];
        activeTab.SetActive(true);
    }

    public void switchTab(string name) {
        GameObject currentTab = activeTab;
        activeTab = tabs[name];
        currentTab.SetActive(false);
        activeTab.SetActive(true);
    }
}
