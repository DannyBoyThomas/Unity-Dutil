using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Dutil
{
    public static class ListExtensions
    {

        public static List<T> Fill<T>(this List<T> list, int count, T item) => Enumerable.Repeat(item, count).ToList();
        public static void Print<T>(this List<T> list)
        {
            if (list.Count <= 0) { Debug.Log("Empty"); return; }
            list.ForEach(x => Debug.Log(x));
        }
        public static T Any<T>(this List<T> list)
        {
            int index = Random.Range(0, list.Count);
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
            int newIndex = index > 0 ? index : max + index - 1;
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







        //Vector3
        public static Vector3 Average(this List<Vector3> list)
        {
            Vector3 sum = Vector3.zero;
            list.ForEach(x => sum += x);
            sum /= list.Count;
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
        public static void Draw(this List<Vector3> points, bool close = false)
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
        public static void Draw(this List<Vector2> points, bool close = false)
        {
            points.Select(x => x.XY()).ToList().Draw(close);
        }

        public static Vector2 Average(this List<Vector2> list)
        {
            Vector2 sum = Vector3.zero;
            list.ForEach(x => sum += x);
            sum /= list.Count;
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
    }
}