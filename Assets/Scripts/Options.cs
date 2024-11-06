using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

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
    [SerializeField] private TMP_Text VSensText;
    [SerializeField] private TMP_Text HSensText;
    //[SerializeField] private 

    private string FilePath = "notloaded";

    void Awake()
    {
        FilePath = Application.persistentDataPath + "/settings.xml";
        if (Instance != null) { Destroy(this.gameObject); }
        Instance = this;

        // TODO: Saving stuff 

        if (File.Exists(FilePath))
        {
            _Settings = LoadSettings();
            LoadValues();
            Camera.LoadSettings(_Settings);
        }
        else 
        {
            // This is using the presets in the scene to make a default settings
            SaveSettings();
        }

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

    public void LoadValues() 
    {
        
        if (CamVInverseToggle != null)
            CamVInverseToggle.isOn = _Settings.CameraVInverse;
        if (CamHInverseToggle != null)
            CamHInverseToggle.isOn = _Settings.CameraHInverse;
        
        if (VSensText != null)
            VSensText.text = $"{_Settings.CameraV:F2}";
        if (HSensText != null)
            HSensText.text = $"{_Settings.CameraH:F2}";
        
        if (CamVSensitivity !=null)
            CamVSensitivity.value = _Settings.CameraV;
        if (CamHSensitivity != null)
            CamHSensitivity.value = _Settings.CameraH;
    }

    public void OpenMenu() 
    {
        MenuGO.SetActive(true);
        LoadValues();
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
        SaveSettings();
    }
    /*
    private bool IsSettingsFileReady() 
    {
        
    }*/

    private void SaveSettings() 
    {
        FileStream FS = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
        XmlSerializer serial = new(typeof(Settings));
        TextWriter writer = new StreamWriter(FS);
        serial.Serialize(writer, _Settings);
        writer.Close();
        FS.Close();
    }

    private Settings LoadSettings() 
    {
        XmlSerializer xmlSerializer = new(typeof(Settings));
        using FileStream FS = new FileStream(FilePath, FileMode.Open);
        Settings s = (Settings)xmlSerializer.Deserialize(FS);
       

        return s;
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