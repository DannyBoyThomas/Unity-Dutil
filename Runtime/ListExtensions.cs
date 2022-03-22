using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
namespace Dutil
{
    public static class ListExtensions
    {

        public static List<T> Fill<T>(this List<T> list, int count, T item) => Enumerable.Repeat(item, count).ToList();
        public static void Print<T>(this List<T> list)
        {
            if (list == null || list.Count <= 0) { Debug.Log("Empty"); return; }
            list.ForEach(x => Debug.Log(x));
        }
        public static T Any<T>(this List<T> list)
        {
            int index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }
        public static List<T> Any<T>(this List<T> list, int num)
        {
            List<T> anyList = new List<T>();
            for (int i = 0; i < num; i++)
            {
                anyList.Add(list.Any());
            }
            return anyList;
        }
        public static List<T> AnyUnique<T>(this List<T> list, int num)
        {
            num = Mathf.Clamp(num, 0, list.Count);
            List<T> anyList = list.Shuffle();
            return anyList.Take(num).ToList();
        }
        public static void AddUnique<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }
        public static void AddAll<T>(this List<T> list, params T[] args)
        {
            for (int i = 0; i < args.Count(); i++)
            {
                list.Add(args[i]);
            }
        }
        public static void AddUniqueAll<T>(this List<T> list, params T[] args)
        {
            for (int i = 0; i < args.Count(); i++)
            {
                list.AddUnique(args[i]);
            }
        }
        public static T Get<T>(this List<T> list, int index)
        {
            int max = list.Count;
            int newIndex = index > 0 ? index : max + index;
            return list[newIndex];
        }
        public static List<T> Shuffle<T>(this List<T> list)
        {

            List<T> unshuffled = new List<T>(list);
            List<T> shuffled = new List<T>();
            while (unshuffled.Count > 0)
            {
                T any = unshuffled.Any();
                unshuffled.Remove(any);
                shuffled.Add(any);
            }

            return shuffled;
        }
        public static void Clean<T>(this List<T> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == null) { list.RemoveAt(i); }
            }

        }
        public static List<T> Copy<T>(this List<T> list)
        {
            T[] array = new T[list.Count];
            list.CopyTo(array);
            return array.ToList();
        }
        public static T Pop<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                T element = list[0];
                list.RemoveAt(0);
                return element;
            }
            return default(T);
        }
        public static T FirstD<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                return list[0];
            }
            return default(T);
        }
        public static List<List<T>> Split<T>(this List<T> list, int count)
        {
            List<List<T>> groups = new List<List<T>>();
            List<T> from = list.Copy();

            while (from.Count > 0)
            {
                int toTake = Math.Min(count, from.Count);
                List<T> newGroup = from.GetRange(0, toTake);
                groups.Add(newGroup);
                newGroup.ForEach(x => from.Remove(x));
            }
            return groups;
        }







        //Vector3
        public static Vector3 Average(this List<Vector3> list)
        {
            Vector3 sum = Vector3.zero;
            list.ForEach(x => sum += x);
            sum /= (float)list.Count;
            return sum;
        }
        public static Vector3? ClosestPoint(this List<Vector3> list, Vector3 point)
        {
            Vector3? closest = null;
            float closestDistance = Mathf.Infinity;
            for (int i = 0; i < list.Count; i++)
            {
                float distance = Vector3.Distance(list[i], point);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = list[i];
                }
            }
            return closest;
        }
        public static Vector3 GetClosestPointOnPath(this List<Vector3> points, Vector3 point)
        {
            Vector3 closest = Vector3.zero;
            float minDist = float.MaxValue;
            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector3 p1 = points[i];
                Vector3 p2 = points[i + 1];
                Vector3 closestPoint = D.GetClosestPointOnLine(p1, p2, point);
                float dist = Vector3.Distance(point, closestPoint);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = closestPoint;
                }
            }
            return closest;
        }
        public static Vector3 Move(this List<Vector3> points, float moveDistance, int clampOrExtrapolate = 0)
        {
            Vector3 pos;
            int index;
            (pos, index) = MoveWithIndex(points, moveDistance, clampOrExtrapolate);
            return pos;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="moveDistance"></param>
        /// <param name="clampOrExtrapolate">o = Clamp, 1 = Extrapolate</param>
        /// <returns></returns>
        public static (Vector3, int) MoveWithIndex(this List<Vector3> points, float moveDistance, int clampOrExtrapolate = 0)
        {
            if (clampOrExtrapolate == 0)
            {
                moveDistance = Mathf.Max(moveDistance, 0);
            }
            float remainingMoveDistance = moveDistance;
            //move through all the points starting at the first one
            Vector3 lastPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                Vector3 currentPoint = points[i];
                float distance = Vector3.Distance(lastPoint, currentPoint);
                if (distance <= remainingMoveDistance)//can move across this line completely
                {
                    remainingMoveDistance -= distance;
                    lastPoint = currentPoint;
                }
                else// somewhere on this line
                {
                    Vector3 direction = (currentPoint - lastPoint).normalized;
                    Vector3 newPoint = lastPoint + direction * remainingMoveDistance;
                    return (newPoint, i - 1);
                }
            }
            //ran out of points
            if (clampOrExtrapolate == 1)
            {
                Vector3 direction = (points.Last() - points.Get(-1)).normalized;
                Vector3 newPoint = points.Last() + direction * remainingMoveDistance;
                return (newPoint, points.Count - 1);
            }
            return (points.Last(), points.Count - 1);

        }

        public static List<Vector3> Smooth(this List<Vector3> path, int iterations = 4, bool close = false)
        {
            iterations = Mathf.Clamp(iterations, 0, 6);
            List<Vector3> points = new List<Vector3>(path);

            for (int n = 0; n < iterations; n++)
            {
                var output = new List<Vector3>();
                if (close)
                {
                    var p0 = points[0];
                    var p1 = points[points.Count - 1];
                    var Q = p0 * 0.75f + p1 * 0.25f;
                    output.Add(Q);
                }
                else if (points.Count > 0)
                {
                    output.Add(points[0]);
                }

                for (var i = 0; i < points.Count - 1; i++)
                {
                    var p0 = points[i];
                    var p1 = points[i + 1];

                    var Q = p0 * 0.75f + p1 * 0.25f;
                    var R = p0 * 0.25f + p1 * 0.75f;

                    output.Add(Q);
                    output.Add(R);
                }
                if (close)
                {
                    var p0 = points[points.Count - 1];
                    var p1 = points[0];
                    var Q = p0 * 0.75f + p1 * 0.25f;
                    output.Add(Q);
                }
                else if (points.Count > 1)
                {
                    output.Add(points[points.Count - 1]);
                }

                points = output;
            }
            return points;
        }
        public static void DrawWithGizmos(this List<Vector3> points, bool close = false)
        {
            Color colA = Color.cyan;
            Color colB = Color.green;
            for (int i = 1; i < points.Count; i++)
            {
                Vector3 p0 = points[i - 1];
                Vector3 p1 = points[i];
                Gizmos.color = Color.Lerp(colA, colB, (i - 1) / (float)points.Count);
                Gizmos.DrawLine(p0, p1);
            }
            if (close && points.Count > 2)
            {
                Gizmos.DrawLine(points.First(), points.Last());
            }
        }
        public static DLine Renderer(this List<Vector3> points)
        {
            GameObject lineParent = D.TrackFirst("d_line_renderers") as GameObject;
            if (lineParent == null)
            {
                lineParent = new GameObject("D Lines");
                D.Track("d_line_renderers", lineParent);
            }
            GameObject g = new GameObject("D Line");
            g.transform.SetParent(lineParent.transform);
            DLine dline = g.AddComponent<DLine>();
            dline.Setup(points);
            return dline;
        }
        public static List<List<Vector3>> Cluster(this List<Vector3> list, int num, int iterations = 24)
        {
            List<Vector3> centres = list.AnyUnique(num);
            Dictionary<Vector3, List<Vector3>> dict = new Dictionary<Vector3, List<Vector3>>();
            //centres.ForEach(x => dict.Add(x, new List<Vector3>()));

            iterations = Mathf.Clamp(iterations, 1, 300);

            for (int n = 0; n < iterations; n++)
            {
                dict.Clear();
                centres.ForEach(x => dict.Add(x, new List<Vector3>()));
                for (int i = 0; i < list.Count; i++)
                {
                    Vector3 point = list[i];
                    Vector3? closestCentre = centres.ClosestPoint(point);
                    if (closestCentre != null)
                    {
                        dict[closestCentre.Value].Add(point);
                    }
                }
                for (int j = 0; j < centres.Count; j++)
                {
                    Vector3 centre = centres[j];
                    List<Vector3> points = dict[centre];
                    dict.Remove(centre);
                    Vector3 newCentre = points.Average();
                    dict.Add(newCentre, points);
                    centres[j] = newCentre;
                }
            }
            List<List<Vector3>> finalList = new List<List<Vector3>>();
            dict.Keys.ToList().ForEach(x => finalList.Add(dict[x]));
            return finalList;
        }


        //Vector2

        public static void DrawWithGizmos(this List<Vector2> points, bool close = false)
        {
            points.Select(x => x.XY()).ToList().DrawWithGizmos(close);
        }

        public static Vector2 Average(this List<Vector2> list)
        {
            Vector2 sum = Vector3.zero;
            list.ForEach(x => sum += x);
            sum /= (float)list.Count;
            return sum;
        }
        public static Vector2? ClosestPoint(this List<Vector2> list, Vector2 point)
        {
            Vector2? closest = null;
            float closestDistance = Mathf.Infinity;
            for (int i = 0; i < list.Count; i++)
            {
                float distance = Vector2.Distance(list[i], point);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = list[i];
                }
            }
            return closest;
        }
        public static Vector2 GetClosestPointOnPath(this List<Vector2> points, Vector2 point)
        {
            Vector2 closest = Vector2.zero;
            float minDist = float.MaxValue;
            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 p1 = points[i];
                Vector2 p2 = points[i + 1];
                Vector2 closestPoint = D.GetClosestPointOnLine(p1, p2, point);
                float dist = Vector2.Distance(point, closestPoint);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = closestPoint;
                }
            }
            return closest;
        }




        public static DLine Renderer(this List<Vector2> points)
        {
            return Renderer(points.Select(x => x.XY()).ToList());
        }
        public static float Distance(this List<Vector3> points)
        {
            float sum = 0;
            for (int i = 1; i < points.Count; i++)
            {
                float distance = Vector3.Distance(points[i - 1], points[i]);
                sum += distance;
            }
            return sum;
        }
        public static Vector3 PointAtTime(this List<Vector3> points, float t)
        {
            Vector3 point, dir;
            (point, dir) = PointAndDirectionAtTime(points, t);
            return point;
        }
        public static (Vector3, Vector3) PointAndDirectionAtTime(this List<Vector3> points, float t)
        {
            t = Mathf.Clamp01(t);

            float totalDistance = points.Distance();
            float distanceAtTime = totalDistance * t;

            float traversed = 0;
            for (int i = 1; i < points.Count; i++)
            {
                float length = Vector3.Distance(points[i - 1], points[i]);
                if (traversed + length >= distanceAtTime)// its on this length
                {
                    //traversed + x = distanceAtTime
                    float firstNodeT = traversed / totalDistance;
                    float endNodeT = (traversed + length) / totalDistance;
                    //.7 - .8 ..want .73
                    float tAcrossLength = Mathf.InverseLerp(firstNodeT, endNodeT, t);
                    Vector3 point = Vector3.Lerp(points[i - 1], points[i], tAcrossLength);
                    Vector3 dir = (points[i] - points[i - 1]).normalized;
                    return (point, dir);
                }
                traversed += length;
            }
            return (Vector3.zero, Vector3.zero);
        }










        //Arrays
        public static (bool, Vector2Int) Contains<T>(this T[,] array, T checkItem)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    T item = array[i, j];
                    if (item.GetHashCode() == checkItem.GetHashCode()) { return (true, new Vector2Int(i, j)); }
                }
            }
            return (false, Vector2Int.one * -1);
        }
        public static (bool, Vector3Int) Contains<T>(this T[,,] array, T checkItem)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    for (int k = 0; k < array.GetLength(2); k++)
                    {
                        T item = array[i, j, k];
                        if (item.GetHashCode() == checkItem.GetHashCode()) { return (true, new Vector3Int(i, j, k)); }
                    }
                }
            }
            return (false, Vector3Int.one * -1);
        }
        public static void Fill<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }
        public static void Fill<T>(this T[,] array, T value)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = value;
                }
            }
        }

        public static List<Array2DItem<T>> ToList<T>(this T[,] array)
        {

            List<Array2DItem<T>> list = new List<Array2DItem<T>>();
            if (array == null) { return list; }
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    T v = array[i, j];
                    if (v == null) { continue; }
                    Array2DItem<T> item = new Array2DItem<T>() { value = v, index = new Vector2Int(i, j) };
                    list.Add(item);
                }
            }
            return list;
        }










        //Enums
        public static T Next<T>(this T thisEnum) where T : Enum
        {
            Type t = thisEnum.GetType();
            int currentIndex = (int)Convert.ChangeType(thisEnum, typeof(int));
            int length = Enum.GetValues(t).Length;
            int nextIndex = (int)Mathf.Repeat(currentIndex + 1, length);
            return (T)Convert.ChangeType(Enum.GetValues(t).GetValue(nextIndex), t);
        }
        public static T Previous<T>(this T thisEnum) where T : Enum
        {
            Type t = thisEnum.GetType();
            int currentIndex = (int)Convert.ChangeType(thisEnum, typeof(int));
            int length = Enum.GetValues(t).Length;
            int prevIndex = (int)Mathf.Repeat(currentIndex - 1, length);
            return (T)Convert.ChangeType(Enum.GetValues(t).GetValue(prevIndex), t);
        }

        public static bool HasAnyMatchingFlags<T>(this T thisEnum, T flags) where T : Enum
        {
            foreach (T item in Enum.GetValues(thisEnum.GetType()))
            {
                if (thisEnum.HasFlag(item) && flags.HasFlag(item))
                {
                    return true;
                }
            }
            return false;
        }
        public static T Any<T>(this T thisEnum) where T : Enum
        {
            return (T)Enum.GetValues(thisEnum.GetType()).GetValue(UnityEngine.Random.Range(0, Enum.GetValues(thisEnum.GetType()).Length));
        }

    }
}