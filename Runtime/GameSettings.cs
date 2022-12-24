using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dutil
{

    public class GameSettings : MonoBehaviour
    {
        static float _masterVolume = 1f;
        static bool _isInitialised = false;
        public static void Initialise()
        {
            _masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        }
        public static float MasterVolume
        {
            get { if (!_isInitialised) { Initialise(); } return _masterVolume; }
            set { _masterVolume = value.Clamp(); PlayerPrefs.SetFloat("MasterVolume", value.Clamp()); }
        }
        public static void SetFloat(string varName, float value)
        {
            PlayerPrefs.SetFloat(varName, value.Clamp());
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
        public static List<int> GetRefreshRatesFromResolution()
        {
            List<int> refreshRates = new List<int>();
            foreach (Resolution res in Screen.resolutions)
            {
                if (res.width == Screen.width && res.height == Screen.height)
                {
                    refreshRates.Add(res.refreshRate);
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