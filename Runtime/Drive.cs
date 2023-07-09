using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Dutil.Data
{
    public static class Drive
    {
        static string Prefix = "d4vI3l";
        static string Suffix = "t40M4s";

        static string Directory(string fileName = null)
        {
            return Application.persistentDataPath + "/" + fileName + ".txt";
        }
        static string Wrap(string text)
        {
            return Prefix + text + Suffix;
        }
        static string Unwrap(string text)
        {
            return text.Substring(Prefix.Length, text.Length - Prefix.Length - Suffix.Length);
        }
        public static T Load<T>(string filename, T defaultData = default(T))
        {
            if (filename == null)
            {
                return defaultData;
            }
            string path = Directory(filename);
            if (File.Exists(path))
            {
                string text = Unwrap(File.ReadAllText(path));
                try
                {
                    byte[] decrypted = System.Convert.FromBase64String(text);
                    string json = System.Text.Encoding.UTF8.GetString(decrypted);
                    return JsonUtility.FromJson<T>(json);
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Failed to load: " + e.Message);
                    return defaultData;
                }
            }
            return defaultData;
        }

        public static bool Save(this object data, string fileName, bool printLocation = false)
        {
            if (fileName == null)
            {
                return false;
            }
            try
            {
                string json = JsonUtility.ToJson(data);
                byte[] encrypted = System.Text.Encoding.UTF8.GetBytes(json);
                string base64 = System.Convert.ToBase64String(encrypted);
                string encryptedString = Wrap(base64);
                string path = Directory(fileName);
                File.WriteAllText(path, encryptedString);
                if (printLocation)
                {
                    Debug.Log("Saved to: " + path);
                }
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Failed to save: " + e.Message);
                return false;
            }
        }
    }
}