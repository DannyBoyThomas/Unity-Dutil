using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;

public class UiPage : MonoBehaviour
{
    UiPageManager manager;
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button]
#endif
    public void Focus()
    {
        Manager.Focus(this);
    }
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button]
#endif
    public void Navigate()
    {
        Manager.Navigate(this);
    }
    UiPageManager Manager
    {
        get
        {
            if (manager == null)
                manager = GetComponentInParent<UiPageManager>();
            return manager;
        }
    }
}