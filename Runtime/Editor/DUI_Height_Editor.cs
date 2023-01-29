using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
#if UNITY_EDITOR
using UnityEditor;
using System;
using System.Reflection;

namespace Dutil
{

    [CustomEditor(typeof(DUI_Height))]
    public class DUI_Height_Editor : Editor
    {
        DUI_Height Target;
        bool showMinHeight = false;
        bool showMaxHeight = false;
        bool showPadding = false;
        private void OnEnable()
        {
            Target = (DUI_Height)base.target;
        }
        public override void OnInspectorGUI()
        {
            int spacing = 6;


            //get serialized property
            SerializedProperty forceToCenter = serializedObject.FindProperty("forceToCenter");
            SerializedProperty targetHeight = serializedObject.FindProperty("targetHeight");
            SerializedProperty useTargetPercentage = serializedObject.FindProperty("useTargetPercentage");
            SerializedProperty useMinHeight = serializedObject.FindProperty("useMinHeight");
            SerializedProperty useMaxHeight = serializedObject.FindProperty("useMaxHeight");
            SerializedProperty usePadding = serializedObject.FindProperty("usePadding");


            EditorGUILayout.PropertyField(forceToCenter);
            EditorGUILayout.PropertyField(targetHeight);
            //target percentage
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(useTargetPercentage);
            if (useTargetPercentage.boolValue)
            {
                GUILayout.Space(spacing);
                Target.targetHeightPercentage = EditorGUILayout.Slider(Target.targetHeightPercentage, 0, 100);
            }
            EditorGUILayout.EndHorizontal();

            //min height  

            showMinHeight = EditorGUILayout.BeginFoldoutHeaderGroup(showMinHeight, "Minimum Height");
            if (showMinHeight)
            {

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(spacing * 3);
                EditorGUILayout.PropertyField(useMinHeight);
                EditorGUILayout.EndHorizontal();

                if (useMinHeight.boolValue)
                {

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(spacing * 3);
                    SerializedProperty minHeight = serializedObject.FindProperty("minHeight");
                    EditorGUILayout.PropertyField(minHeight);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(spacing * 3);
                    SerializedProperty maxHeightUnit = serializedObject.FindProperty("maxHeightUnit");
                    EditorGUILayout.PropertyField(maxHeightUnit);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            //max height
            showMaxHeight = EditorGUILayout.BeginFoldoutHeaderGroup(showMaxHeight, "Maximum Height");
            if (showMaxHeight)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(spacing * 3);
                EditorGUILayout.PropertyField(useMaxHeight);
                EditorGUILayout.EndHorizontal();

                if (useMaxHeight.boolValue)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(spacing * 3);
                    SerializedProperty maxHeight = serializedObject.FindProperty("maxHeight");
                    EditorGUILayout.PropertyField(maxHeight);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(spacing * 3);
                    SerializedProperty maxHeightUnit = serializedObject.FindProperty("maxHeightUnit");
                    EditorGUILayout.PropertyField(maxHeightUnit);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            //padding
            showPadding = EditorGUILayout.BeginFoldoutHeaderGroup(showPadding, "Padding");
            if (showPadding)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(spacing * 3);
                EditorGUILayout.PropertyField(usePadding);
                EditorGUILayout.EndHorizontal();

                if (usePadding.boolValue)
                {

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(spacing * 3);
                    SerializedProperty padding = serializedObject.FindProperty("padding");
                    EditorGUILayout.PropertyField(padding);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(spacing * 3);
                    SerializedProperty paddingUnit = serializedObject.FindProperty("paddingUnit");
                    EditorGUILayout.PropertyField(paddingUnit);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            serializedObject.ApplyModifiedProperties();


        }

    }
}
#endif