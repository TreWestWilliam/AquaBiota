using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSetting : MonoBehaviour {
    public Slider VolumeSlider;
    public Toggle VolumeToggle;
    public TMP_Text VolumeText;

    // Update is called once per frame
    void Update() {
        //turn off volume toggle when slider changes
        if (VolumeSlider.value > 0 && VolumeToggle.isOn) {
            int val = (int)VolumeSlider.value;
            VolumeToggle.isOn = false;
            VolumeSlider.value = val;
        }
    }
    public void VolumeChange(float val) {
        if (VolumeText != null)
            VolumeText.text = $"{(int)val}";
    }
    public void toggleVolume(ref Settings _Settings, bool t) {
        _Settings.setMuteVolume(name, t);
        if (t) {
            _Settings.setVolumeVal(name, (int)VolumeSlider.value);
            VolumeSlider.value = 0;
        }
        else
            VolumeSlider.value = _Settings.getVolumeVal(name);
    }
    public void loadVolume(ref Settings _Settings) {
        if (VolumeToggle != null) {
            int val = _Settings.getVolumeVal(name);
            VolumeToggle.isOn = _Settings.getMuteVolume(name);
            _Settings.setVolumeVal(name, val);
        }

        if (VolumeToggle.isOn) {
            if (VolumeText != null)
                VolumeText.text = "0";
            if (VolumeSlider != null)
                VolumeSlider.value = 0;
        }
        else {
            if (VolumeText != null)
                VolumeText.text = $"{_Settings.getVolumeVal(name)}";
            if (VolumeSlider != null)
                VolumeSlider.value = _Settings.getVolumeVal(name);
        }
    }
}
