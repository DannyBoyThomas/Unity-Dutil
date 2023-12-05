using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

using System;
using System.Linq;
namespace Dutil
{
    public class QuickReplaceEditor : EditorWindow
    {
        public List<GameObject> newPrefabs = new List<GameObject>();
        public List<GameObject> toReplace = new List<GameObject>();
        public List<GameObject> converted = new List<GameObject>();

        [MenuItem("Dutil/Quick Replace %&r")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            QuickReplaceEditor window = (QuickReplaceEditor)EditorWindow.GetWindow(typeof(QuickReplaceEditor), false, "Quick Replace");
            window.Show();
        }
        void OnGUI()
        {
            Color oldBG = GUI.backgroundColor;
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Colours.Orange.Shade(2);
            if (Selection.gameObjects.Length > 0)
            {
                if (GUILayout.Button("Pull Selected", new GUILayoutOption[] { GUILayout.Height(24) }))
                {
                    toReplace.Clear();
                    toReplace = Selection.gameObjects.ToList();
                }
            }


            if (GUILayout.Button("Clear", new GUILayoutOption[] { GUILayout.Height(24) }))
            {
                toReplace.Clear();
            }
            GUI.backgroundColor = Colours.Red.Shade(2);
            if (GUILayout.Button("Reset", new GUILayoutOption[] { GUILayout.Height(24) }))
            {
                toReplace.Clear();
                converted.Clear();
                newPrefabs.Clear();
            }
            EditorGUILayout.EndHorizontal();
            bool performed = false;
            GUI.backgroundColor = Colours.Blue.Shade(2);
            if (GUILayout.Button("Replace", new GUILayoutOption[] { GUILayout.Height(32) }))
            {
                if (newPrefabs.Count <= 0)
                {
                    Debug.LogError("Need to set a new prefab to replace with");
                }
                else
                {
                    toReplace.Clean();
                    converted.Clear();
                    Undo.SetCurrentGroupName("replaceables");
                    for (int i = 0; i < toReplace.Count; i++)
                    {
                        GameObject old = toReplace[i];
                        GameObject toPlace = newPrefabs.Any();
                        if (toPlace != null)
                        {
                            bool isprefab = PrefabUtility.GetPrefabAssetType(toPlace) != PrefabAssetType.NotAPrefab;

                            GameObject spawned = null;
                            if (isprefab)
                            {
                                spawned = PrefabUtility.InstantiatePrefab(toPlace) as GameObject;
                            }
                            else
                            {
                                spawned = GameObject.Instantiate(toPlace);
                            }
                            spawned.transform.position = old.transform.position;
                            spawned.transform.rotation = old.transform.rotation;
                            spawned.transform.localScale = old.transform.localScale;
                            spawned.name = old.name;
                            converted.Add(spawned);
                            Undo.RegisterCreatedObjectUndo(spawned, "Replaced using Dutil");
                        }
                    }

                    toReplace.ForEach(x => Undo.DestroyObjectImmediate(x));
                    toReplace.Clear();
                    performed = true;
                }
            }
            GUILayout.Label("");
            ScriptableObject scriptableObj = this;
            SerializedObject serialObj = new SerializedObject(scriptableObj);

            SerializedProperty list = serialObj.FindProperty("toReplace");

            SerializedProperty convertedList = serialObj.FindProperty("converted");

            SerializedProperty prefabList = serialObj.FindProperty("newPrefabs");

            //SerializedProperty newItem = serialObj.FindProperty("newPrefab");

            GUI.backgroundColor = Colours.Blue.Shade(1);
            EditorGUILayout.PropertyField(prefabList, true);

            GUI.backgroundColor = Colours.Orange.Shade(2);
            EditorGUILayout.PropertyField(list, !performed);

            GUI.backgroundColor = oldBG;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("^^^", new GUILayoutOption[] { GUILayout.Height(24), GUILayout.Width(60) }))
            {
                toReplace = converted.Copy();
                converted.Clear();
            }
            GUILayout.EndHorizontal();
            //GUI.backgroundColor = Colours.Blue.Shade(2);
            EditorGUILayout.PropertyField(convertedList, true);
            serialObj.ApplyModifiedProperties();
            GUI.backgroundColor = oldBG;





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
}
#endif