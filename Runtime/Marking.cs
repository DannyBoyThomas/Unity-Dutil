using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dutil
{


    public class Marking
    {
        public static Dictionary<Object, List<string>> marks = new Dictionary<Object, List<string>>();
        public static bool AddMark(Object obj, string mark)
        {
            if (!IsValidMark(mark)) { Debug.Log("Invalid Mark"); return false; }
            if (!marks.ContainsKey(obj))
            {
                marks.Add(obj, new List<string>());
            }
            marks[obj].Add(mark);
            return true;
        }
        public static void RemoveMark(Object obj, string mark)
        {
            if (!marks.ContainsKey(obj))
            {
                return;
            }
            marks[obj].Remove(mark);
        }
        public static bool HasMark(Object obj, string mark)
        {
            if (!marks.ContainsKey(obj))
            {
                return false;
            }
            return marks[obj].Contains(mark);
        }
        public static void ClearMarks(Object obj)
        {
            if (!marks.ContainsKey(obj))
            {
                return;
            }
            marks[obj].Clear();
        }
        public static void ClearAllMarks()
        {
            marks.Clear();
        }
        public static bool IsValidMark(string mark)
        {
            return mark != null && mark.Length > 0;
        }

    }
}