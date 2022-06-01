using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;

public class TrackMe : MonoBehaviour
{
    public string id = "";
    // Start is called before the first frame update
    void Start()
    {
        D.Track(id, gameObject);
    }


}