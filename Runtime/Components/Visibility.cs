using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dutil
{


    public class Visibility : MonoBehaviour
    {
        public enum VisibilityType { Show, Hide }
        public VisibilityType visibility;
        void Awake()
        {
            bool active = visibility == VisibilityType.Show;
            gameObject.SetActive(active);
        }
    }
}