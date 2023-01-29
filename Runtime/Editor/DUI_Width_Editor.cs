using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System;
using System.Reflection;

namespace Dutil
{
    [CustomEditor(typeof(DUI_Width))]
    public class DUI_Width_Editor : Editor
    {
        DUI_Width Target;
        bool showMinWidth = false;
        bool showMaxWidth = false;
        bool showPadding = false;
        private void OnEnable()
        {
            Target = (DUI_Width)base.target;
        }
        public override void OnInspectorGUI()
        {
            int spacing = 6;


            //get serialized property
            SerializedProperty forceToCenter = serializedObject.FindProperty("forceToCenter");
            SerializedProperty targetWidth = serializedObject.FindProperty("targetWidth");
            SerializedProperty useTargetPercentage = serializedObject.FindProperty("useTargetPercentage");
            SerializedProperty useMinWidth = serializedObject.FindProperty("useMinWidth");
            SerializedProperty useMaxWidth = serializedObject.FindProperty("useMaxWidth");
            SerializedProperty usePadding = serializedObject.FindProperty("usePadding");


            EditorGUILayout.PropertyField(forceToCenter);
            EditorGUILayout.PropertyField(targetWidth);
            //target percentage
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(useTargetPercentage);
            if (useTargetPercentage.boolValue)
            {
                GUILayout.Space(spacing);
                Target.targetWidthPercentage = EditorGUILayout.Slider(Target.targetWidthPercentage, 0, 100);
            }
            EditorGUILayout.EndHorizontal();

            //min width  

            showMinWidth = EditorGUILayout.BeginFoldoutHeaderGroup(showMinWidth, "Minimum Width");
            if (showMinWidth)
            {

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(spacing * 3);
                EditorGUILayout.PropertyField(useMinWidth);
                EditorGUILayout.EndHorizontal();

                if (useMinWidth.boolValue)
                {

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(spacing * 3);
                    SerializedProperty minWidth = serializedObject.FindProperty("minWidth");
                    EditorGUILayout.PropertyField(minWidth);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(spacing * 3);
                    SerializedProperty maxWidthUnit = serializedObject.FindProperty("maxWidthUnit");
                    EditorGUILayout.PropertyField(maxWidthUnit);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            //max width
            showMaxWidth = EditorGUILayout.BeginFoldoutHeaderGroup(showMaxWidth, "Maximum Width");
            if (showMaxWidth)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(spacing * 3);
                EditorGUILayout.PropertyField(useMaxWidth);
                EditorGUILayout.EndHorizontal();

                if (useMaxWidth.boolValue)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(spacing * 3);
                    SerializedProperty maxWidth = serializedObject.FindProperty("maxWidth");
                    EditorGUILayout.PropertyField(maxWidth);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(spacing * 3);
                    SerializedProperty maxWidthUnit = serializedObject.FindProperty("maxWidthUnit");
                    EditorGUILayout.PropertyField(maxWidthUnit);
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