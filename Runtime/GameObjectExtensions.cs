using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dutil
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject g) where T : Component
        {
            T comp = g.GetComponent<T>();
            if (comp == null)
            {
                comp = g.AddComponent<T>();
            }
            return comp;
        }
        public static List<Transform> GetChildrenNested(GameObject g, bool includeSelf = false)
        {
            int index = 0;
            int maxLoop = 2000;
            int loop = 0;
            List<Transform> list = new List<Transform>();
            list.Add(g.transform);
            while (index < list.Count && loop++ < maxLoop)
            {
                Transform current = list[index];
                GetChildren(ref list, current);
                index++;
            }
            if (!includeSelf)
            {
                list.Remove(g.transform);
            }
            return list;
        }
        static void GetChildren(ref List<Transform> list, Transform parent)
        {
            int count = parent.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform child = parent.GetChild(i);
                list.AddUnique(child);
            }
        }
    }

}