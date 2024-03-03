using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;

using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;



[InitializeOnLoad]
[ExecuteAlways]
#endif
public class Productivity : MonoBehaviour
{
#if UNITY_EDITOR
    public static string Prod_Timestammp = "dutil/productivity_timestamp";
    public static string Prod_Seconds = "dutil/productivity_seconds";
    public int days, hours, minutes, seconds;
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.FoldoutGroup("Adjust")]
#endif
    public int _days, _hours, _minutes, _seconds;
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button]
    [Sirenix.OdinInspector.FoldoutGroup("Adjust")]
#endif


    [ContextMenu("Adjust")]
    public void Adjust()
    {
        int totalSeconds = GetSeconds();
        totalSeconds += (_days * 86400) + (_hours * 3600) + (_minutes * 60) + _seconds;
        totalSeconds = Mathf.Max(0, totalSeconds);
        Drive.Save(totalSeconds, Prod_Seconds);
        _days = _hours = _minutes = _seconds = 0;
        Calculate();
    }

    static Productivity()
    {
        EditorApplication.update += BgUPdate;
        EditorApplication.playModeStateChanged += PlayModeStateChanged;
        EditorApplication.wantsToQuit += OnAttemptingQuit;
    }
    static void PlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            Calculate();
        }
    }

    static void BgUPdate()
    {
        if (D.Chance(.005f))
        {
            Calculate();
        }
    }
    static bool OnAttemptingQuit()
    {
        Debug.Log("Quitting");
        Calculate();
        Drive.Remove(Prod_Timestammp);
        return true;
    }
    void Update()
    {
        //update localdata
        //update ui
        int totalSeconds = GetSeconds();
        days = totalSeconds / 86400;
        hours = (totalSeconds % 86400) / 3600;
        minutes = (totalSeconds % 3600) / 60;
        seconds = totalSeconds % 60;

    }
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button]
#endif
    [ContextMenu("Refresh")]
    public void Refresh()
    {
        SetTimestamp();

    }

    public static void Display()
    {
        int totalSeconds = GetSeconds();
        int days = totalSeconds / 86400;
        int hours = (totalSeconds % 86400) / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;
        bool showDays = days > 0;
        int allHours = (days * 24) + hours;
        string time = showDays ? $"{days} days, {hours} hours, {minutes} minutes, {seconds} seconds" : $"{hours} hours, {minutes} minutes, {seconds} seconds. [{allHours} Hours]";
        Debug.Log("Productivity: " + time);
    }
    static void Calculate()
    {
        int seconds = GetSecondsSinceLastTimestamp();
        AddSeconds(seconds);
        SetTimestamp();
    }
    static int GetSeconds()
    {
        int steps = Drive.Load<int>(Prod_Seconds, 0);
        return steps;
    }
    static void AddSeconds(int steps)
    {
        int prev = GetSeconds();
        int total = prev + steps;
        Drive.Save(total, Prod_Seconds);
    }
    public static void SetTimestamp()
    {
        string time = System.DateTime.Now.ToString();
        Drive.Save(time, Prod_Timestammp);
    }
    static string GetTimestamp()
    {
        string time = Drive.Load<string>(Prod_Timestammp, System.DateTime.Now.ToString());
        return time;
    }
    static int GetSecondsSinceLastTimestamp()
    {
        string lastTimestamp = GetTimestamp();
        System.DateTime last = System.DateTime.Parse(lastTimestamp);
        System.DateTime now = System.DateTime.Now;
        System.TimeSpan span = now - last;
        int seconds = (int)span.TotalSeconds;
        return seconds;

    }

#endif
}