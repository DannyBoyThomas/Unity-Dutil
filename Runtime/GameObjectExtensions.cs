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
        public static List<Transform> Children(this Transform t)
        {
            List<Transform> children = new List<Transform>();
            int count = t.childCount;
            for (int i = 0; i < count; i++)
            {
                children.Add(t.GetChild(i));
            }
            return children;
        }
        public static bool IsInLayer(this GameObject g, LayerMask layer)
        {
            return ((layer.value & (1 << g.layer)) > 0);
        }

        //DEVELOP
        //gameobject
        public static DevelopTask DevelopPosition(this GameObject g, Vector3 position, float duration, bool ease = false)
        {
            return DevelopPosition(g.transform, position, duration, ease);
        }
        public static DevelopTask DevelopRotation(this GameObject g, Quaternion rotation, float duration, bool ease = false)
        {
            return DevelopRotation(g.transform, rotation, duration, ease);
        }
        public static DevelopTask DevelopRotation(this GameObject g, Vector3 rotation, float duration, bool ease = false)
        {
            return DevelopRotation(g.transform, rotation, duration, ease);
        }
        public static DevelopTask DevelopScale(this GameObject g, Vector3 scale, float duration, bool ease = false)
        {
            return DevelopScale(g.transform, scale, duration, ease);
        }

        //Transforms
        public static DevelopTask DevelopPosition(this Transform t, Vector3 position, float duration, bool ease = false)
        {
            return Develop.Begin(duration, t.position, position, (task, value) =>
              {
                  t.position = value;
              }, ease);
        }
        public static DevelopTask DevelopRotation(this Transform t, Quaternion rotation, float duration, bool ease = false)
        {
            return Develop.Begin(duration, t.rotation, rotation, (task, value) =>
            {
                t.rotation = value;
            }, ease);
        }
        public static DevelopTask DevelopRotation(this Transform t, Vector3 rotation, float duration, bool ease = false)
        {
            return Develop.Begin(duration, t.eulerAngles, rotation, (task, value) =>
            {
                t.eulerAngles = value;
            }, ease);
        }
        public static DevelopTask DevelopScale(this Transform t, Vector3 scale, float duration, bool ease = false)
        {
            return Develop.Begin(duration, t.localScale, scale, (task, value) =>
            {
                t.localScale = value;
            }, ease);
        }





    }
}
