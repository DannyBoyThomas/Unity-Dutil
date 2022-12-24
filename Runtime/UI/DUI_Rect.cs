using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using UnityEngine.UI;
using System.Linq;
namespace Dutil
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class DUI_Rect : MonoBehaviour
    {
        RectTransform rt;
        public bool forceToCenter = false;
        internal RectTransform RT
        {
            get
            {
                if (rt == null)
                {
                    rt = GetComponent<RectTransform>();
                    if (forceToCenter)
                    {
                        float w = rt.rect.width;
                        float h = rt.rect.height;
                        rt.anchorMin = rt.anchorMax = rt.pivot = Vector2.one * (this is DUI_Canvas ? 0 : .5f);
                        rt.sizeDelta = new Vector2(w, h);
                        rt.position = Vector3.zero;
                        rt.anchoredPosition = Vector2.zero;
                    }
                }
                return rt;
            }
            set { rt = value; }
        }

        public void Align(Vector2? anchor = null)
        {
            if (anchor == null)
            {
                anchor = Vector2.one * (this is DUI_Canvas ? 0 : .5f);
            }
            float w = RT.rect.width;
            float h = RT.rect.height;
            RT.anchorMin = RT.anchorMax = RT.pivot = anchor.Value;
            RT.sizeDelta = new Vector2(w, h);
            RT.position = Vector3.zero;
            RT.anchoredPosition = Vector2.zero;
        }
        // Start is called before the first frame update
        public virtual void Start()
        {
            PreStart();
            RT.name = name;
            PostStart();
            Calculate();
        }
        void OnValidate()
        {
            // if (!Application.isPlaying) { return; }
            // Calculate();
        }
        public virtual void PreStart()
        {

        }
        public virtual void PostStart()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!Application.isPlaying)
            {
                Calculate();
            }
        }
        public virtual void SetWidth(float width)
        {
            RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            InformChildrenOfChange();
        }
        public virtual float GetWidth()
        {
            return RT.rect.width;
        }
        public virtual void SetHeight(float height)
        {
            RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            InformChildrenOfChange();
        }
        public virtual float GetHeight()
        {
            return RT.rect.height;
        }
        public void OnParentChanged(RectTransform parent)
        {
            OnPreParentChanged(parent);
            InformChildrenOfChange();
        }
        public virtual void OnPreParentChanged(RectTransform parent)
        {
            Calculate();
        }
        public DUI_Rect Parent()
        {
            return transform.parent.GetComponent<DUI_Rect>();
        }
        public Vector2 Size()
        {
            return RT.rect.size;
        }
        public virtual void Calculate()
        {

        }
        public void InformChildrenOfChange()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponents<DUI_Rect>().ToList().ForEach(x => x.OnParentChanged(RT));
            }
        }
    }
    public enum DUI_UnitType { Pixels, Percentage }
}