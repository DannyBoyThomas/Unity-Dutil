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
        public static void AddUnique<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }
        public static T Get<T>(this List<T> list, int index)
        {
            int max = list.Count;
            int newIndex = index > 0 ? index : max + index - 1;
            return list[newIndex];
        }

        //Vector3
        public static Vector3 Centre(this List<Vector3> list)
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

        public static Vector2 Centre(this List<Vector2> list)
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