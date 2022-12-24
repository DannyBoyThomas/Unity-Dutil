using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
namespace Dutil
{
    public class DUI_ForceSquare : DUI_Rect
    {
        [Tooltip("Tries to create a square by matching the width or height to the adjacent dimension.")]
        public DUISquareType autoAlter = DUISquareType.None;
        public override void Calculate()
        {
            if (autoAlter == DUISquareType.Width && HasWidthOverride())
            {
                autoAlter = DUISquareType.None;
                Debug.Log("DUI_ForceSquare: Width override found, match type set to None");
            }
            if (autoAlter == DUISquareType.Height && HasHeightOverride())
            {
                autoAlter = DUISquareType.None;
                Debug.Log("DUI_ForceSquare: Height override found, match type set to None");
            }
            if (autoAlter == DUISquareType.Width)
            {
                SetWidth(GetHeight());
            }
            else if (autoAlter == DUISquareType.Height)
            {
                SetHeight(GetWidth());
            }
        }
        public bool HasWidthOverride()
        {
            return GetComponent<DUI_Width>();
        }
        public bool HasHeightOverride()
        {
            return GetComponent<DUI_Height>();
        }
    }
    public enum DUISquareType { None, Width, Height };
}