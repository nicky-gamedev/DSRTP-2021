using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    Resolution[] resolutions;
    public Dropdown resDropdown;
    public Slider master, music, fx;
    private void Start()
    {
        resolutions = Screen.resolutions;
        
        resDropdown.ClearOptions();

        int currentRes = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + " Hz";
            options.Add(option);
            if (resolutions[i].height == Screen.currentResolution.height && resolutions[i].width == Screen.currentResolution.width)
            {
                currentRes = i;
            }
        }

        resDropdown.AddOptions(options);

        resDropdown.value = currentRes;
        resDropdown.RefreshShownValue();

        // Aplicar o que tiver guardado de som nos sliders
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            master.value = PlayerPrefs.GetFloat("MasterVolume");
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            music.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        if (PlayerPrefs.HasKey("FXVolume"))
        {
            fx.value = PlayerPrefs.GetFloat("FXVolume");
        }
    }

    #region Volume Sliders
    public AudioMixer audioMixer;
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetFXVolume(float volume)
    {
        audioMixer.SetFloat("FXVolume", volume);
        PlayerPrefs.SetFloat("FXVolume", volume);
    }
    #endregion

    #region Graphics
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resIndex)
    {
        Screen.SetResolution(resolutions[resIndex].width, resolutions[resIndex].height, Screen.fullScreen);
    }
    #endregion
}
