using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
namespace Dutil
{
    public class DUI_Height : DUI_Rect, IDUI_SizeManipulator
    {
        public float targetHeight = 100;
        public bool useTargetPercentage = false;
        [Range(0, 100)]
        public float targetHeightPercentage = 100;

        public bool useMinHeight = false;
        public float minHeight = 0;
        public DUI_UnitType minHeightUnit = DUI_UnitType.Pixels;

        public bool useMaxHeight = false;
        public float maxHeight = 10000;
        public DUI_UnitType maxHeightUnit = DUI_UnitType.Pixels;

        public bool usePadding = false;
        public float padding = 0;
        public DUI_UnitType paddingUnit = DUI_UnitType.Pixels;




        // Start is called before the first frame update


        public override void Calculate()
        {
            float prevHeight = GetHeight();
            float parentHeight = Parent()?.GetHeight() ?? 0;


            float newHeight = useTargetPercentage ? (parentHeight * targetHeightPercentage / 100) : targetHeight;


            if (useMaxHeight)
            {
                float maxTargetHeight = maxHeightUnit == DUI_UnitType.Pixels ? maxHeight : (parentHeight * maxHeight / 100);
                newHeight = Mathf.Min(newHeight, maxTargetHeight);
            }
            if (useMinHeight)
            {
                float minTargetHeight = minHeightUnit == DUI_UnitType.Pixels ? minHeight : (parentHeight * minHeight / 100);
                newHeight = Mathf.Max(newHeight, minTargetHeight);
            }
            if (usePadding)
            {
                float paddingPixels = usePadding ? (paddingUnit == DUI_UnitType.Pixels ? padding : (parentHeight * padding / 100)) : 0;
                float maxHeightDueToPadding = parentHeight - paddingPixels;
                newHeight = Mathf.Min(newHeight, maxHeightDueToPadding);
            }

            if (prevHeight != newHeight)
            {
                SetHeight(newHeight);
            }

        }

        // public override void OnPreParentChanged(RectTransform parent)
        // {
        //     Calculate();
        // }
    }
}