using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using UnityEditor;
using System.Linq;
[CustomEditor(typeof(LiveValue))]
public class LiveValueEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LiveValue liveValue = (LiveValue)target;
        LiveValue.LiveValueType beforeType = liveValue.valueType;
        liveValue.valueType = (LiveValue.LiveValueType)EditorGUILayout.EnumPopup("Value Type", liveValue.valueType);
        switch (liveValue.valueType)
        {
            case LiveValue.LiveValueType.Float:
                liveValue.floatValue = EditorGUILayout.FloatField("Float Value", liveValue.floatValue);
                break;
            case LiveValue.LiveValueType.Int:
                liveValue.intValue = EditorGUILayout.IntField("Int Value", liveValue.intValue);
                break;
            case LiveValue.LiveValueType.Bool:
                liveValue.boolValue = EditorGUILayout.Toggle("Bool Value", liveValue.boolValue);
                break;
            case LiveValue.LiveValueType.String:
                liveValue.stringValue = EditorGUILayout.TextField("String Value", liveValue.stringValue);
                break;
            case LiveValue.LiveValueType.Vector2:
                liveValue.vector2Value = EditorGUILayout.Vector2Field("Vector2 Value", liveValue.vector2Value);
                break;
            case LiveValue.LiveValueType.Vector3:
                liveValue.vector3Value = EditorGUILayout.Vector3Field("Vector3 Value", liveValue.vector3Value);
                break;
            case LiveValue.LiveValueType.Color:
                liveValue.colorValue = EditorGUILayout.ColorField("Color Value", liveValue.colorValue);
                break;
            default:
                break;
        }
        if (beforeType != liveValue.valueType)
        {
            if (liveValue.liveProperties.Count > 0)
            {
                //message box
                if (EditorUtility.DisplayDialog("Change Type", "Changing the type will remove all properties. Are you sure you want to continue?", "Yes", "No"))
                {
                    liveValue.liveProperties.Clear();
                }
                else
                {
                    liveValue.valueType = beforeType;
                }
            }
            else
            {
                liveValue.liveProperties.Clear();
            }
        }
        //event
        liveValue.showEvents = EditorGUILayout.Foldout(liveValue.showEvents, "Event");
        if (liveValue.showEvents)
        {
            SerializedProperty eventProp = serializedObject.FindProperty("onValueChanged"); // <-- UnityEvent

            //EditorGUIUtility.LookLikeControls();
            EditorGUILayout.PropertyField(eventProp);
        }

        //draw list
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Property"))
        {
            liveValue.liveProperties.Add(new LiveProperty());
        }

        EditorGUILayout.EndHorizontal();
        SerializedProperty serializedPropertyList = serializedObject.FindProperty("livePropertiesToShow");
        if (liveValue.liveProperties.Count > 0)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Properties ");
            liveValue.liveProperties.ToList().ForEach(x => CreateListElement(x));
            EditorGUILayout.EndVertical();
        }

        //changed
        if (GUI.changed)
        {
            EditorUtility.SetDirty(liveValue);
            liveValue.UpdateValues();
        }
    }
    // private void RenderList(SerializedProperty list)
    // {
    //     EditorGUI.indentLevel++; // Increase the indent level for better readability

    //     for (int i = 0; i < list.arraySize; i++)
    //     {
    //         SerializedProperty element = list.GetArrayElementAtIndex(i);

    //         // You can customize this line to handle different types of properties
    //         // EditorGUILayout.PropertyField(element, true);
    //         CreateListElement
    //     }

    //     EditorGUI.indentLevel--; // Decrease the indent level after rendering the list
    // }
    void CreateListElement(LiveProperty property)
    {
        if (property == null) { return; }
        List<string> options = property.GetOptions(((LiveValue)target).valueType);
        int currentIndex = options.IndexOf(property.propertyName);
        if (currentIndex == -1)
        {
            currentIndex = 0;
        }
        string title = $"{(property.component?.name ?? "Component")} - {property.propertyName}".CapitaliseAll();
        //fold out
        property.open = EditorGUILayout.Foldout(property.open, title);
        if (property.open)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical("Box");
            property.component = (MonoBehaviour)EditorGUILayout.ObjectField("Component", property.component, typeof(MonoBehaviour), true);
            property.propertyName = options[EditorGUILayout.Popup("Property Name", currentIndex, options.Select(x => x.CapitaliseAll()).ToArray())];
            EditorGUILayout.BeginHorizontal();
            property.update = EditorGUILayout.Toggle("Update", property.update);
            if (GUILayout.Button("Remove"))
            {
                ((LiveValue)target).liveProperties.Remove(property);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }
    }

}
