using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : MonoBehaviour
{
    public GameObject menu;
    public Options options;

    public Slider CamVSensitivity;
    public Slider CamHSensitivity;
    public Toggle CamVInverseToggle;
    public Toggle CamHInverseToggle;
    public TMP_Text VSensText;
    public TMP_Text HSensText;

    public VolumeSetting masterVolume;
    public VolumeSetting musicVolume;
    public VolumeSetting ambientVolume;
    public VolumeSetting sfxVolume;
    public VolumeSetting uiVolume;


}
