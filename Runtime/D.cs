using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.Events;
namespace Dutil
{
    public class D
    {

        /// <summary>
        /// Chance(0.7) has a 70% chance to return true
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        /// 
        public static bool AllowLogging = true;
        public static bool Chance(float f)
        {
            f = Mathf.Clamp01(f);
            return Random.Range(0, 1f) <= f;
        }
        [System.Obsolete("Use GetClosestPointOnLine() instead")]
        public static Vector3 NearestPointOnLine(Vector3 point, Vector3 pointA, Vector3 pointB)
        {
            return new Line3D(pointA, pointB).NearestPoint(point);
        }
        public static Vector3 GetClosestPointOnLine(Vector3 p1, Vector3 p2, Vector3 point)
        {
            Vector3 v = p2 - p1;
            Vector3 w = point - p1;
            float c1 = Vector3.Dot(w, v);
            if (c1 <= 0)
                return p1;
            float c2 = Vector3.Dot(v, v);
            if (c2 <= c1)
                return p2;
            float b = c1 / c2;
            Vector3 pb = p1 + b * v;
            return pb;
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
            for (int i = 0; i <= divisions; i++)
            {
                float t = i * incr;
                Vector3 pointAtT = (Mathf.Pow(1 - t, 2) * p0) + (2 * t * (1 - t) * p1) + (Mathf.Pow(t, 2) * p2);
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
        public static string FormatNumber(double number, int maxDecimals = 4)
        {
            return FormatNumber((float)number, maxDecimals);
        }
        public static string FormatNumber(float number, int maxDecimals = 4)
        {
            return Regex.Replace(string.Format("{0:n" + maxDecimals + "}", number),
                                 @"[" + System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "]?0+$", "");
        }
        /// <summary>
        /// Does what it says on the tin
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"> A list of every item it could be</param>
        /// <param name="first">Where the flood fill should start</param>
        /// <param name="GetNeighbours">(list,currentItem)=>{return this currentItems neighbours}</param>
        /// <returns>A list of all connected items</returns>
        public static List<T> FloodFill<T>(List<T> list, T first, System.Func<List<T>, T, List<T>> GetNeighbours)
        {
            List<T> flooded = new List<T>();
            flooded.Add(first);
            for (int i = 0; i < flooded.Count; i++)
            {
                T current = flooded[i];
                List<T> neighbours = GetNeighbours(list, current);
                for (int j = 0; j < neighbours.Count; j++)
                {
                    T neigh = neighbours[j];
                    if (list.Contains(neigh))
                    {
                        flooded.AddUnique(neigh);
                    }
                }
            }
            return flooded;
        }
        public static float EquallySpace(float totalLength, int index, int count)
        {
            float singleSpacing = totalLength / (count + 1f);
            float relativeSpacing = (index + 1) * singleSpacing;
            float val = (relativeSpacing - (totalLength / 2f));
            return val;
        }
        public static void Log(object message)
        {
            if (!AllowLogging) { return; }
            Debug.Log(message);
        }
    }






}

