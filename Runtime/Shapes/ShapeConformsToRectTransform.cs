using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
#if SHAPES_INSTALLED
using Shapes;
[ExecuteInEditMode]
public class ShapeConformsToRectTransform : MonoBehaviour
{

    void Update()
    {
        if (RT?.hasChanged ?? false)
        {
            if (Rectangle != null)
                Run();

        }
    }
    RectTransform rt;
    RectTransform RT
    {
        get { if (rt == null) rt = GetComponent<RectTransform>(); return rt; }
    }
    Rectangle rectangle;
    Rectangle Rectangle
    {
        get { if (rectangle == null) rectangle = GetComponent<Rectangle>(); return rectangle; }
    }
    void Run()
    {
        Rectangle.Width = RT.rect.width;
        Rectangle.Height = RT.rect.height;
        Rectangle.transform.position = RT.position;
    }
}
#endif