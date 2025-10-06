using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle fullscreenTog, vsyncTog;

    public List<ResItem> resolutions = new List<ResItem>();

    private int selectedResolution = 0;

    public Text resolutionsDisplay;

    public AudioMixer audioMixer;

    public Slider masterSlider, musicSlider, sfxSlider;

    public GameObject VirBoy;

    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }

        float vol = 0f;

        audioMixer.GetFloat("MasterVol", out vol);
        masterSlider.value = vol;

        audioMixer.GetFloat("MusicVol", out vol);
        masterSlider.value = vol;

        audioMixer.GetFloat("SFXVol", out vol);
        masterSlider.value = vol;

    }

    void Update()
    {
        Screen.fullScreen = fullscreenTog.isOn;

        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        if(Input.GetKeyDown(KeyCode.Tab) || Gamepad.current.selectButton.isPressed)
        {
            VirBoy.SetActive(true);
        }
    }

    public void ResLeft()
    {
        selectedResolution--;

        if (selectedResolution < 0)
        {
            selectedResolution = 0;
        }

        UpdateDisplayResolution();
    }

    public void UpdateDisplayResolution()
    {
        resolutionsDisplay.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ResRight()
    {
        selectedResolution++;

        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }

        UpdateDisplayResolution();
    }

    public void SetMasterVolume()
    {
        audioMixer.SetFloat("MasterVol", masterSlider.value);

        PlayerPrefs.SetFloat("MasterVol", masterSlider.value);
    }

    public void SetMusicVolume()
    {
        audioMixer.SetFloat("MusicVol", musicSlider.value);

        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
    }

    public void SetSFXVolume()
    {
        audioMixer.SetFloat("SFXVol", sfxSlider.value);

        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
