using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Dutil;
using System;
public class AutoLerpTask
{
    float Progress { get { return (passedTime / targetDuration).Clamp(); } }

    bool shouldRemove = false;
    float passedTime = 0;
    float targetDuration = 1;
    bool useUnscaledDeltaTime = false;
    /*  UnityAction<DevelopTask, float> callback; */
    UnityAction<AutoLerpTask, float> callbackFloat;
    UnityAction<AutoLerpTask, int> callbackInt;
    UnityAction<AutoLerpTask, Vector2> callbackVector2;
    UnityAction<AutoLerpTask, Vector3> callbackVector3;
    UnityAction<AutoLerpTask, Quaternion> callbackQuaternion;
    UnityAction<AutoLerpTask, Color> callbackColor;
    UnityAction<AutoLerpTask> onCompleteCallback;
    byte[] from, to;
    bool useEasing = false;
    DevelopTypes developType = DevelopTypes.None;

    public AutoLerpTask(float _duration, UnityAction<AutoLerpTask, float> _callback, bool _ease = false)
    {
        targetDuration = _duration;
        callbackFloat = _callback;
        useEasing = _ease;
    }
    public AutoLerpTask(float _duration, float start, float end, UnityAction<AutoLerpTask, float> _callback, bool _ease = false)
    {
        targetDuration = _duration;
        callbackFloat = _callback;
        useEasing = _ease;
        SetFloat(start, end);
    }
    public AutoLerpTask(float _duration, int start, int end, UnityAction<AutoLerpTask, int> _callback, bool _ease = false)
    {
        targetDuration = _duration;
        callbackInt = _callback;
        useEasing = _ease;
        SetInt(start, end);
    }
    public AutoLerpTask(float _duration, Vector2 start, Vector2 end, UnityAction<AutoLerpTask, Vector2> _callback, bool _ease = false)
    {
        targetDuration = _duration;
        callbackVector2 = _callback;
        useEasing = _ease;
        SetVector2(start, end);
    }
    public AutoLerpTask(float _duration, Vector3 start, Vector3 end, UnityAction<AutoLerpTask, Vector3> _callback, bool _ease = false)
    {
        targetDuration = _duration;
        callbackVector3 = _callback;
        useEasing = _ease;
        SetVector3(start, end);
    }
    public AutoLerpTask(float _duration, Quaternion start, Quaternion end, UnityAction<AutoLerpTask, Quaternion> _callback, bool _ease = false)
    {
        targetDuration = _duration;
        callbackQuaternion = _callback;
        useEasing = _ease;
        SetQuaternion(start, end);
    }
    public AutoLerpTask(float _duration, Color start, Color end, UnityAction<AutoLerpTask, Color> _callback, bool _ease = false)
    {
        targetDuration = _duration;
        callbackColor = _callback;
        useEasing = _ease;
        SetColor(start, end);
    }
    public virtual bool Tick()
    {
        if (shouldRemove) { return true; }
        passedTime += useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
        try
        {
            CallCallback();
        }
        catch (Exception e)
        {
            Debug.LogWarning("[Dutil] AutoLerp: ".Color(Colours.Orange).B() + "Task removed from AutoLerp.\n" + e.Message);
            return true;
        }

        if (Progress == 1)
        {
            onCompleteCallback?.Invoke(this);
            return true;
        }
        return Progress == 1;
    }
    public AutoLerpTask Cancel()
    {
        shouldRemove = true;
        return this;
    }
    float EasedProgress
    {
        get
        {
            float p = Progress;
            float q = 1 - p;

            return Mathf.Lerp(p * p * p, 1 - (q * q * q), p);

        }
    }
    void CallCallback()
    {
        switch (developType)
        {
            case DevelopTypes.Float:
                if (callbackFloat == null) { Cancel(); return; }

                callbackFloat?.Invoke(this, GetValueFloat()); return;
            case DevelopTypes.Integer:
                if (callbackInt == null) { Cancel(); return; }
                callbackInt?.Invoke(this, GetValueInt()); return;
            case DevelopTypes.Vector2:
                if (callbackVector2 == null) { Cancel(); return; }
                callbackVector2?.Invoke(this, GetValueVector2()); return;
            case DevelopTypes.Vector3:
                if (callbackVector3 == null) { Cancel(); return; }
                callbackVector3?.Invoke(this, GetValueVector3()); return;
            case DevelopTypes.Quaternion:
                if (callbackQuaternion == null) { Cancel(); return; }
                callbackQuaternion?.Invoke(this, GetValueQuaternion()); return;
            case DevelopTypes.Color:
                if (callbackColor == null) { Cancel(); return; }
                callbackColor?.Invoke(this, GetValueColor()); return;
            default:
                if (callbackFloat == null) { Cancel(); return; }
                callbackFloat?.Invoke(this, useEasing ? EasedProgress : Progress); return;
        }
    }

    public AutoLerpTask OnComplete(UnityAction<AutoLerpTask> callback)
    {
        onCompleteCallback = callback;
        return this;
    }
    public AutoLerpTask UseUnscaledDeltaTime(bool use = true)
    {
        useUnscaledDeltaTime = use;
        return this;
    }

    public void SetInt(int a, int b)
    {
        developType = DevelopTypes.Integer;
        from = BitConverter.GetBytes(a);
        to = BitConverter.GetBytes(b);
    }
    public void SetFloat(float a, float b)
    {
        developType = DevelopTypes.Float;
        from = BitConverter.GetBytes(a);
        to = BitConverter.GetBytes(b);
    }
    //SetVector2
    public void SetVector2(Vector2 a, Vector2 b)
    {
        developType = DevelopTypes.Vector2;
        from = new byte[sizeof(float) * 2];
        Buffer.BlockCopy(BitConverter.GetBytes(a.x), 0, from, 0 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(a.y), 0, from, 1 * sizeof(float), sizeof(float));
        to = new byte[sizeof(float) * 2];
        Buffer.BlockCopy(BitConverter.GetBytes(b.x), 0, to, 0 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(b.y), 0, to, 1 * sizeof(float), sizeof(float));
    }
    public void SetVector3(Vector3 a, Vector3 b)
    {
        developType = DevelopTypes.Vector3;
        from = new byte[sizeof(float) * 3];
        Buffer.BlockCopy(BitConverter.GetBytes(a.x), 0, from, 0 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(a.y), 0, from, 1 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(a.z), 0, from, 2 * sizeof(float), sizeof(float));
        to = new byte[sizeof(float) * 3];
        Buffer.BlockCopy(BitConverter.GetBytes(b.x), 0, to, 0 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(b.y), 0, to, 1 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(b.z), 0, to, 2 * sizeof(float), sizeof(float));
    }
    public void SetQuaternion(Quaternion a, Quaternion b)
    {
        developType = DevelopTypes.Quaternion;
        from = new byte[sizeof(float) * 4];
        Buffer.BlockCopy(BitConverter.GetBytes(a.x), 0, from, 0 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(a.y), 0, from, 1 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(a.z), 0, from, 2 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(a.w), 0, from, 3 * sizeof(float), sizeof(float));
        to = new byte[sizeof(float) * 4];
        Buffer.BlockCopy(BitConverter.GetBytes(b.x), 0, to, 0 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(b.y), 0, to, 1 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(b.z), 0, to, 2 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(b.w), 0, to, 3 * sizeof(float), sizeof(float));
    }
    public void SetColor(Color a, Color b)
    {
        developType = DevelopTypes.Color;
        from = new byte[sizeof(float) * 4];
        Buffer.BlockCopy(BitConverter.GetBytes(a.r), 0, from, 0 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(a.g), 0, from, 1 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(a.b), 0, from, 2 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(a.a), 0, from, 3 * sizeof(float), sizeof(float));
        to = new byte[sizeof(float) * 4];
        Buffer.BlockCopy(BitConverter.GetBytes(b.r), 0, to, 0 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(b.g), 0, to, 1 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(b.b), 0, to, 2 * sizeof(float), sizeof(float));
        Buffer.BlockCopy(BitConverter.GetBytes(b.a), 0, to, 3 * sizeof(float), sizeof(float));
    }
    /*  public dynamic GetValue()
     {
         float t = useEasing ? EasedProgress : Progress;
         switch (developType)
         {
             case DevelopTypes.Integer:
                 int a = BitConverter.ToInt32(from, 0);
                 int b = BitConverter.ToInt32(to, 0);
                 return (int)Mathf.Lerp(a, b, t);
             case DevelopTypes.Float:
                 float fa = BitConverter.ToSingle(from, 0);
                 float fb = BitConverter.ToSingle(to, 0);
                 return Mathf.Lerp(fa, fb, t);
             case DevelopTypes.Vector2:
                 Vector2 va = new Vector2(BitConverter.ToSingle(from, 0), BitConverter.ToSingle(from, sizeof(float)));
                 Vector2 vb = new Vector2(BitConverter.ToSingle(to, 0), BitConverter.ToSingle(to, sizeof(float)));
                 return Vector2.Lerp(va, vb, t);
             case DevelopTypes.Vector3:
                 Vector3 va3 = new Vector3(BitConverter.ToSingle(from, 0), BitConverter.ToSingle(from, sizeof(float)), BitConverter.ToSingle(from, sizeof(float) * 2));
                 Vector3 vb3 = new Vector3(BitConverter.ToSingle(to, 0), BitConverter.ToSingle(to, sizeof(float)), BitConverter.ToSingle(to, sizeof(float) * 2));
                 return Vector3.Lerp(va3, vb3, t);
             case DevelopTypes.Quaternion:
                 Quaternion qa = new Quaternion(BitConverter.ToSingle(from, 0), BitConverter.ToSingle(from, sizeof(float)), BitConverter.ToSingle(from, sizeof(float) * 2), BitConverter.ToSingle(from, sizeof(float) * 3));
                 Quaternion qb = new Quaternion(BitConverter.ToSingle(to, 0), BitConverter.ToSingle(to, sizeof(float)), BitConverter.ToSingle(to, sizeof(float) * 2), BitConverter.ToSingle(to, sizeof(float) * 3));
                 return Quaternion.Lerp(qa, qb, t);
             case DevelopTypes.Color:
                 Color ca = new Color(BitConverter.ToSingle(from, 0), BitConverter.ToSingle(from, sizeof(float)), BitConverter.ToSingle(from, sizeof(float) * 2), BitConverter.ToSingle(from, sizeof(float) * 3));
                 Color cb = new Color(BitConverter.ToSingle(to, 0), BitConverter.ToSingle(to, sizeof(float)), BitConverter.ToSingle(to, sizeof(float) * 2), BitConverter.ToSingle(to, sizeof(float) * 3));
                 return Color.Lerp(ca, cb, t);
             default:
                 return t;
         }
     } */

    public float GetValueFloat()
    {
        float t = useEasing ? EasedProgress : Progress;
        float fa = BitConverter.ToSingle(from, 0);
        float fb = BitConverter.ToSingle(to, 0);
        return Mathf.Lerp(fa, fb, t);
    }
    public int GetValueInt()
    {
        float t = useEasing ? EasedProgress : Progress;
        int a = BitConverter.ToInt32(from, 0);
        int b = BitConverter.ToInt32(to, 0);
        return (int)Mathf.Lerp(a, b, t);
    }
    public Vector2 GetValueVector2()
    {
        float t = useEasing ? EasedProgress : Progress;
        Vector2 va = new Vector2(BitConverter.ToSingle(from, 0), BitConverter.ToSingle(from, sizeof(float)));
        Vector2 vb = new Vector2(BitConverter.ToSingle(to, 0), BitConverter.ToSingle(to, sizeof(float)));
        return Vector2.Lerp(va, vb, t);
    }
    public Vector3 GetValueVector3()
    {
        float t = useEasing ? EasedProgress : Progress;
        Vector3 va3 = new Vector3(BitConverter.ToSingle(from, 0), BitConverter.ToSingle(from, sizeof(float)), BitConverter.ToSingle(from, sizeof(float) * 2));
        Vector3 vb3 = new Vector3(BitConverter.ToSingle(to, 0), BitConverter.ToSingle(to, sizeof(float)), BitConverter.ToSingle(to, sizeof(float) * 2));
        return Vector3.Lerp(va3, vb3, t);
    }
    public Quaternion GetValueQuaternion()
    {
        float t = useEasing ? EasedProgress : Progress;
        Quaternion qa = new Quaternion(BitConverter.ToSingle(from, 0), BitConverter.ToSingle(from, sizeof(float)), BitConverter.ToSingle(from, sizeof(float) * 2), BitConverter.ToSingle(from, sizeof(float) * 3));
        Quaternion qb = new Quaternion(BitConverter.ToSingle(to, 0), BitConverter.ToSingle(to, sizeof(float)), BitConverter.ToSingle(to, sizeof(float) * 2), BitConverter.ToSingle(to, sizeof(float) * 3));
        return Quaternion.Lerp(qa, qb, t);
    }
    public Color GetValueColor()
    {
        float t = useEasing ? EasedProgress : Progress;
        Color ca = new Color(BitConverter.ToSingle(from, 0), BitConverter.ToSingle(from, sizeof(float)), BitConverter.ToSingle(from, sizeof(float) * 2), BitConverter.ToSingle(from, sizeof(float) * 3));
        Color cb = new Color(BitConverter.ToSingle(to, 0), BitConverter.ToSingle(to, sizeof(float)), BitConverter.ToSingle(to, sizeof(float) * 2), BitConverter.ToSingle(to, sizeof(float) * 3));
        return Color.Lerp(ca, cb, t);
    }
}
public enum DevelopTypes { None, Float, Integer, Vector2, Vector3, Color, Quaternion }
