using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using UnityEngine.UI;
using UnityEngine.Events;
[RequireComponent(typeof(Canvas))]
public class DUI_Canvas : DUI_Rect
{
    Vector2 lastSize;
    public bool autoRefresh = true;
    [ConditionalShowProperty("autoRefresh")]
    public float autoRefreshDelay = 2.5f;
    ScheduledTask autoRefreshTask;
    //public UnityEvent<Vector2, Vector2> OnSizeChangedEvent = new UnityEvent<Vector2, Vector2>();

    public override void PostStart()
    {
        if (Application.isPlaying)
        {
            if (autoRefresh)
            {
                autoRefreshTask = Schedule.Add(autoRefreshDelay, (t) => { InformChildrenOfChange(); }, autoRefreshDelay);
            }
        }
        lastSize = Size();
    }


    // Update is called once per frame
    void Update()
    {
        CheckForSizeChange();
    }
    void CheckForSizeChange()
    {
        Vector2 size = Size();
        if (size != lastSize)
        {
            InformChildrenOfChange();
            lastSize = size;
        }
    }
    void OnDestroy()
    {
        autoRefreshTask?.Cancel();
    }

    /*   public Vector2 Size()
      {
          Vector2 size = new Vector2(canvas.pixelRect.width, canvas.pixelRect.height);
          return size;
      } */
}