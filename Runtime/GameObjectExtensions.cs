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
        /// <summary>
        /// It destroys the object, regardless of whether the application is running or not.
        /// </summary>
        /// <param name="Object">The object you want to destroy.</param>
        public static void EasyDestroy(this Object g)
        {
            if (g == null) { return; }
            if (Application.IsPlaying(g))
            {
                GameObject.Destroy(g);
            }
            else
            {
                GameObject.DestroyImmediate(g);
            }
        }
        /// <summary>
        /// Returns all children...and their children...and their children...
        /// </summary>
        /// <param name="GameObject">The gameobject you want to get the children of.</param>
        /// <param name="includeSelf">If true, the GameObject passed in will be included in the
        /// list.</param>
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
        /// <summary>
        /// It returns a list of all the children of a transform.
        /// </summary>
        /// <param name="Transform"></param>
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
        public static void KillChildren(this Transform t)
        {
            int count = t.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform child = t.GetChild(i);
                GameObject.Destroy(child.gameObject);
            }
        }
        /// <summary>
        /// Searches through all nested children to find a child with the given name.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform FindChildWithName(this Transform t, string name)
        {
            List<Transform> list = GetChildrenNested(t.gameObject);
            foreach (Transform child in list)
            {
                if (child.name == name)
                {
                    return child;
                }
            }
            return null;
        }
        public static bool IsInLayer(this GameObject g, LayerMask layer)
        {
            return ((layer.value & (1 << g.layer)) > 0);
        }
        public static Vector3 Pos(this GameObject g)
        {
            return g.transform.position;
        }
        //Transform Quick Change
        public static void SetPosition(this Transform t, Vector3 pos)
        {
            t.position = pos;
        }
        public static Vector3 Move(this Transform t, Vector3 delta)
        {
            t.position += delta;
            return t.position;
        }
        public static Vector3 MoveLocal(this Transform t, Vector3 delta)
        {
            t.localPosition += delta;
            return t.position;
        }
        public static void MovePosX(this Transform t, float x)
        {
            t.position = new Vector3(t.position.x + x, t.position.y, t.position.z);
        }
        public static void MovePosY(this Transform t, float y)
        {
            t.position = new Vector3(t.position.x, t.position.y + y, t.position.z);
        }
        public static void MovePosZ(this Transform t, float z)
        {
            t.position = new Vector3(t.position.x, t.position.y, t.position.z + z);
        }
        public static void MovePos(this Transform t, float x, float y, float z)
        {
            t.position = new Vector3(t.position.x + x, t.position.y + y, t.position.z + z);
        }


        public static void SetPosX(this Transform t, float x)
        {
            t.position = new Vector3(x, t.position.y, t.position.z);
        }
        public static void SetPosY(this Transform t, float y)
        {
            t.position = new Vector3(t.position.x, y, t.position.z);
        }
        public static void SetPosZ(this Transform t, float z)
        {
            t.position = new Vector3(t.position.x, t.position.y, z);
        }
        public static void ResetLocally(this Transform t)
        {
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
        }

        //DEVELOP
        //gameobject
        public static AutoLerpTask AutoLerpPosition(this GameObject g, Vector3 position, float duration, bool ease = false, bool useUnscaled = false)
        {
            return AutoLerpPosition(g.transform, position, duration, ease, useUnscaled).LinkObject(g);
        }
        public static AutoLerpTask AutoLerpRotation(this GameObject g, Quaternion rotation, float duration, bool ease = false, bool useUnscaled = false)
        {
            return AutoLerpRotation(g.transform, rotation, duration, ease, useUnscaled).LinkObject(g);
        }
        public static AutoLerpTask AutoLerpRotation(this GameObject g, Vector3 rotation, float duration, bool ease = false, bool useUnscaled = false)
        {
            return AutoLerpRotation(g.transform, rotation, duration, ease, useUnscaled).LinkObject(g);
        }
        public static AutoLerpTask AutoLerpScale(this GameObject g, Vector3 scale, float duration, bool ease = false, bool useUnscaled = false)
        {
            return AutoLerpScale(g.transform, scale, duration, ease, useUnscaled).LinkObject(g);
        }

        //Transforms
        public static AutoLerpTask AutoLerpPosition(this Transform t, Vector3 position, float duration, bool ease = false, bool useUnscaled = false)
        {
            if (t.HasMark("d_lerping_position"))
            {
                Debug.LogWarning("Already lerping position");
                return null;
            }
            t.Mark("d_lerping_position");
            Schedule.Add(duration, (e) =>
            {
                t.Unmark("d_lerping_position");
            });
            return AutoLerp.Begin(duration, t.position, position, (task, value) =>
              {
                  if (t == null) { task.Cancel(); return; }
                  t.position = value;
              }, ease, useUnscaled);
        }
        public static AutoLerpTask AutoLerpRotation(this Transform t, Quaternion rotation, float duration, bool ease = false, bool useUnscaled = false)
        {
            if (t.HasMark("d_lerping_rotation"))
            {
                Debug.LogWarning("Already lerping rotation");
                return null;
            }
            t.Mark("d_lerping_rotation");
            Schedule.Add(duration, (e) =>
            {
                t.Unmark("d_lerping_rotation");
            });
            return AutoLerp.Begin(duration, t.rotation, rotation, (task, value) =>
            {
                if (t == null) { task.Cancel(); return; }
                t.rotation = value;
            }, ease, useUnscaled);
        }
        public static AutoLerpTask AutoLerpRotation(this Transform t, Vector3 rotation, float duration, bool ease = false, bool useUnscaled = false)
        {
            if (t.HasMark("d_lerping_rotation"))
            {
                Debug.LogWarning("Already lerping rotation");
                return null;
            }
            t.Mark("d_lerping_rotation");
            Schedule.Add(duration, (e) =>
            {
                t.Unmark("d_lerping_rotation");
            });

            return AutoLerp.Begin(duration, t.eulerAngles, rotation, (task, value) =>
            {
                if (t == null) { task.Cancel(); return; }
                t.eulerAngles = value;
            }, ease, useUnscaled);
        }
        public static AutoLerpTask AutoLerpScale(this Transform t, Vector3 scale, float duration, bool ease = false, bool useUnscaled = false)
        {
            if (t.HasMark("d_lerping_scale"))
            {
                Debug.LogWarning("Already lerping scale");
                return null;
            }
            t.Mark("d_lerping_scale");
            Schedule.Add(duration, (e) =>
            {
                t.Unmark("d_lerping_scale");
            });
            return AutoLerp.Begin(duration, t.localScale, scale, (task, value) =>
            {
                if (t == null) { task.Cancel(); return; }
                t.localScale = value;
            }, ease, useUnscaled);
        }

        //MARKS
        public static void Mark(this Object g, string mark)
        {
            Marking.AddMark(g, mark);
        }
        public static void Unmark(this Object g, string mark)
        {
            Marking.RemoveMark(g, mark);
        }
        public static bool HasMark(this Object g, string mark)
        {
            return Marking.HasMark(g, mark);
        }
        public static void ClearMarks(this Object g)
        {
            Marking.ClearMarks(g);
        }


        //DEATH
        public static void Kill(this GameObject g, float speed = 1, DeathType deathType = DeathType.ScaleDown)
        {
            if (g.HasMark("d_killing"))
            {
                Debug.LogWarning("GameObject is already being killed");
                return;
            }
            g.Mark("d_killing");
            switch (deathType)
            {
                case DeathType.ScaleDown:
                    g.AutoLerpScale(Vector3.zero, speed).OnComplete((x) =>
                    {
                        GameObject.Destroy(g);
                    });
                    break;
                case DeathType.ScaleDownX:
                    g.AutoLerpScale(Vector3.one.WithX(0), speed).OnComplete((x) =>
                   {
                       GameObject.Destroy(g);
                   });
                    break;
                case DeathType.ScaleDownY:
                    g.AutoLerpScale(Vector3.one.WithY(0), speed).OnComplete((x) =>
                    {
                        GameObject.Destroy(g);
                    });
                    break;
                case DeathType.ScaleDownZ:
                    g.AutoLerpScale(Vector3.one.WithZ(0), speed).OnComplete((x) =>
                    {
                        GameObject.Destroy(g);
                    });
                    break;
            }
        }



    }
}
