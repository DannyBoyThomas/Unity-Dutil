using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using System;
public static class ByteExtensions
{
    public static float ToFloat(this byte[] bytes)
    {
        return System.BitConverter.ToSingle(bytes, 0);
    }
    public static int ToInt(this byte[] bytes)
    {
        return System.BitConverter.ToInt32(bytes, 0);
    }
    public static bool ToBool(this byte[] bytes)
    {
        return System.BitConverter.ToBoolean(bytes, 0);
    }
    public static string ToString(this byte[] bytes)
    {
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
    public static Vector2 ToVector2(this byte[] bytes)
    {
        return new Vector2(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, sizeof(float)));
    }
    public static Vector3 ToVector3(this byte[] bytes)
    {
        return new Vector3(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, sizeof(float)), BitConverter.ToSingle(bytes, sizeof(float) * 2));
    }
    public static Color ToColor(this byte[] bytes)
    {
        return new Color(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, sizeof(float)), BitConverter.ToSingle(bytes, sizeof(float) * 2), BitConverter.ToSingle(bytes, sizeof(float) * 3));
    }






    public static byte[] ToBytes(this float value)
    {
        return System.BitConverter.GetBytes(value);
    }
    public static byte[] ToBytes(this int value)
    {
        return System.BitConverter.GetBytes(value);
    }
    public static byte[] ToBytes(this bool value)
    {
        return System.BitConverter.GetBytes(value);
    }
    public static byte[] ToBytes(this string value)
    {
        return System.Text.Encoding.UTF8.GetBytes(value);
    }
    public static byte[] ToBytes(this Vector2 value)
    {
        //use sysdtem.array
        byte[] x = value.x.ToBytes();
        byte[] y = value.y.ToBytes();
        byte[] result = new byte[x.Length + y.Length];
        System.Buffer.BlockCopy(x, 0, result, 0, x.Length);
        System.Buffer.BlockCopy(y, 0, result, x.Length, y.Length);
        return result;

    }
    public static byte[] ToBytes(this Vector3 value)
    {
        //use sysdtem.array
        byte[] x = value.x.ToBytes();
        byte[] y = value.y.ToBytes();
        byte[] z = value.z.ToBytes();
        byte[] result = new byte[x.Length + y.Length + z.Length];
        System.Buffer.BlockCopy(x, 0, result, 0, x.Length);
        System.Buffer.BlockCopy(y, 0, result, x.Length, y.Length);
        System.Buffer.BlockCopy(z, 0, result, x.Length + y.Length, z.Length);
        return result;
    }
    public static byte[] ToBytes(this Color value)
    {
        //use sysdtem.array
        byte[] r = value.r.ToBytes();
        byte[] g = value.g.ToBytes();
        byte[] b = value.b.ToBytes();
        byte[] a = value.a.ToBytes();
        byte[] result = new byte[r.Length + g.Length + b.Length + a.Length];
        System.Buffer.BlockCopy(r, 0, result, 0, r.Length);
        System.Buffer.BlockCopy(g, 0, result, r.Length, g.Length);
        System.Buffer.BlockCopy(b, 0, result, r.Length + g.Length, b.Length);
        System.Buffer.BlockCopy(a, 0, result, r.Length + g.Length + b.Length, a.Length);
        return result;
    }

}