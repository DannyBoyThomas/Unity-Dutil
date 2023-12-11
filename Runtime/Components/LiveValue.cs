using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;

using System.Reflection;
using System;
using UnityEngine.Events;
namespace Dutil
{
    [System.Serializable]
    [ExecuteInEditMode]

    public class LiveValue : MonoBehaviour
    {
        public enum LiveValueType { Float, Int, Bool, String, Vector2, Vector3, Color }
        public LiveValueType valueType;
        public float floatValue;
        public int intValue;
        public bool boolValue;
        public string stringValue;
        public Vector2 vector2Value;
        public Vector3 vector3Value;
        public Color colorValue;
        public UnityEvent onValueChanged = new UnityEvent();
        internal bool showEvents = false;
        [SerializeField]
        public List<LiveProperty> liveProperties = new List<LiveProperty>();

        byte[] rawValue;

        void OnValidate()
        {
            UpdateValues();
        }

        public void SetFloat(float value)
        {
            if (valueType != LiveValueType.Float) { return; }
            floatValue = value;
            UpdateValues();
        }
        public void SetInt(int value)
        {
            if (valueType != LiveValueType.Int) { return; }
            intValue = value;
            UpdateValues();
        }
        public void SetBool(bool value)
        {
            if (valueType != LiveValueType.Bool) { return; }
            boolValue = value;
            UpdateValues();
        }
        public void SetString(string value)
        {
            if (valueType != LiveValueType.String) { return; }
            stringValue = value;
            UpdateValues();
        }
        public void SetVector2(Vector2 value)
        {
            if (valueType != LiveValueType.Vector2) { return; }
            vector2Value = value;
            UpdateValues();
        }
        public void SetVector3(Vector3 value)
        {
            if (valueType != LiveValueType.Vector3) { return; }
            vector3Value = value;
            UpdateValues();
        }
        public void SetColor(Color value)
        {
            if (valueType != LiveValueType.Color) { return; }
            colorValue = value;
            UpdateValues();
        }

        public static bool MatchesType(System.Type type, LiveValue.LiveValueType valueType)
        {
            switch (valueType)
            {
                case LiveValue.LiveValueType.Float:
                    return type == typeof(float);
                case LiveValue.LiveValueType.Int:
                    return type == typeof(int);
                case LiveValue.LiveValueType.Bool:
                    return type == typeof(bool);
                case LiveValue.LiveValueType.String:
                    return type == typeof(string);
                case LiveValue.LiveValueType.Vector2:
                    return type == typeof(Vector2);
                case LiveValue.LiveValueType.Vector3:
                    return type == typeof(Vector3);
                case LiveValue.LiveValueType.Color:
                    return type == typeof(Color);
                default:
                    return false;
            }
        }
        // internal void UpdateValues()
        // {
        //     for (int i = 0; i < liveProperties.Count; i++)
        //     {
        //         Debug.Log(i);
        //         if (liveProperties[i].component == null) { continue; }


        //         if (!liveProperties[i].update) { continue; }
        //         SerializedObject so = new SerializedObject(liveProperties[i].component);
        //         var targetProperty = so.FindProperty(liveProperties[i].propertyName);
        //         Debug.Log("Attempting to set " + liveProperties[i].propertyName + " on " + liveProperties[i].component.name);
        //         if (targetProperty == null) { Debug.Log("Failed"); continue; }
        //         if (targetProperty.propertyType == SerializedPropertyType.Float)
        //         {
        //             targetProperty.floatValue = floatValue;
        //         }
        //         else if (targetProperty.propertyType == SerializedPropertyType.Integer)
        //         {
        //             targetProperty.intValue = intValue;
        //         }
        //         else if (targetProperty.propertyType == SerializedPropertyType.Boolean)
        //         {
        //             targetProperty.boolValue = boolValue;
        //         }
        //         else if (targetProperty.propertyType == SerializedPropertyType.String)
        //         {
        //             targetProperty.stringValue = stringValue;
        //         }
        //         else if (targetProperty.propertyType == SerializedPropertyType.Vector2)
        //         {
        //             targetProperty.vector2Value = vector2Value;
        //         }
        //         else if (targetProperty.propertyType == SerializedPropertyType.Vector3)
        //         {
        //             targetProperty.vector3Value = vector3Value;
        //         }
        //         else if (targetProperty.propertyType == SerializedPropertyType.Color)
        //         {
        //             targetProperty.colorValue = colorValue;
        //         }
        //         else
        //         {
        //             Debug.LogError("Property type not supported");
        //         }
        //         so.ApplyModifiedProperties();

        //     }
        // }
        internal void UpdateValues()
        {
            for (int i = 0; i < liveProperties.Count; i++)
            {
                if (liveProperties[i].component == null) { continue; }
                if (!liveProperties[i].update) { continue; }
                FieldInfo field = liveProperties[i].component.GetType().GetField(liveProperties[i].propertyName);
                if (field == null) { continue; }
#if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(liveProperties[i].component, "Live Value Update");

#endif
                bool valueChanged = false;
                if (field.FieldType == typeof(float))
                {
                    if ((float)field.GetValue(liveProperties[i].component) != floatValue)
                    {
                        valueChanged = true;
                    }
                    field.SetValue(liveProperties[i].component, floatValue);
                }
                else if (field.FieldType == typeof(int))
                {
                    if ((int)field.GetValue(liveProperties[i].component) != intValue)
                    {
                        valueChanged = true;
                    }
                    field.SetValue(liveProperties[i].component, intValue);
                }
                else if (field.FieldType == typeof(bool))
                {
                    if ((bool)field.GetValue(liveProperties[i].component) != boolValue)
                    {
                        valueChanged = true;
                    }
                    field.SetValue(liveProperties[i].component, boolValue);
                }
                else if (field.FieldType == typeof(string))
                {
                    if ((string)field.GetValue(liveProperties[i].component) != stringValue)
                    {
                        valueChanged = true;
                    }
                    field.SetValue(liveProperties[i].component, stringValue);
                }
                else if (field.FieldType == typeof(Vector2))
                {
                    if ((Vector2)field.GetValue(liveProperties[i].component) != vector2Value)
                    {
                        valueChanged = true;
                    }
                    field.SetValue(liveProperties[i].component, vector2Value);
                }
                else if (field.FieldType == typeof(Vector3))
                {
                    if ((Vector3)field.GetValue(liveProperties[i].component) != vector3Value)
                    {
                        valueChanged = true;
                    }
                    field.SetValue(liveProperties[i].component, vector3Value);
                }
                else if (field.FieldType == typeof(Color))
                {
                    if ((Color)field.GetValue(liveProperties[i].component) != colorValue)
                    {
                        valueChanged = true;
                    }
                    field.SetValue(liveProperties[i].component, colorValue);
                }
                else
                {
                    Debug.LogError("Property type not supported");
                }
                if (valueChanged)
                {
                    onValueChanged.Invoke();
                }
                //validate
                try
                {

                    MethodInfo onValidateMethod = liveProperties[i].component.GetType().GetMethod("OnValidate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                    if (onValidateMethod != null)
                    {
                        onValidateMethod.Invoke(liveProperties[i].component, null);
                    }
                    else
                    {
                        Debug.LogWarning("OnValidate method not found for " + liveProperties[i].component.GetType());
                    }

                }
                catch (Exception)
                {
                    //Debug.LogError("Error validating " + liveProperties[i].component.name + " " + e.Message);
                }
            }
        }


    }
    [Serializable]
    public class LiveProperty
    {
        public MonoBehaviour component;
        public string propertyName = "None";
        public bool update = true;
        public bool open = true;
        public List<string> GetOptions(LiveValue.LiveValueType type)
        {
            List<string> options = new List<string>();
            options.Add("None");
            if (component != null)
            {
                FieldInfo[] fields = component.GetType().GetFields();
                for (int i = 0; i < fields.Length; i++)
                {
                    bool sameType = LiveValue.MatchesType(fields[i].FieldType, type);
                    bool isPublicOrSerialized = fields[i].IsPublic || fields[i].IsDefined(typeof(SerializeField), true);
                    if (sameType && isPublicOrSerialized)
                        options.Add(fields[i].Name);
                }
            }
            return options;
        }
    }
}