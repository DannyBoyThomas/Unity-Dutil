using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
namespace Dutil
{


    public class TrackMe : MonoBehaviour
    {
        public string id = "";
        // Start is called before the first frame update
        void Awake()
        {
            D.Track(id, gameObject);
        }
    }
}