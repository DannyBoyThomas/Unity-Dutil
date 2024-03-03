using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Codice.Client.Common;




#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Dutil
{
    public static class D
    {
        //d_auto_insert
        public static bool AllowLogging
        {
            get
            {
#if UNITY_EDITOR
                return EditorPrefs.GetBool("d_allow_logging", true);
#else
                return false;
#endif
            }
            set
            {
#if UNITY_EDITOR
                EditorPrefs.SetBool("d_allow_logging", value);
#endif
            }
        }



        /// <summary>
        /// Chance(0.7) has a 70% chance to return true
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        /// 

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
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="point"></param>
        /// <param name="list"> All options</param>
        /// <param name="GetPosition"> (item)=>item.position</param>
        /// <returns></returns>
        public static T GetClosest<T>(Vector3 point, List<T> list, System.Func<T, Vector3> GetPosition)
        {
            T closest = default(T);
            float closestDist = Mathf.Infinity;
            for (int i = 0; i < list.Count; i++)
            {
                T item = list[i];
                Vector3 pos = GetPosition(item);
                float dist = Vector3.Distance(point, pos);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = item;
                }
            }
            return closest;
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
        public static T RandomEnum<T>() where T : System.Enum
        {
            System.Array values = System.Enum.GetValues(typeof(T));
            return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }

        public static List<Vector3> PointsOnCircle(Vector3 centre, Vector3 forward, Vector3 normal, int segments, float radius = 1f, bool close = false)
        {
            List<Vector3> points = new List<Vector3>();
            float incr = 360 / (float)segments;
            int segmentNum = close ? segments + 1 : segments;
            for (int i = 0; i < segmentNum; i++)
            {
                Vector3 point = Quaternion.Euler(normal * i * incr) * forward;
                points.Add(centre + point * radius);
            }
            return points;
        }
        public static List<Vector3> PointsOnSphere(int n, float radius = 1)
        {
            List<Vector3> upts = new List<Vector3>();
            float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
            float off = 2.0f / n;
            float x = 0;
            float y = 0;
            float z = 0;
            float r = 0;
            float phi = 0;

            for (var k = 0; k < n; k++)
            {
                y = k * off - 1 + (off / 2);
                r = Mathf.Sqrt(1 - y * y);
                phi = k * inc;
                x = Mathf.Cos(phi) * r;
                z = Mathf.Sin(phi) * r;

                upts.Add(new Vector3(x, y, z) * radius);
            }

            return upts;
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
        public static bool LineOfSight(Vector3 pointA, Vector3 pointB, List<Collider> colsToIgnore = null)
        {
            if (colsToIgnore == null)
            {
                colsToIgnore = new List<Collider>();
            }
            Vector3 dir = (pointB - pointA);
            List<RaycastHit> hits = Physics.RaycastAll(pointA, dir.normalized, dir.magnitude).ToList();
            hits = hits.Where(x => !colsToIgnore.Contains(x.collider)).ToList();
            if (hits.Count > 0)
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
                list.Clean();
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
            return Track<Object>(key);
        }
        public static List<T> Track<T>(string key)
        {
            string cleanKey = key.Replace(" ", "").ToLower();
            List<Object> allTracked = new List<Object>();

            trackedObjects.TryGetValue(cleanKey, out allTracked);
            if (allTracked == null) { return new List<T>(); }
            List<T> list = allTracked.Where(x => x is T).Cast<T>().ToList();
            return list;
        }
        public static Object TrackFirst(string key)
        {
            string cleanKey = key.Replace(" ", "").ToLower();
            List<Object> list = new List<Object>();
            trackedObjects.TryGetValue(cleanKey, out list);

            if (list != null && list.Count > 0)
            {
                return list.FirstOrDefault(x => x != null);
            }

            return null;
        }
        public static T TrackFirst<T>(string key)
        {
            Object obj = TrackFirst(key);
            if (obj != null && obj is T)
            {
                return (T)System.Convert.ChangeType(TrackFirst(key), typeof(T));
            }
            return default(T);

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
        static string CreateAnchor(object message, Color col, Color messageCol, [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
        {
            try
            {
                string className = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;
                string info = $"[{className}.{callerName}:{callerLine}]: ".Color(col).B();
                string anchor = $"<a href=\"{callerPath}\" line=\"{callerLine}\">" + info + "</a>";
                return anchor + message.ToString().Color(messageCol);
            }
            catch
            {
                return message.ToString();
            }
        }
        public static void Log(object message, Color color = default(Color), [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
        {
            if (!AllowLogging) { return; }
            Color textCol = color == default(Color) ? Color.white : color;
            UnityEngine.Debug.Log(CreateAnchor(message, Colours.Blue, textCol, callerName, callerPath, callerLine));
        }
        public static void LogWarning(object message, Color color = default(Color), [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
        {
            if (!AllowLogging) { return; }
            Color textCol = color == default(Color) ? Color.white : color;
            UnityEngine.Debug.LogWarning(CreateAnchor(message, Colours.Orange, textCol, callerName, callerPath, callerLine));
        }
        public static void LogError(object message, Color color = default(Color), [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
        {
            if (!AllowLogging) { return; }
            Color textCol = color == default(Color) ? Color.white : color;
            UnityEngine.Debug.LogError(CreateAnchor(message, Colours.Red, textCol, callerName, callerPath, callerLine));
        }





        /// <summary>
        /// Returns a skewed value between 0 and 1, based on bias.
        /// See https://youtu.be/lctXaT9pxA0?t=446
        /// </summary>
        /// <param name="x">The value to bias.</param>
        /// <param name="bias">The bias value.</param>
        public static float Bias(float x, float bias)
        {
            float k = Mathf.Pow(1 - bias, 3);
            return (x * k) / (x * k - x + 1);
        }
        /// <summary>
        /// Returns a normalised Vector2, pointing in the direction of the Vector2.Up rotated around the Z-axis.
        /// </summary>
        /// <param name="angle">The angle in radians.</param>
        public static Vector2 VectorFromAngle(float angle)
        {
            return new Vector2(Mathf.Cos((-angle + 90) * Mathf.Deg2Rad), Mathf.Sin((-angle + 90) * Mathf.Deg2Rad)).normalized;
        }

        public static void DrawSquareXY(Vector3 centre, float size, Color? col = null, float time = 1)
        {
            Color color = col ?? Colours.Pink;
            Vector3 halfSize = Vector3.one * size / 2f;
            Vector3 p1 = centre + new Vector3(-halfSize.x, -halfSize.y, 0);
            Vector3 p2 = centre + new Vector3(halfSize.x, -halfSize.y, 0);
            Vector3 p3 = centre + new Vector3(halfSize.x, halfSize.y, 0);
            Vector3 p4 = centre + new Vector3(-halfSize.x, halfSize.y, 0);
            UnityEngine.Debug.DrawLine(p1, p2, color, time);
        }
        static string PermaBoolFileName = "dutil/perma_bools";
        /// <summary>
        /// A bool that returns true once, then returns false forever.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="autoLock"> Reading the bool, sets it to false</param>
        /// <returns></returns>
        public static bool PermaBool(string key, bool autoLock = true)
        {
            //return true if not stored in list
            PermaCheckList checkList = Drive.Load<PermaCheckList>(PermaBoolFileName, new PermaCheckList());
            if (!checkList.list.Contains(key))
            {
                if (autoLock)
                {
                    checkList.list.Add(key);
                    Drive.Save(checkList, PermaBoolFileName, Drive.DriveType.JSON);
                }
                return true;
            }
            return false;
        }
        public static void ResetPermaBool(string key)
        {
            PermaCheckList checkList = Drive.Load<PermaCheckList>(PermaBoolFileName, new PermaCheckList());
            checkList.list.Remove(key);
            Drive.Save(checkList, PermaBoolFileName, Drive.DriveType.JSON);
        }
        public static void ResetAllPermaBools()
        {
            Drive.Remove(PermaBoolFileName);
        }
        public static List<string> GetPermaBools()
        {
            PermaCheckList checkList = Drive.Load<PermaCheckList>(PermaBoolFileName, new PermaCheckList());
            return checkList.list;
        }

    }






}

