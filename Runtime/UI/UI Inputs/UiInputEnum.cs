using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
namespace Dutil
{
    [ExecuteInEditMode]
    public class UiInputEnum : MonoBehaviour
    {
        public enum SelectionType { String, Enum }
        public int startingIndex;
        public SelectionType selectionType;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf("selectionType", SelectionType.String)]
#endif
        public List<string> stringOptions = new List<string>();
        [Tooltip("namespace.class_name+enum_name")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf("selectionType", SelectionType.Enum)]

#endif
        public string enumName = "Dutil.UiInputEnum+SelectionType";
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf("selectionType", SelectionType.Enum)]

#endif
        public List<string> enumOptions = new List<string>();
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
        public string labelValue;
#endif
        Slider slider;
        TextMeshProUGUI label;
        int index;
        public UnityEvent<int> OnValueChangedEvent = new UnityEvent<int>();
        void Start()
        {
            int actIndex = startingIndex.Clamp(0, Options.Count - 1);
            Index = index;
        }

        // Update is called once per frame
        void Update()
        {

        }
        void OnValidate()
        {
            Refresh();
        }
        void Refresh()
        {
            if (selectionType == SelectionType.Enum)
            {
                enumOptions.Clear();
                System.Type type = System.Type.GetType(enumName);
                if (type != null)
                {
                    System.Enum.GetNames(type);
                    enumOptions = System.Enum.GetNames(System.Type.GetType(enumName)).ToList();
                }
            }
            Index = startingIndex.Clamp(0, Options.Count - 1);
            Slider.minValue = 0;
            Slider.maxValue = Options.Count - 1;
            Slider.value = index;
            Slider.wholeNumbers = true;
            Label.text = Value;
        }
        public List<string> Options
        {
            get
            {
                if (selectionType == SelectionType.String)
                    return stringOptions;
                else
                    return enumOptions;
            }
        }
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                this.labelValue = Options[value];
                Slider.value = value;
                index = value;
                Label.text = Value;
            }
        }
        public string Value
        {
            get { return labelValue; }

        }
        Slider Slider
        {
            get
            {
                if (slider == null)
                    slider = GetComponentInChildren<Slider>();
                return slider;
            }
        }
        public void SetOptions(List<string> options)
        {
            stringOptions = options;
            selectionType = SelectionType.String;
            Refresh();
        }
        public void SetOptions(System.Type type)
        {
            enumName = type.FullName;
            selectionType = SelectionType.Enum;
            Refresh();
        }

        public void OnValueChanged(float value)
        {

            Index = value.Round();
            OnValueChangedEvent.Invoke(Index);
        }

        TextMeshProUGUI Label
        {
            get
            {
                if (label == null)
                    label = transform.FindChildWithName("Value Text").GetComponent<TextMeshProUGUI>();
                return label;
            }
        }

    }
}