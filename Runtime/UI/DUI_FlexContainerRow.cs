using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using UnityEngine.UI;
namespace Dutil
{
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class DUI_FlexContainerRow : DUI_Rect
    {
        [Tooltip("Input a number. Each index represents a respective child's flex")]
        public string flex = "1";
        [Min(1)]
        public int defaultFlex = 1;


        public override void Calculate()
        {
            List<RectTransform> chidlren = GetChildren();
            HorizontalLayoutGroup hlg = GetComponent<HorizontalLayoutGroup>();
            float spaceLost = hlg.spacing * (chidlren.Count - 1);
            int totalFlex = GetTotalFlex();
            for (int i = 0; i < chidlren.Count; i++)
            {
                RectTransform child = chidlren[i];
                float childFlex = GetFlexAt(i);

                float childWidth = ((GetWidth() - spaceLost) / totalFlex) * childFlex;
                child.sizeDelta = new Vector2(childWidth, child.sizeDelta.y);


            }
        }

        int GetFlexAt(int index)
        {
            int flexInt = defaultFlex;
            if (index < 0 || index >= transform.childCount || index >= flex.Length)
            {
                return flexInt;
            }
            string c = flex[index].ToString().ToLower();
            if (c == "x") { return defaultFlex; }
            return int.Parse(c);
        }
        int GetTotalFlex()
        {
            int totalFlex = 0;
            List<RectTransform> chidlren = GetChildren();

            for (int i = 0; i < chidlren.Count; i++)
            {
                totalFlex += GetFlexAt(i);
            }
            return totalFlex;
        }
        List<RectTransform> GetChildren()
        {
            List<RectTransform> children = new List<RectTransform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
                DUI_Rect cantHave = (DUI_Rect)child.GetComponent<DUI_Width>();
                if (child != null && cantHave == null)
                {
                    children.Add(child);
                }
            }
            return children;
        }
    }

}