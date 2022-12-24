using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
namespace Dutil
{
    public class DUI_Width : DUI_Rect, IDUI_SizeManipulator
    {
        public float targetWidth = 100;
        public bool useTargetPercentage = false;
        [Range(0, 100)]
        public float targetWidthPercentage = 100;

        public bool useMinWidth = false;
        public float minWidth = 0;
        public DUI_UnitType minWidthUnit = DUI_UnitType.Pixels;

        public bool useMaxWidth = false;
        public float maxWidth = 10000;
        public DUI_UnitType maxWidthUnit = DUI_UnitType.Pixels;

        public bool usePadding = false;
        public float padding = 0;
        public DUI_UnitType paddingUnit = DUI_UnitType.Pixels;




        // Start is called before the first frame update


        public override void Calculate()
        {
            float prevWidth = GetWidth();
            float parentWidth = Parent()?.GetWidth() ?? 0;
            float newWidth = useTargetPercentage ? (parentWidth * targetWidthPercentage / 100) - padding : targetWidth;

            if (useMinWidth)
            {
                float minTargetWidth = minWidthUnit == DUI_UnitType.Pixels ? minWidth : (parentWidth * minWidth / 100) - padding;
                newWidth = Mathf.Max(newWidth, minTargetWidth);
            }
            if (useMaxWidth)
            {
                float maxTargetWidth = maxWidthUnit == DUI_UnitType.Pixels ? maxWidth : (parentWidth * maxWidth / 100) - padding;
                newWidth = Mathf.Min(newWidth, maxTargetWidth);
            }
            if (usePadding)
            {
                float paddingPixels = usePadding ? (paddingUnit == DUI_UnitType.Pixels ? padding : (parentWidth * padding / 100)) : 0;
                paddingPixels *= 2;
                float maxWidthDueToPadding = parentWidth - paddingPixels;
                newWidth = Mathf.Min(newWidth, maxWidthDueToPadding);
            }
            if (prevWidth != newWidth)
            {
                SetWidth(newWidth);
            }

        }

        // public override void OnPreParentChanged(RectTransform parent)
        // {
        //     Calculate();
        // }
    }
}