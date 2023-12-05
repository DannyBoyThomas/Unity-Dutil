using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using System;

public class HierarchyItem : Attribute
{
    public Color Color;
    public Color TextColor;
    public HierarchyItem(string color)
    {
        Color = Colours.Hex(color);
        TextColor = Color.Opposite();
    }
}