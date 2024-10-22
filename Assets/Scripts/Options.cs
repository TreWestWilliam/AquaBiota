using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    [Header("Primary")]
    public static Options Instance;
    public Settings _Settings;
    public CameraOrbit Camera;
    public GameObject MenuGO;

    [Header("UI")]
    [SerializeField] private Slider CamVSensitivity;
    [SerializeField] private Slider CamHSensitivity;
    [SerializeField] private Toggle CamVInverseToggle;
    [SerializeField] private Toggle CamHInverseToggle;
    //[SerializeField] private 

    void Awake()
    {
        if (Instance != null) { Destroy(this.gameObject); }
        Instance = this;

        // TODO: Saving stuff 

        //Loading Values onto stuff;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleCameraVertical(bool t) 
    {
        _Settings.CameraVInverse = t;
    }
    public void ToggleCameraHorizontal(bool t)
    {
        _Settings.CameraHInverse = t;
    }

    public void OpenMenu() 
    {
        CamVSensitivity.value = _Settings.CameraV;
        CamHSensitivity.value = _Settings.CameraH;
        CamVInverseToggle.isOn = _Settings.CameraVInverse;
        CamHInverseToggle.isOn = _Settings.CameraHInverse;
        MenuGO.SetActive(true);
    }

    public void CloseMenu() 
    {
        MenuGO.SetActive(false);
    }

    public void ApplySettings() 
    {
        _Settings.CameraV = CamVSensitivity.value;
        _Settings.CameraH = CamHSensitivity.value; 
        Camera.LoadSettings(_Settings);
       
    }
}

[System.Serializable]
public struct Settings 
{
    public float CameraV;
    public float CameraH;
    public bool CameraVInverse;
    public bool CameraHInverse;
}