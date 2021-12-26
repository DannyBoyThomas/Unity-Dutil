using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using Dutil;
public class QuickReplaceEditor : EditorWindow
{
    public GameObject newObject;
    public List<GameObject> toReplace = new List<GameObject>();
    public List<GameObject> converted = new List<GameObject>();

    [MenuItem("Dutil/Quick Replace %&r")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        QuickReplaceEditor window = (QuickReplaceEditor)EditorWindow.GetWindow(typeof(QuickReplaceEditor));
        window.Show();
    }
    void OnGUI()
    {
        Color oldBG = GUI.backgroundColor;
        EditorGUILayout.BeginHorizontal();

        if (Selection.gameObjects.Length > 0)
        {
            if (GUILayout.Button("Pull Selected"))
            {
                toReplace.Clear();
                toReplace = Selection.gameObjects.ToList();
            }
        }
        if (GUILayout.Button("Reset"))
        {
            toReplace.Clear();
            converted.Clear();
            newObject = null;
        }

        bool performed = false;
        GUI.backgroundColor = Colours.Green;
        if (GUILayout.Button("Replace"))
        {
            if (newObject == null)
            {
                Debug.LogError("Need to set a new prefab to replace with");
            }
            else
            {
                toReplace.Clean();
                converted.Clear();

                for (int i = 0; i < toReplace.Count; i++)
                {
                    GameObject old = toReplace[i];
                    GameObject spawned = GameObject.Instantiate(newObject, old.transform.position, old.transform.rotation);
                    spawned.transform.localScale = old.transform.localScale;
                    spawned.name = old.name;
                    converted.Add(spawned);
                    Undo.RegisterCreatedObjectUndo(spawned, "Replaced using Dutil");
                }
                Undo.RecordObjects(toReplace.ToArray(), "Removed by Dutil");
                toReplace.ForEach(x => DestroyImmediate(x));
                toReplace.Clear();
                performed = true;
            }
        }
        GUI.backgroundColor = oldBG;
        EditorGUILayout.EndHorizontal();
        ScriptableObject scriptableObj = this;
        SerializedObject serialObj = new SerializedObject(scriptableObj);
        SerializedProperty list = serialObj.FindProperty("toReplace");
        SerializedProperty convertedList = serialObj.FindProperty("converted");
        SerializedProperty newItem = serialObj.FindProperty("newObject");

        EditorGUILayout.PropertyField(newItem, true);
        EditorGUILayout.PropertyField(list, !performed);
        EditorGUILayout.PropertyField(convertedList, true);
        serialObj.ApplyModifiedProperties();





    }

    public static void Show(SerializedProperty list)
    {
        EditorGUILayout.PropertyField(list);
        for (int i = 0; i < list.arraySize; i++)
        {
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
        }
    }
}
