using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Develop : MonoBehaviour
{
    public static Develop Instance;
    public List<DevelopTask> tasks = new List<DevelopTask>();
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
            DevelopTask task = tasks[i];
            if (task.Tick())
            {
                tasks.RemoveAt(i);
            }
        }

    }
    public static void CreateInstance()
    {
        if (Instance != null) { return; }
        GameObject obj = GameObject.FindGameObjectWithTag("Util");
        if (obj != null)
        {
            Develop comp = obj.GetComponent<Develop>();
            if (comp != null)
            {
                Instance = comp;
                return;
            }
        }
        GameObject newObj = new GameObject("Develop System");
        newObj.tag = "Util";
        Instance = newObj.AddComponent<Develop>();
    }
    public static DevelopTask Begin(DevelopTask task)
    {
        CreateInstance();
        Instance.tasks.Add(task);
        return task;
    }
    public static DevelopTask Begin(float duration, UnityAction<DevelopTask, float> callback, bool ease = false)
    {
        CreateInstance();
        DevelopTask task = new DevelopTask(duration, callback, ease);
        Instance.tasks.Add(task);
        return task;
    }
    public static DevelopTask Begin(float duration, float start, float end, UnityAction<DevelopTask, float> callback, bool ease = false)
    {
        CreateInstance();
        DevelopTask task = new DevelopTask(duration, start, end, callback, ease);
        Instance.tasks.Add(task);
        return task;
    }
    public static DevelopTask Begin(float duration, int start, int end, UnityAction<DevelopTask, int> callback, bool ease = false)
    {
        CreateInstance();
        DevelopTask task = new DevelopTask(duration, start, end, callback, ease);
        Instance.tasks.Add(task);
        return task;
    }
    public static DevelopTask Begin(float duration, Vector2 start, Vector2 end, UnityAction<DevelopTask, Vector2> callback, bool ease = false)
    {
        CreateInstance();
        DevelopTask task = new DevelopTask(duration, start, end, callback, ease);
        Instance.tasks.Add(task);
        return task;
    }
    public static DevelopTask Begin(float duration, Vector3 start, Vector3 end, UnityAction<DevelopTask, Vector3> callback, bool ease = false)
    {
        CreateInstance();
        DevelopTask task = new DevelopTask(duration, start, end, callback, ease);
        Instance.tasks.Add(task);
        return task;
    }
    public static DevelopTask Begin(float duration, Quaternion start, Quaternion end, UnityAction<DevelopTask, Quaternion> callback, bool ease = false)
    {
        CreateInstance();
        DevelopTask task = new DevelopTask(duration, start, end, callback, ease);
        Instance.tasks.Add(task);
        return task;
    }
    public static DevelopTask Begin(float duration, Color start, Color end, UnityAction<DevelopTask, Color> callback, bool ease = false)
    {
        CreateInstance();
        DevelopTask task = new DevelopTask(duration, start, end, callback, ease);
        Instance.tasks.Add(task);
        return task;
    }

}
