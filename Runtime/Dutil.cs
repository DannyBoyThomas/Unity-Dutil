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
        public static Color RandomColour()
        {
            return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }

        public static List<Vector3> PointsOnCircle(Vector3 centre, Vector3 forward, Vector3 normal, int segments, bool close = false)
        {
            List<Vector3> points = new List<Vector3>();
            float incr = 360 / (float)segments;
            int segmentNum = close ? segments + 1 : segments;
            for (int i = 0; i < segmentNum; i++)
            {
                Vector3 point = Quaternion.Euler(normal * i * incr) * forward;
                points.Add(centre + point);
            }
            return points;
        }
        //Physics
        public static bool LineOfSight(Vector3 pointA, Vector3 pointB)
        {
            Vector3 dir = (pointB - pointA);
            if (Physics.Raycast(pointA, dir.normalized, dir.magnitude))
            {
                return false;
            }
            return true;
        }
        //Static Referencer
        static Dictionary<string, List<Object>> trackedObjects = new Dictionary<string, List<Object>>();

        public static void Track(string key, params Object[] objs)
        {
            List<Object> args = objs.ToList();
            args.Clean();
            string cleanKey = key.Replace(" ", "").ToLower();
            for (int i = 0; i < args.Count; i++)
            {
                Object obj = args[i];

                List<Object> list = new List<Object>();
                trackedObjects.TryGetValue(cleanKey, out list);
                if (list == null)
                {
                    list = new List<Object>();
                }

                list.AddUnique(obj);
                if (list.Count > 0)
                {
                    trackedObjects[cleanKey] = list;
                }
                else
                {
                    trackedObjects.Remove(cleanKey);
                }

            }
        }
        public static List<Object> Track(string key)
        {
            string cleanKey = key.Replace(" ", "").ToLower();
            List<Object> list = new List<Object>();
            trackedObjects.TryGetValue(cleanKey, out list);
            if (list == null) { list = new List<Object>(); }
            return list;
        }
        public static Object TrackFirst(string key)
        {
            string cleanKey = key.Replace(" ", "").ToLower();
            List<Object> list = new List<Object>();
            trackedObjects.TryGetValue(cleanKey, out list);
            if (list != null && list.Count > 0)
            {
                return list.First();
            }

            return null;
        }
        public static void Untrack(string key)
        {
            List<Object> list = new List<Object>();
            string cleanKey = key.Replace(" ", "").ToLower();
            if (trackedObjects.ContainsKey(cleanKey))
            {
                trackedObjects.Remove(cleanKey);
            }
        }
        public static void Untrack(string key, params Object[] objs)
        {
            List<Object> list = new List<Object>();
            string cleanKey = key.Replace(" ", "").ToLower();
            if (trackedObjects.ContainsKey(cleanKey))
            {
                trackedObjects.TryGetValue(cleanKey, out list);
                objs.ToList().ForEach(x => list.Remove(x));
                if (list.Count > 0)
                {
                    trackedObjects[cleanKey] = list;
                }
                else
                {
                    Untrack(key);
                }
            }
        }
        public static void ForgetAll()
        {
            trackedObjects.Clear();
        }
        public static List<string> TrackedKeys()
        {
            return trackedObjects.Keys.ToList();
        }
    }
}