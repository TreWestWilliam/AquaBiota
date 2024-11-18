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

    [SerializeField] private VolumeSetting masterVolume;
    [SerializeField] private VolumeSetting musicVolume;
    [SerializeField] private VolumeSetting ambientVolume;
    [SerializeField] private VolumeSetting sfxVolume;
    [SerializeField] private VolumeSetting uiVolume;
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
    public void ToggleMasterVol(bool t)
    {
        masterVolume.toggleVolume(ref _Settings, t);
    }
    public void ToggleMusicVol(bool t) {
        musicVolume.toggleVolume(ref _Settings, t);
    }
    public void ToggleAmbientVol(bool t) {
        ambientVolume.toggleVolume(ref _Settings, t);
    }
    public void ToggleSfxVol(bool t) {
        sfxVolume.toggleVolume(ref _Settings, t);
    }
    public void ToggleUiVol(bool t) {
        uiVolume.toggleVolume(ref _Settings, t);
    }

    public void LoadValues() 
    {
        //camera values
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

        //volume values
        masterVolume.loadVolume(ref _Settings);
        musicVolume.loadVolume(ref _Settings);
        ambientVolume.loadVolume(ref _Settings);
        sfxVolume.loadVolume(ref _Settings);
        uiVolume.loadVolume(ref _Settings);
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

        if(!masterVolume.VolumeToggle.isOn)
            _Settings.setVolumeVal(masterVolume.name, (int)masterVolume.VolumeSlider.value);
        if (!musicVolume.VolumeToggle.isOn)
            _Settings.setVolumeVal(musicVolume.name, (int)musicVolume.VolumeSlider.value);
        if (!ambientVolume.VolumeToggle.isOn)
            _Settings.setVolumeVal(ambientVolume.name, (int)ambientVolume.VolumeSlider.value);
        if (!sfxVolume.VolumeToggle.isOn)
            _Settings.setVolumeVal(sfxVolume.name, (int)sfxVolume.VolumeSlider.value);
        if (!uiVolume.VolumeToggle.isOn)
            _Settings.setVolumeVal(uiVolume.name, (int)uiVolume.VolumeSlider.value);
        
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
    //camera settings
    public float CameraV;
    public float CameraH;
    public bool CameraVInverse;
    public bool CameraHInverse;

    //volume settings
    public bool muteMasterVol;
    public int masterVolVal;    
    public bool muteMusicVol;
    public int musicVolVal;
    public bool muteAmbientVol;
    public int ambientVolVal;    
    public bool muteSfxVol;
    public int sfxVolVal;
    public bool muteUiVol;
    public int UiVolVal;

    public int getVolumeVal(string name) {
        switch (name) {
            case "Master Volume":
                return masterVolVal;
            case "Music Volume":
                return musicVolVal;
            case "Ambient Volume":
                return ambientVolVal;
            case "Sfx Volume":
                return sfxVolVal;
            case "Ui Volume":
                return UiVolVal;
            default:
                return -1;
        }
    }
    public bool getMuteVolume(string name) {
        switch (name) {
            case "Master Volume":
                return muteMasterVol;
            case "Music Volume":
                return muteMusicVol;
            case "Ambient Volume":
                return muteAmbientVol;
            case "Sfx Volume":
                return muteSfxVol;
            case "Ui Volume":
                return muteUiVol;
            default:
                return false;
        }
    }
    public void setMuteVolume(string name, bool t) {
        switch (name) {
            case "Master Volume":
                muteMasterVol=t;
                break;
            case "Music Volume":
                muteMusicVol=t;
                break;
            case "Ambient Volume":
                muteAmbientVol=t;
                break;
            case "Sfx Volume":
                muteSfxVol=t;
                break;
            case "Ui Volume":
                muteUiVol=t;
                break;
            default:
                break;
        }
    }
    public void setVolumeVal(string name, int val) {
        switch (name) {
            case "Master Volume":
                masterVolVal = val;
                break;
            case "Music Volume":
                musicVolVal = val;
                break;
            case "Ambient Volume":
                ambientVolVal = val;
                break;
            case "Sfx Volume":
                sfxVolVal = val;
                break;
            case "Ui Volume":
                UiVolVal = val;
                break;
            default:
                break;
        }
    }
}