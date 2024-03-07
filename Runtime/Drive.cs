using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace Dutil
{
    public static class Drive
    {
        static int hashLength = 6;
        static string Prefix { get => D.Hash(hashLength); }
        static string Suffix { get => D.Hash(hashLength); }

        static string BasePath
        {
            get
            {
                string path = Application.persistentDataPath + "/drive/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        static string GetDirectory(string fileName = null, DriveType type = DriveType.Binary, bool addExtension = true)
        {
            bool hasNestedFolders = fileName.Contains("/");
            string extension = type switch
            {
                DriveType.Binary => ".bin",
                DriveType.JSON => ".json",
                DriveType.Base64 => ".txt",
                DriveType.Jumble => ".txt",
                _ => ".txt"
            };
            if (hasNestedFolders)
            {
                string[] folders = fileName.Split('/');
                string folder = BasePath;
                for (int i = 0; i < folders.Length - 1; i++)
                {
                    folder += folders[i] + "/";
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                }
                return folder + folders[folders.Length - 1] + (addExtension ? extension : "");
            }
            return BasePath + fileName + (addExtension ? extension : "");
        }
        static string Wrap(string text)
        {
            return Prefix + text + Suffix;
        }
        static string Unwrap(string text)
        {
            return text.Substring(hashLength, text.Length - (hashLength * 2));
        }
        public static T Load<T>(string filename, T defaultData = default(T))
        {
            List<string> files = Directory.GetFiles(BasePath, "*", SearchOption.AllDirectories).ToList();
            files = files.Select(x => x.Replace("\\", "/")).ToList();
            string path = files.FirstOrDefault(x => x.Contains(filename));
            string extension = Path.GetExtension(path);
            //if file exists
            if (File.Exists(path))
            {
                //if binary
                if (extension == ".bin")
                {
                    //open file
                    FileStream dataStream = new FileStream(path, FileMode.OpenOrCreate);
                    try
                    {
                        //convert to binary
                        BinaryFormatter converter = new BinaryFormatter();
                        //deserialize
                        T saveData = (T)converter.Deserialize(dataStream);
                        //close file
                        dataStream.Close();
                        //return data
                        return saveData;
                    }
                    catch (System.Exception)
                    {
                        //if failed, close file
                        //Debug.Log("Failed to load as Binary." + e.Message);
                        dataStream.Close();
                    }
                }
                //if json
                else if (extension == ".json")
                {
                    //read text
                    string text = File.ReadAllText(path);
                    try
                    {
                        //deserialize
                        return JsonUtility.FromJson<T>(text);
                    }
                    catch (System.Exception)
                    {
                        //Debug.Log("Failed to load as JSON: " + f.Message);
                    }
                }
                //if base64
                else if (extension == ".txt")
                {
                    //read text
                    string text = File.ReadAllText(path);
                    try
                    {
                        //convert to bytes
                        byte[] decrypted = System.Convert.FromBase64String(text);
                        //convert to json
                        string json = System.Text.Encoding.UTF8.GetString(decrypted);
                        //deserialize
                        return JsonUtility.FromJson<T>(json);
                    }
                    catch (System.Exception)
                    {
                        //Debug.Log("Failed to load as Base64: " + f.Message);
                        try
                        {
                            //unwrap
                            string unwrapped = Unwrap(text);
                            //convert to bytes
                            byte[] decrypted = System.Convert.FromBase64String(unwrapped);
                            //convert to json
                            string json = System.Text.Encoding.UTF8.GetString(decrypted);
                            //deserialize
                            return JsonUtility.FromJson<T>(json);
                        }
                        catch (System.Exception)
                        {
                            //Debug.Log("Failed to load as Jumble: " + f.Message);
                        }
                    }
                }
            }
            return defaultData;
        }

        /// <summary>
        /// Saves data in a variety of formats. Binary is the most efficient, but JSON is human readable. Json/Binary/Jumble only works on object with nested data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <param name="printLocation"></param>
        /// <returns></returns>
        public static bool Save(this object data, string fileName, DriveType type = DriveType.Binary, bool printLocation = false)
        {
            if (fileName == null)
            {
                return false;
            }
            try
            {
                string path = GetDirectory(fileName, type);
                if (type == DriveType.Binary)
                {
                    FileStream dataStream = new FileStream(path, FileMode.OpenOrCreate);
                    BinaryFormatter converter = new BinaryFormatter();
                    converter.Serialize(dataStream, data);
                    dataStream.Close();
                }
                else if (type == DriveType.JSON)
                {

                    string json = JsonUtility.ToJson(data);
                    File.WriteAllText(path, json);
                }
                else if (type == DriveType.Base64)
                {
                    string json = JsonUtility.ToJson(data);
                    byte[] encrypted = System.Text.Encoding.UTF8.GetBytes(json);
                    string base64 = System.Convert.ToBase64String(encrypted);
                    //string encryptedString = Wrap(base64);
                    File.WriteAllText(path, base64);
                }
                else if (type == DriveType.Jumble)
                {
                    string json = JsonUtility.ToJson(data);
                    byte[] encrypted = System.Text.Encoding.UTF8.GetBytes(json);
                    string base64 = System.Convert.ToBase64String(encrypted);
                    string encryptedString = Wrap(base64);

                    File.WriteAllText(path, encryptedString);
                }


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

        public enum DriveType { JSON, Binary, Base64, Jumble }
        public static void Remove(string filename)
        {
            //search without knowing the extension
            List<string> files = Directory.GetFiles(BasePath, "*", SearchOption.AllDirectories).ToList();
            files = files.Select(x => x.Replace("\\", "/")).ToList();
            string path = files.FirstOrDefault(x => x.Contains(filename));
            //if file exists
            if (File.Exists(path))
            {
                //delete it
                File.Delete(path);
            }

        }
    }

}