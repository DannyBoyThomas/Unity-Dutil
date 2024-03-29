using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Dutil
{

    public class GameSettings : MonoBehaviour
    {
        public static UnityEvent<float> OnMasterVolumeChanged = new UnityEvent<float>();
        public static UnityEvent<string, float> OnValueChanged = new UnityEvent<string, float>();
        public static UnityEvent<string, float> OnRelativeVolumeChanged = new UnityEvent<string, float>();
        static float _masterVolume = 1f;
        static bool _isInitialised = false;
        public static void Initialise()
        {
            _masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        }
        public static float MasterVolume
        {
            get { if (!_isInitialised) { Initialise(); } return _masterVolume; }
            set
            {
                _masterVolume = value.Clamp();
                PlayerPrefs.SetFloat("MasterVolume", value.Clamp()); OnMasterVolumeChanged.Invoke(value);
            }
        }
        /// <summary>
        /// Returns volume pre-adjusted by master volume
        /// </summary>
        public static float GetVolume(string varName, float defaultValue = 1)
        {
            return PlayerPrefs.GetFloat(varName, defaultValue) * MasterVolume;
        }
        public static void SetFloat(string varName, float value)
        {
            PlayerPrefs.SetFloat(varName, value.Clamp());
            OnValueChanged.Invoke(varName, value);
            OnRelativeVolumeChanged.Invoke(varName, value * MasterVolume);
        }
        public static float GetFloat(string varName, float defaultValue)
        {
            return PlayerPrefs.GetFloat(varName, defaultValue);
        }
        public static void SetInt(string varName, int value)
        {
            PlayerPrefs.SetInt(varName, value);
        }
        public static int GetInt(string varName, int defaultValue)
        {
            return PlayerPrefs.GetInt(varName, defaultValue);
        }

        public static List<string> GetResolutions()
        {
            List<string> resolutions = new List<string>();
            foreach (Resolution res in Screen.resolutions)
            {
                resolutions.Add(res.width + "x" + res.height);
            }
            return resolutions;
        }
        //set the resolution
        public static void SetResolution(string resolution)
        {
            string[] res = resolution.Split('x');
            int width = int.Parse(res[0]);
            int height = int.Parse(res[1]);
            Screen.SetResolution(width, height, Screen.fullScreen);
        }
        public static void SetResolution(int width, int height)
        {
            Screen.SetResolution(width, height, Screen.fullScreen);
        }
        public static List<int> GetRefreshRatesFromResolution(string resolution)
        {
            string[] res = resolution.Split('x');
            int width = int.Parse(res[0]);
            int height = int.Parse(res[1]);

            List<int> refreshRates = new List<int>();
            foreach (Resolution allRes in Screen.resolutions)
            {
                if (allRes.width == width && allRes.height == height)
                {
                    refreshRates.Add(allRes.refreshRate);
                }
            }
            return refreshRates;
        }
        public static void SetRefreshRate(int refreshRate)
        {
            Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen, refreshRate);
        }
        public static void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }
    }
}