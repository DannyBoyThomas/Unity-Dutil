using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using UnityEngine.Events;
#if SHAPES_INSTALLED
using Shapes;
namespace Dutil
{
    [ExecuteInEditMode]
    public class UiInputBool : MonoBehaviour
    {

        public bool value;
        public Color color = Color.white;
        Rectangle inner, outer;
        public UnityEvent<bool> OnValueChanged = new UnityEvent<bool>();
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        void OnValidate()
        {
            UpdateValue();
        }
        public bool Value
        {
            get { return value; }
            set
            {
                bool different = this.value != value;
                this.value = value; UpdateValue();
                if (different)
                {
                    OnValueChanged.Invoke(value);
                }
            }
        }
        public Rectangle Inner
        {
            get
            {
                if (inner == null)
                    inner = transform.FindChildWithName("Inner")?.gameObject.GetComponent<Rectangle>();
                return inner;
            }
        }
        public Rectangle Outer
        {
            get
            {
                if (outer == null)
                    outer = transform.FindChildWithName("Outer")?.gameObject.GetComponent<Rectangle>();
                return outer;
            }
        }
        public Color Color { get { return color; } set { color = value; UpdateValue(); } }
        void UpdateValue()
        {
            if (Inner != null)
            {
                Inner.Color = Color;
                Inner.enabled = Value;
            }
            if (Outer != null)
            {
                Outer.Color = Color;
            }
        }
        public void Toggle()
        {
            Value = !Value;
        }
    }
}
#endif