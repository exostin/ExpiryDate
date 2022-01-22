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
        [SerializeField] private Slider masterVolumeSlider, musicSlider, soundSlider;

        private int usedFullScreenMode;
        private float usedMasterVolume, usedMusicVolume, usedSoundVolume;

        private void Awake()
        {
            if (!PlayerPrefs.HasKey("FullScreenMode"))
            {
                PlayerPrefs.SetInt("FullScreenMode", 0);
                PlayerPrefs.SetFloat("MasterVolume", 0f);
                PlayerPrefs.SetFloat("MusicVolume", -10f);
                PlayerPrefs.SetFloat("SoundVolume", 0f);
                PlayerPrefs.Save();
            }
            usedMasterVolume = PlayerPrefs.GetFloat("MasterVolume");
            usedMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            usedSoundVolume = PlayerPrefs.GetFloat("SoundVolume");
            usedFullScreenMode = PlayerPrefs.GetInt("FullScreenMode");

            UpdateFullScreenMode();
            UpdateMasterVolume();
            UpdateMusicVolume();
            UpdateSoundsVolume();

            fullScreenModeDropdown.value = usedFullScreenMode;
            masterVolumeSlider.value = usedMasterVolume;
            musicSlider.value = usedMusicVolume;
            soundSlider.value = usedSoundVolume;
        }

        // ---- Methods that set the settings value
        public void SetFullScreenModeValue(int value)
        {
            usedFullScreenMode = value;
            UpdateFullScreenMode();
        }
        public void SetMasterVolumeValue(float value)
        {
            usedMasterVolume = value;
            UpdateMasterVolume();
        }
        public void SetMusicVolumeValue(float value)
        {
            usedMusicVolume = value;
            UpdateMusicVolume();
        }
        public void SetSoundsVolumeValue(float value)
        {
            usedSoundVolume = value;
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
            masterMixer.SetFloat("SoundsVolume", usedSoundVolume);
        }

        public void SavePreferences()
        {
            PlayerPrefs.SetInt("FullScreenMode", usedFullScreenMode);
            PlayerPrefs.SetFloat("MasterVolume", usedMasterVolume);
            PlayerPrefs.SetFloat("MusicVolume", usedMusicVolume);
            PlayerPrefs.SetFloat("SoundVolume", usedSoundVolume);
            PlayerPrefs.Save();
        }
    }
}