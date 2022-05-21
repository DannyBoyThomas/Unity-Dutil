using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Dutil
{
    public class ContextOptions : EditorWindow
    {

        [InitializeOnLoadMethod]
        static void Start()
        {

            EditorApplication.contextualPropertyMenu += OnPropertyContextMenu;
        }

        static void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.Color)
            {
                menu.AddItem(new GUIContent("Pastel"), false, () =>
                {
                    property.colorValue = property.colorValue.Pastel();
                    property.serializedObject.ApplyModifiedProperties();
                    property.serializedObject.Update();
                });
                menu.AddItem(new GUIContent("Calm"), false, () =>
               {
                   property.colorValue = property.colorValue.Calm();
                   property.serializedObject.ApplyModifiedProperties();
                   property.serializedObject.Update();
               });

            }

        }

    }
}
