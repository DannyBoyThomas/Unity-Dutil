using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dutil
{
    public static class MathExtensions
    {
        public static float Clamp(this float f) => Mathf.Clamp01(f);
        public static float Square(this float f) => f * f;
        public static float SquareRoot(this float f) => Mathf.Sqrt(f);
        public static float To(this float f, float p) => Mathf.Pow(f, p);
        public static bool Between(this float f, float min, float max) => f >= min && f <= max;

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