using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private AudioSource sfxPrefab;

    /*
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }*/


    #region Controls
    private void setVolume(string type, float volume)
    {
        audioMixer.SetFloat(type, Mathf.Log10(volume) * 20f);
    }

    private void setMute(string type, bool mute)
    {
        float volume = !mute ? -80f : 0f;
        audioMixer.SetFloat(type, volume);

    }
    public void setMasterVolume(float volume)
    {
        setVolume("Master Volume", volume);
    }

    public void setAmbientVolume(float volume)
    {
        setVolume("Ambient Volume", volume);
    }

    public void setMusicVolume(float volume)
    {
        setVolume("Music Volume", volume);
    }

    public void setSFXVolume(float volume)
    {
        setVolume("SFX Volume", volume);
    }

    public void setUIVolume(float volume)
    {
        setVolume("UI Volume", volume);
    }

    public void setMasterMute(bool mute)
    {
        setMute("Master Mute", mute);
    }

    public void setAmbientMute(bool mute)
    {
        setMute("Ambient Mute", mute);
    }

    public void setMusicMute(bool mute)
    {
        setMute("Music Mute", mute);
    }

    public void setSFXMute(bool mute)
    {
        setMute("SFX Mute", mute);
    }

    public void setUIMute(bool mute)
    {
        setMute("UI Mute", mute);
    }
    #endregion

    private void playAudioClip(AudioSource prefab, AudioClip audioClip, Transform location, float volume)
    {
        AudioSource audioSource = Instantiate<AudioSource>(prefab, location.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void playSFXAudioClip(AudioClip audioClip, Transform location, float volume)
    {
        playAudioClip(sfxPrefab, audioClip, location, volume);
    }
}
