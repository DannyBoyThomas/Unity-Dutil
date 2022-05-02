using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dutil
{
    [System.Serializable]
    public class CooldownStack : Cooldown
    {
        [SerializeField]
        public int stackSize, maxStackSize;
        public CooldownStack(int currentStackSize, int _maxStackSize, float _waitDuration, bool _startActive = true) : base(_waitDuration, _startActive)
        {
            maxStackSize = _maxStackSize;
            stackSize = Mathf.Clamp(currentStackSize, 0, maxStackSize);
        }
        internal override void OnCooldownComplete()
        {
            coolDownTask = null;
            stackSize = Mathf.Clamp(stackSize + 1, 0, maxStackSize);
            isReady = true;
            if (stackSize < maxStackSize)
            {
                BeginCooldown();
            }
            base.OnCooldownComplete();
        }
        internal override void Start()
        {
            Debug.Log(this);
            if (stackSize < maxStackSize)
            {
                BeginCooldown();
            }

        }
        public override bool Use()
        {
            if (stackSize > 0)
            {
                stackSize--;
                if (stackSize <= 0)
                {
                    isReady = false;
                }

                BeginCooldown();
                return true;
            }

            return false;
        }

    }
}