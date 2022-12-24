using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using UnityEngine.UI;
using UnityEngine.Events;
namespace Dutil
{
    public class DUI_Select : MonoBehaviour
    {
        public bool loop = true;
        public List<string> options = new List<string>();
        Text text;
        int selectedIndex = 0;
        public UnityEvent<int> OnSelectionChanged = new UnityEvent<int>();
        void Start()
        {
            transform.Find("Left Button").GetComponent<Button>().onClick.AddListener(() => { SelectPrevious(); });
            transform.Find("Right Button").GetComponent<Button>().onClick.AddListener(() => { SelectNext(); });
            text = transform.Find("Selection").GetComponentInChildren<Text>();
            UpdateText();
        }

        public void SetOptions(List<string> newOptions, int defaultIndex = 0, bool triggerEvent = false)
        {
            options = newOptions;
            selectedIndex = Mathf.Clamp(defaultIndex, 0, options.Count - 1);
            if (triggerEvent)
            {
                OnSelectionChanged.Invoke(selectedIndex);
            }
            UpdateText();
        }
        public void SetIndex(int index, bool triggerEvent = false)
        {
            selectedIndex = Mathf.Clamp(selectedIndex, 0, options.Count - 1);
            if (triggerEvent)
            {
                OnSelectionChanged.Invoke(selectedIndex);
            }
            UpdateText();
        }
        void UpdateText()
        {
            if (selectedIndex < 0 || selectedIndex >= options.Count)
            {
                Debug.Log("Selected index out of range");
                return;
            }
            text.text = options[selectedIndex];
        }
        void SelectPrevious()
        {
            int before = selectedIndex;
            selectedIndex = loop ? (selectedIndex - 1 + options.Count) % options.Count : Mathf.Max(0, selectedIndex - 1);
            UpdateText();
            if (before != selectedIndex)
            {
                OnSelectionChanged.Invoke(selectedIndex);
            }
        }
        void SelectNext()
        {
            int before = selectedIndex;
            selectedIndex = loop ? (selectedIndex + 1) % options.Count : Mathf.Min(options.Count - 1, selectedIndex + 1);
            UpdateText();
            if (before != selectedIndex)
            {
                OnSelectionChanged.Invoke(selectedIndex);
            }
        }
        void OnDestroy()
        {
            transform.Find("Left Button").GetComponent<Button>().onClick.RemoveAllListeners();
            transform.Find("Right Button").GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}