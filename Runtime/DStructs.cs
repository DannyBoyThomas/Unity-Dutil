using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dutil
{
    public class DStructs
    {

    }
    public struct ArrayItem<T>
    {
        public int index;
        public T value;
        public void Print()
        {
            Debug.Log(index + ": " + value);
        }
    }
    public struct Array2DItem<T>
    {
        public Vector2Int index;
        public T value;
        public void Print()
        {
            Debug.Log(index + ": " + value);
        }
    }

}