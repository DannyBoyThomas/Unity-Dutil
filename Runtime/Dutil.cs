using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Dutil
{
    public class D
    {
        /// <summary>
        /// Chance(0.7) has a 70% chance to return true
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static bool Chance(float f)
        {
            f = Mathf.Clamp01(f);
            return Random.Range(0, 1f) <= f;
        }
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

        public static List<Vector3> BezierPoints(Vector3 p0, Vector3 p1, Vector3 p2, int divisions = 20)
        {
            List<Vector3> points = new List<Vector3>();
            float incr = 1 / (float)divisions;
            for (int i = 0; i < divisions; i++)
            {
                float t = i * incr;
                Vector3 pointAtT = p0 + (Mathf.Pow(1 - t, 2) * (p0 - p1)) + (Mathf.Pow(t, 2) * (p2 - p1));
                points.Add(pointAtT);
            }
            return points;
        }
        public static List<Vector3> BezierPoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int divisions = 20)
        {
            List<Vector3> points = new List<Vector3>();
            float incr = 1 / (float)divisions;
            for (int i = 0; i <= divisions; i++)
            {
                float t = i * incr;
                Vector3 pointAtT = (Mathf.Pow(1 - t, 3) * p0) + (3 * Mathf.Pow(1 - t, 2) * t * p1) + (3 * (1 - t) * t * t * p2) + (t * t * t * p3);
                points.Add(pointAtT);
            }
            return points;
        }



    }
}