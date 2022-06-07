using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;

public class Gradual
{

    public static float Lerp(float t, params float[] values)
    {
        int count = values.Length;
        if (count < 2) { return 0; }
        if (t == 1) { return values[count - 1]; }
        if (t == 0) { return values[0]; }
        int indexFrom = (int)(t * (count - 1));
        int indexTo = Mathf.Min(count - 1, indexFrom + 1);
        float tAtIndexFrom = (float)indexFrom / (count - 1);
        float tAtIndexTo = (float)indexTo / (count - 1);
        return t.Map(tAtIndexFrom, tAtIndexTo, values[indexFrom], values[indexTo]);
    }
    public static Vector2 Lerp(float t, params Vector2[] values)
    {
        int count = values.Length;
        if (count < 2) { return Vector2.zero; }
        if (t == 1) { return values[count - 1]; }
        if (t == 0) { return values[0]; }
        int indexFrom = (int)(t * (count - 1));
        int indexTo = Mathf.Min(count - 1, indexFrom + 1);
        float tAtIndexFrom = (float)indexFrom / (count - 1);
        float tAtIndexTo = (float)indexTo / (count - 1);
        float inverseT = Mathf.InverseLerp(tAtIndexFrom, tAtIndexTo, t);
        return Vector2.Lerp(values[indexFrom], values[indexTo], inverseT);
    }
    //vector3
    public static Vector3 Lerp(float t, params Vector3[] values)
    {
        int count = values.Length;
        if (count < 2) { return Vector3.zero; }
        if (t == 1) { return values[count - 1]; }
        if (t == 0) { return values[0]; }
        int indexFrom = (int)(t * (count - 1));
        int indexTo = Mathf.Min(count - 1, indexFrom + 1);
        float tAtIndexFrom = (float)indexFrom / (count - 1);
        float tAtIndexTo = (float)indexTo / (count - 1);
        float inverseT = Mathf.InverseLerp(tAtIndexFrom, tAtIndexTo, t);
        return Vector3.Lerp(values[indexFrom], values[indexTo], inverseT);
    }
    //color
    public static Color Lerp(float t, params Color[] values)
    {
        int count = values.Length;
        if (count < 2) { return Color.white; }
        if (t == 1) { return values[count - 1]; }
        if (t == 0) { return values[0]; }
        int indexFrom = (int)(t * (count - 1));
        int indexTo = Mathf.Min(count - 1, indexFrom + 1);
        float tAtIndexFrom = (float)indexFrom / (count - 1);
        float tAtIndexTo = (float)indexTo / (count - 1);
        float inverseT = Mathf.InverseLerp(tAtIndexFrom, tAtIndexTo, t);
        return Color.Lerp(values[indexFrom], values[indexTo], inverseT);
    }
}