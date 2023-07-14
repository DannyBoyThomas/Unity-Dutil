using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dutil
{
    public static class MathExtensions
    {
        public static float Clamp(this float f) => Mathf.Clamp01(f);
        public static float Clamp(this float f, float min, float max) => Mathf.Clamp(f, min, max);
        public static int Clamp(this int f, int min, int max) => Mathf.Clamp(f, min, max);
        public static float Square(this float f) => f * f;
        public static float SquareRoot(this float f) => Mathf.Sqrt(f);
        public static float To(this float f, float p) => Mathf.Pow(f, p);
        public static float To(this int f, float p) => Mathf.Pow(f, p);
        public static bool Between(this float f, float min, float max) => f >= min && f <= max;
        // public static float Map(this float f, float oldMin, float oldMax, float newMin, float newMax) => (f - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
        // public static float Map(this int f, float oldMin, float oldMax, float newMin, float newMax) => (f - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
        public static float Map(this float f, float oldMin, float oldMax, float newMin, float newMax, bool clamp = false)
        {
            float val = (f - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
            if (clamp)
            {
                val = val.Clamp(newMin, newMax);
            }
            return val;
        }
        public static float Map(this int f, float oldMin, float oldMax, float newMin, float newMax, bool clamp = false)
        {
            return ((float)f).Map(oldMin, oldMax, newMin, newMax, clamp);
        }
        public static int Round(this float f) => Mathf.RoundToInt(f);
        public static Vector3 X(this float f) => new Vector3(f, 0, 0);
        public static Vector3 Y(this float f) => new Vector3(0, f, 0);
        public static Vector3 Z(this float f) => new Vector3(0, 0, f);
        public static Vector3 XY(this float f) => new Vector3(f, f, 0);
        public static Vector3 XZ(this float f) => new Vector3(f, 0, f);
        public static Vector3 YZ(this float f) => new Vector3(0, f, f);
        public static Vector3 XYZ(this float f) => new Vector3(f, f, f);
        //same with integers
        public static Vector3 X(this int f) => new Vector3(f, 0, 0);
        public static Vector3 Y(this int f) => new Vector3(0, f, 0);
        public static Vector3 Z(this int f) => new Vector3(0, 0, f);
        public static Vector3 XY(this int f) => new Vector3(f, f, 0);
        public static Vector3 XZ(this int f) => new Vector3(f, 0, f);
        public static Vector3 YZ(this int f) => new Vector3(0, f, f);
        public static Vector3 XYZ(this int f) => new Vector3(f, f, f);


    }
}