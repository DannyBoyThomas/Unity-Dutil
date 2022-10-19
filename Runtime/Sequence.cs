using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using UnityEngine.Events;
using System.Linq;
public class Sequence : MonoBehaviour
{
    public static Sequence Instance;
    public static List<SequenceTask> tasks = new List<SequenceTask>();
    void Start()
    {
        CreateInstance();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = tasks.Count - 1; i >= 0; i--)
        {
            SequenceTask task = tasks[i];
            if (task.Tick())
            {
                tasks.RemoveAt(i);
            }
        }

    }
    public static void CreateInstance()
    {
        if (Instance != null) { return; }
        GameObject obj = (GameObject)D.TrackFirst("dutil_sequencer");////GameObject.FindGameObjectWithTag("Util");
        if (obj != null)
        {
            Sequence comp = obj.GetComponent<Sequence>();
            if (comp != null)
            {
                Instance = comp;
                return;
            }
        }
        GameObject newObj = new GameObject("Sequencer");
        D.Track("dutil_sequencer", newObj);
        Instance = newObj.AddComponent<Sequence>();
    }
    public static SequenceTask Create(params SequenceAction[] actions)
    {
        SequenceTask task = new SequenceTask(actions.ToList());
        return task;
    }

    public static void Run(SequenceTask task)
    {
        CreateInstance();
        tasks.AddUnique(task);
    }



}
public class SequenceTask
{
    float targetTime;
    float delay;
    bool cancelled = false;
    List<SequenceAction> actions;
    public SequenceTask(List<SequenceAction> actions)
    {
        this.actions = actions ?? new List<SequenceAction>();
        delay = 0;
        if (actions.Count > 0)
        {
            targetTime = actions[0].wait;
        }
        else
        {
            targetTime = 0;
        }

    }
    public void Run()
    {
        Sequence.Run(this);
    }
    public bool Tick()
    {
        if (cancelled) { return true; }

        delay += Time.deltaTime;
        if (delay >= targetTime)
        {
            delay -= targetTime;
            if (actions == null || actions.Count == 0) { return true; }
            try
            {
                actions.Pop().action?.Invoke();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[Dutil] Sequence: ".Color(Colours.Orange).B() + "Task removed from Sequence.\n" + e.Message);
                return true;
            }

            if (actions.Count <= 0)
            {
                return true;
            }
            else
            {
                targetTime = actions.First().wait;
            }
        }
        return false;
    }
    public void Cancel()
    {
        cancelled = true;
    }
}
public struct SequenceAction
{
    public UnityAction action;
    public float wait;

    public SequenceAction(UnityAction action, float wait = 0.1f)
    {
        this.action = action;
        this.wait = wait;
    }
}