using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dutil
{

    [System.Serializable]
    public class Cooldown
    {
        static List<Cooldown> cooldowns = new List<Cooldown>();
        public float totalWaitDuration;
        public float currentWaitDuration = 0;
        public bool isReady = false;
        internal AutoLerpTask coolDownTask;
        public Cooldown(float waitDuration, bool startActive = true)
        {
            totalWaitDuration = waitDuration;
            isReady = startActive;
            cooldowns.AddUnique(this);

        }
        [RuntimeInitializeOnLoadMethod]
        static void OnInit()
        {
            cooldowns.Clean();
            cooldowns.ForEach(x => x.Start());
        }
        internal virtual void Start()
        {
            Debug.Log(this);
            if (isReady)
            {
                currentWaitDuration = totalWaitDuration;
            }
            else
            {
                BeginCooldown();

            }
        }

        [ContextMenu("Use")]
        public virtual bool Use()
        {
            if (isReady || WaitCompletion() >= 1f)
            {
                isReady = false;

                BeginCooldown();
                return true;
            }

            return false;
        }

        internal void BeginCooldown()
        {
            if (coolDownTask != null) { return; }
            currentWaitDuration = 0;

            coolDownTask = AutoLerp.Begin(totalWaitDuration, (task, value) =>
                          {
                              currentWaitDuration = value * totalWaitDuration;
                              if (currentWaitDuration >= totalWaitDuration)
                              {
                                  OnCooldownComplete();
                              }
                          });
        }
        internal virtual void OnCooldownComplete()
        {
            isReady = true;
        }
        /// <summary>
        /// Returning the percentage of time passed since the last use of the cooldown.
        /// </summary>
        /// <returns></returns>
        public float WaitCompletion()
        {
            return currentWaitDuration / totalWaitDuration;
        }

    }
}
