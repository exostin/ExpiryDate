<<<<<<< HEAD
using System;
=======
﻿using System;
>>>>>>> 40-sound-bar-sensivity
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Controllers
{
    public class SettingsMenuController : MonoBehaviour
    {
        [SerializeField] private AudioMixer masterMixer;

        [Header("Settings fields")]
        [SerializeField] private TMP_Dropdown fullScreenModeDropdown;
        [SerializeField] private Slider masterVolumeSlider, musicVolumeSlider, soundsVolumeSlider;

        private int usedFullScreenMode;
        private float usedMasterVolume, usedMusicVolume, usedSoundsVolume;

        private void Awake()
        {
            if (!PlayerPrefs.HasKey("FullScreenMode"))
            {
                PlayerPrefs.SetInt("FullScreenMode", 0);
                PlayerPrefs.SetFloat("MasterVolume", 0f);
                PlayerPrefs.SetFloat("MusicVolume", -10f);
                PlayerPrefs.SetFloat("SoundsVolume", 0f);
                PlayerPrefs.Save();
            }
            usedMasterVolume = PlayerPrefs.GetFloat("MasterVolume");
            usedMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
<<<<<<< HEAD
            usedSoundsVolume = PlayerPrefs.GetFloat("SoundsVolume");
=======
            usedSoundsVolume = PlayerPrefs.GetFloat("SoundVolume");
>>>>>>> 40-sound-bar-sensivity
            usedFullScreenMode = PlayerPrefs.GetInt("FullScreenMode");

            UpdateFullScreenMode();
            UpdateMasterVolume();
            UpdateMusicVolume();
            UpdateSoundsVolume();

            fullScreenModeDropdown.value = usedFullScreenMode;
            masterVolumeSlider.value = usedMasterVolume;
            musicVolumeSlider.value = usedMusicVolume;
            soundsVolumeSlider.value = usedSoundsVolume;
        }

        // ---- Methods that set the settings value
        public void SetFullScreenModeValue(int value)
        {
            usedFullScreenMode = value;
            UpdateFullScreenMode();
        }
        public void SetMasterVolumeValue(float value)
        {
            if (Math.Abs(value - masterVolumeSlider.minValue) < 1) value = -80;
            usedMasterVolume = value;
            UpdateMasterVolume();
        }
        public void SetMusicVolumeValue(float value)
        {
            if (Math.Abs(value - musicVolumeSlider.minValue) < 1) value = -80;
            usedMusicVolume = value;
            UpdateMusicVolume();
        }
        public void SetSoundsVolumeValue(float value)
        {
            if (Math.Abs(value - soundsVolumeSlider.minValue) < 1) value = -80;
            usedSoundsVolume = value;
            UpdateSoundsVolume();
        }

        // ---- Methods that update the current setting
        private void UpdateFullScreenMode()
        {
            switch (usedFullScreenMode)
            {
                case 0:
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.ExclusiveFullScreen);
                    break;

                case 1:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
                    break;
                case 2:
                    Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                    break;
            }
        }

        private void UpdateMasterVolume()
        {
            masterMixer.SetFloat("MasterVolume", usedMasterVolume);
        }
        private void UpdateMusicVolume()
        {
            masterMixer.SetFloat("MusicVolume", usedMusicVolume);
        }

        private void UpdateSoundsVolume()
        {
            masterMixer.SetFloat("SoundsVolume", usedSoundsVolume);
        }

        public void SavePreferences()
        {
            PlayerPrefs.SetInt("FullScreenMode", usedFullScreenMode);
            PlayerPrefs.SetFloat("MasterVolume", usedMasterVolume);
            PlayerPrefs.SetFloat("MusicVolume", usedMusicVolume);
            PlayerPrefs.SetFloat("SoundVolume", usedSoundsVolume);
            PlayerPrefs.Save();
        }
    }
}