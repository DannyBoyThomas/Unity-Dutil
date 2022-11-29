using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Dutil
{
    public class ScheduledTask
    {
        float delay;
        UnityAction<ScheduledTask> callback;
        float passedTime = 0;
        float delayBetweenRepeat = 0;
        int numberOfTimesToRepeat = 0;
        int callCount = 0;
        bool shouldRemove = false;
        public ScheduledTask(float _delay, UnityAction<ScheduledTask> _callback, float _repeatDelay = 0)
        {
            delay = _delay;
            callback = _callback;
            delayBetweenRepeat = _repeatDelay;
            if (delayBetweenRepeat != 0)
            {
                numberOfTimesToRepeat = -1;
            }

        }
        public virtual bool Tick()
        {
            if (shouldRemove) { return true; }
            passedTime += Time.deltaTime;
            if (passedTime >= delay)
            {
                if (callback == null) { return true; }
                try
                {
                    callback.Invoke(this);
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("[Dutil] Schedule: ".Color(Colours.Orange).B() + "Task removed from Schedule.\n" + e.Message + "\nStackTrace:\n" + e.StackTrace);
                    return true;
                }

                callCount++;
                if (numberOfTimesToRepeat > 0 || numberOfTimesToRepeat == -1)// If it should repeat
                {
                    if (numberOfTimesToRepeat > 0) { numberOfTimesToRepeat--; }
                    delay = delayBetweenRepeat;
                    passedTime -= delay;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        public ScheduledTask Wait(float seconds)
        {
            delay = seconds;
            return this;
        }
        public ScheduledTask Repeat(float _delay, int _count = -1)
        {
            delayBetweenRepeat = _delay;
            numberOfTimesToRepeat = _count;
            return this;
        }
        public ScheduledTask SetCallback(UnityAction<ScheduledTask> _callback)
        {
            callback = _callback;
            return this;
        }
        public ScheduledTask Cancel()
        {
            shouldRemove = true;
            return this;
        }
        public int CallCount { get { return callCount; } }



    }
}
