using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
namespace Dutil
{
    public class LiveRendererColour : MonoBehaviour
    {
        public Color color;
        Material material;

        void OnValidate()
        {
            Color = color;
        }
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                Material mat = Material;
                if (mat != null) { mat.color = color; }
            }
        }
        public Material Material
        {
            get
            {
                if (material == null)
                {
                    material = GetComponent<Renderer>()?.material;
                }
                return material;
            }
        }
    }
}