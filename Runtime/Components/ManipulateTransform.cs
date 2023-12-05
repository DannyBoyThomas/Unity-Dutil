using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;

public class ManipulateTransform : MonoBehaviour
{
    //public enum Space { World, Local }

    public Vector3 translate;
    public Space translateSpace = Space.World;
    public Vector3 rotate;
    public Space rotateSpace = Space.World;
    public Vector3 scale = Vector3.zero;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(translate * Time.deltaTime, translateSpace);
        transform.Rotate(rotate * Time.deltaTime, rotateSpace);

        transform.localScale += scale * Time.deltaTime;
    }
}