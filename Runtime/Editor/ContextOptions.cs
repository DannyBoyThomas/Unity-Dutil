using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace Dutil
{
    [InitializeOnLoad]
    public static class ContextOptions
    {

        static ContextOptions()
        {
            EditorApplication.contextualPropertyMenu -= OnPropertyContextMenu;
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
                int amount = Colours.All.Count;
                for (int i = 0; i < amount; i++)
                {
                    Color c = Colours.All[i];
                    menu.AddItem(new GUIContent(Colours.AllNames[i]), false, () =>
                    {
                        property.colorValue = c;
                        property.serializedObject.ApplyModifiedProperties();
                        property.serializedObject.Update();
                    });
                }


            }

        }

    }
}
#endif