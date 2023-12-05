using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;

public class CustomHierarchyItem : MonoBehaviour
{
    public Color textColor, backgroundColor;
    public bool isDivider = false;
    public Font font;

    void Reset()
    {
        backgroundColor = Colours.Hex("#EC6F3E");
        textColor = Colours.Hex("#353535");
    }
}