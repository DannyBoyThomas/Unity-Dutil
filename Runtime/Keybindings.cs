using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
namespace Dutil.Keybinding
{
    public class Keybindings
    {
        static List<Keybind> keybinds = new List<Keybind>();
        static Dictionary<KeyCode, KeyCode> keybindsDict = new Dictionary<KeyCode, KeyCode>(); //new -> original
        static Dictionary<string, Keybind> nameToKeybind = new Dictionary<string, Keybind>();
        static AutoLerpTask listenTask;
        static Dictionary<string, KeyCode> defaultInputs = new Dictionary<string, KeyCode>();
        /// <summary>
        /// Initialise keybindings. Also pass in all inputs that will be used in the game; then can use Keybindings.IfKeyDown("name") for all inputs
        /// </summary>
        /// <param name="inputs"></param>
        public static void Initialise(params (string name, KeyCode key)[] inputs)
        {
            keybinds = new List<Keybind>();
            defaultInputs = new Dictionary<string, KeyCode>();
            foreach ((string name, KeyCode key) in inputs)
            {
                defaultInputs.Add(name, key);
            }
            Load();
            foreach ((string name, KeyCode key) in inputs)
            {
                if (!keybinds.Any(x => x.name == name))
                {
                    CreateKeybind(name, key, key);
                }
            }
        }
        /// <summary>
        /// Initialise keybindings with default inputs (up, down, left, right, space, left_shift, left_control, mouse0, mouse1)
        /// </summary>
        public static void InitialiseWithBasics(params (string name, KeyCode key)[] inputs)
        {
            keybinds = new List<Keybind>();
            defaultInputs = new Dictionary<string, KeyCode>() { { "up", KeyCode.W }, { "down", KeyCode.S }, { "left", KeyCode.A }, { "right", KeyCode.D }, { "space", KeyCode.Space }, { "left_shift", KeyCode.LeftShift }, { "left_control", KeyCode.LeftControl }, { "mouse0", KeyCode.Mouse0 }, { "mouse1", KeyCode.Mouse1 } };
            foreach ((string name, KeyCode key) in inputs)
            {
                defaultInputs.Add(name, key);
            }
            Load();
            foreach (var entry in defaultInputs)
            {
                if (!keybinds.Any(x => x.name == entry.Key))
                {
                    CreateKeybind(entry.Key, entry.Value, entry.Value);
                }
            }
        }
        static void Load()
        {
            keybinds = Drive.Load<List<Keybind>>("keybinds", new List<Keybind>());
            CreateDictionariesFromList();
        }
        static void CreateDictionariesFromList()
        {
            keybindsDict = new Dictionary<KeyCode, KeyCode>();
            foreach (Keybind k in keybinds)
            {
                keybindsDict.Add(k.New, k.Original);
            }
            nameToKeybind = new Dictionary<string, Keybind>();
            foreach (Keybind k in keybinds)
            {
                nameToKeybind.Add(k.name, k);
            }


        }
        static void Save()
        {
            Drive.Save(keybinds, "keybinds");
        }
        public static KeybindResult CreateKeybind(string name, KeyCode original, KeyCode newKey, bool force = false)
        {
            bool keyAlreadyMapped = keybindsDict.ContainsValue(newKey);
            if (keyAlreadyMapped)
            {
                if (force)
                {
                    keybinds.RemoveAll(x => x.Original == original);
                }
                else
                {
                    return KeybindResult.KeyAlreadyBound;
                }
            }

            keybinds.Add(new Keybind(name, original, newKey));
            CreateDictionariesFromList();
            Save();
            return KeybindResult.Success;
        }
        public static bool IfKeyDown(string name)
        {
            //use dict  to get original key
            if (nameToKeybind.ContainsKey(name))
            {
                return Input.GetKeyDown(nameToKeybind[name].New);
            }
            else
            {
                return false;
            }

        }
        public static bool IfKey(string name)
        {
            //use dict  to get original key
            if (nameToKeybind.ContainsKey(name))
            {
                return Input.GetKey(nameToKeybind[name].New);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// If keycode is a keybind, return the original key, else return the keycode
        /// </summary>
        /// <param name="topKey"></param>
        /// <returns></returns>
        public static KeyCode GetBaseKey(KeyCode topKey)
        {
            if (keybindsDict.ContainsKey(topKey))
            {
                return keybindsDict[topKey];
            }
            else
            {
                return topKey;
            }
        }
        public static void ListenAndCreate(string name, KeyCode original, Action<KeybindResultData> callback, float waitDuration = 4, bool force = false)
        {
            Debug.Log($"Listening for new keybind for {name} [{original}] for {waitDuration} seconds...");
            Schedule.Begin(.01f, (st) =>
            {
                listenTask = AutoLerp.Begin(waitDuration, (t, v) =>
                    {
                        if (Input.anyKeyDown)
                        {
                            foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
                            {
                                if (Input.GetKeyDown(k))
                                {
                                    KeybindResult res = CreateKeybind(name, original, k, force);
                                    KeyCode key = res == KeybindResult.Success ? k : KeyCode.None;
                                    callback(new KeybindResultData(res, key));
                                    t.Cancel();
                                    break;
                                }
                            }
                        }

                    }).OnComplete((c) => { callback(new KeybindResultData(KeybindResult.TimedOut, KeyCode.None)); });
            });

        }
        public static void StopListening()
        {
            listenTask?.Cancel();
        }
        public static void Print()
        {
            foreach (Keybind k in keybinds)
            {
                Debug.Log(k.name + " " + k.originalKey + " -> " + k.newKey);
            }
        }
        public static bool Remove(string name)
        {
            if (keybinds.Any(x => x.name == name))
            {
                keybinds.Remove(keybinds.First(x => x.name == name));
                CreateDictionariesFromList();
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool RemoveNewKey(KeyCode newKey)
        {
            Debug.Log("Removing " + newKey);
            Print();
            if (keybinds.Any(x => x.newKey == newKey.ToString()))
            {
                keybinds.Remove(keybinds.First(x => x.New == newKey));
                Print();
                CreateDictionariesFromList();
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool RemoveOriginalKey(KeyCode originalKey)
        {
            Debug.Log("Removing old " + originalKey);
            if (keybinds.Any(x => x.Original == originalKey))
            {
                keybinds.RemoveAll(x => x.Original == originalKey);
                CreateDictionariesFromList();
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool RemoveAll()
        {
            keybinds.Clear();
            keybindsDict.Clear();
            nameToKeybind.Clear();
            Save();
            return true;
        }


    }
    [Serializable]
    public class Keybind
    {
        public string name;
        public string originalKey, newKey;
        public Keybind(string name, KeyCode og, KeyCode newKey)
        {
            this.name = name;
            this.originalKey = og.ToString();
            this.newKey = newKey.ToString();
        }
        public Keybind(string name, string originalKey, string newKey)
        {
            this.name = name;
            this.originalKey = originalKey;
            this.newKey = newKey;
        }
        public KeyCode Original => (KeyCode)Enum.Parse(typeof(KeyCode), originalKey);
        public KeyCode New => (KeyCode)Enum.Parse(typeof(KeyCode), newKey);
    }
    public enum KeybindResult
    {
        Success,
        KeyAlreadyBound,
        KeyDoesNotExist,
        KeybindDoesNotExist,
        TimedOut
    }
    public struct KeybindResultData
    {
        public KeybindResult result;
        public KeyCode code;
        public KeybindResultData(KeybindResult result, KeyCode code)
        {
            this.result = result;
            this.code = code;
        }
        public override string ToString()
        {
            return $"{result} {code}";
        }
    }
}