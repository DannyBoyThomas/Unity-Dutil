using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dutil
{
    public class Dutil
    {
        public static Vector3 NearestPointOnLine(Vector3 point, Vector3 pointA, Vector3 pointB)
        {
            return new Line3D(pointA, pointB).NearestPoint(point);
        }
        public static string Hash(int length = 8)
        {
            string available = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            string hash = "";
            for (int i = 0; i < length; i++)
            {
                int index = Random.Range(0, available.Length);
                char c = available[index];
                hash += c;
            }
            return hash;
        }
    }
}