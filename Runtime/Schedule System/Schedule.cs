using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Dutil
{
    public class Schedule : MonoBehaviour
    {
        public static Schedule Instance;
        public List<ScheduledTask> tasks = new List<ScheduledTask>();
        void Start()
        {
            CreateInstance();
        }

        // Update is called once per frame
        void Update()
        {
            //CreateInstance();

            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                ScheduledTask task = tasks[i];
                if (task.Tick())
                {
                    tasks.RemoveAt(i);
                }
            }

        }

        public static ScheduledTask Add(float wait, UnityAction<ScheduledTask> callback, float repeat = 0)
        {
            CreateInstance();
            ScheduledTask task = new ScheduledTask(wait, callback, repeat);
            Instance.tasks.Add(task);
            return task;
        }
        public static ScheduledTask Add(ScheduledTask task)
        {
            CreateInstance();
            Instance.tasks.Add(task);
            return task;
        }


        public static void CreateInstance()
        {
            if (Instance != null) { return; }
            GameObject obj = (GameObject)D.TrackFirst("dutil_scheduler");////GameObject.FindGameObjectWithTag("Util");
            if (obj != null)
            {
                Schedule comp = obj.GetComponent<Schedule>();
                if (comp != null)
                {
                    Instance = comp;
                    return;
                }
            }
            GameObject newObj = new GameObject("Scheduler");
            D.Track("dutil_scheduler", newObj);
            //newObj.tag = "Util";
            Instance = newObj.AddComponent<Schedule>();
        }
    }


}




