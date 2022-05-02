using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Dutil;
public class AutoLerp : MonoBehaviour
{
    public static AutoLerp Instance;
    public List<AutoLerpTask> tasks = new List<AutoLerpTask>();
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
            AutoLerpTask task = tasks[i];
            if (task.Tick())
            {
                tasks.RemoveAt(i);
            }
        }

    }
    public static void CreateInstance()
    {
        if (Instance != null) { return; }
        GameObject obj = (GameObject)D.TrackFirst("dutil_autolerp");//GameObject.FindGameObjectWithTag("Util");
        if (obj != null)
        {
            AutoLerp comp = obj.GetComponent<AutoLerp>();
            if (comp != null)
            {
                Instance = comp;
                return;
            }
        }
        GameObject newObj = new GameObject("AutoLerper");
        D.Track("dutil_autolerp", newObj);
        Instance = newObj.AddComponent<AutoLerp>();
    }
    public static AutoLerpTask Begin(AutoLerpTask task)
    {
        CreateInstance();
        Instance.tasks.Add(task);
        return task;
    }
    public static AutoLerpTask Begin(float duration, UnityAction<AutoLerpTask, float> callback, bool ease = false, bool useUnscaled = false)
    {
        CreateInstance();
        AutoLerpTask task = new AutoLerpTask(duration, callback, ease).UseUnscaledDeltaTime(useUnscaled);
        Instance.tasks.Add(task);
        return task;
    }
    public static AutoLerpTask Begin(float duration, float start, float end, UnityAction<AutoLerpTask, float> callback, bool ease = false, bool useUnscaled = false)
    {
        CreateInstance();
        AutoLerpTask task = new AutoLerpTask(duration, start, end, callback, ease).UseUnscaledDeltaTime(useUnscaled);
        Instance.tasks.Add(task);
        return task;
    }
    public static AutoLerpTask Begin(float duration, int start, int end, UnityAction<AutoLerpTask, int> callback, bool ease = false, bool useUnscaled = false)
    {
        CreateInstance();
        AutoLerpTask task = new AutoLerpTask(duration, start, end, callback, ease).UseUnscaledDeltaTime(useUnscaled);
        Instance.tasks.Add(task);
        return task;
    }
    public static AutoLerpTask Begin(float duration, Vector2 start, Vector2 end, UnityAction<AutoLerpTask, Vector2> callback, bool ease = false, bool useUnscaled = false)
    {
        CreateInstance();
        AutoLerpTask task = new AutoLerpTask(duration, start, end, callback, ease).UseUnscaledDeltaTime(useUnscaled);
        Instance.tasks.Add(task);
        return task;
    }
    public static AutoLerpTask Begin(float duration, Vector3 start, Vector3 end, UnityAction<AutoLerpTask, Vector3> callback, bool ease = false, bool useUnscaled = false)
    {
        CreateInstance();
        AutoLerpTask task = new AutoLerpTask(duration, start, end, callback, ease).UseUnscaledDeltaTime(useUnscaled);
        Instance.tasks.Add(task);
        return task;
    }
    public static AutoLerpTask Begin(float duration, Quaternion start, Quaternion end, UnityAction<AutoLerpTask, Quaternion> callback, bool ease = false, bool useUnscaled = false)
    {
        CreateInstance();
        AutoLerpTask task = new AutoLerpTask(duration, start, end, callback, ease).UseUnscaledDeltaTime(useUnscaled);
        Instance.tasks.Add(task);
        return task;
    }
    public static AutoLerpTask Begin(float duration, Color start, Color end, UnityAction<AutoLerpTask, Color> callback, bool ease = false, bool useUnscaled = false)
    {
        CreateInstance();
        AutoLerpTask task = new AutoLerpTask(duration, start, end, callback, ease).UseUnscaledDeltaTime(useUnscaled);
        Instance.tasks.Add(task);
        return task;
    }

}
