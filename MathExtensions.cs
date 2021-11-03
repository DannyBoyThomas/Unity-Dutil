using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dutil
{
    public static class MathExtensions
    {
        public static float Clamp(this float f) => Mathf.Clamp01(f);
    }
}