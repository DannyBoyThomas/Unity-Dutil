using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
#if SHAPES_INSTALLED
using Shapes;
[ExecuteInEditMode]
public class ShapeConformsToRectTransform : MonoBehaviour
{
    public Vector2 padding = Vector2.zero;
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.ShowIf("Disc")]
#endif

    public float radiusOffset = 0;
    void Update()
    {
        if (RT?.hasChanged ?? false)
        {
            UpdateRectangle();
            UpdateDisc();
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

    Disc disc;
    Disc Disc
    {
        get { if (disc == null) disc = GetComponent<Disc>(); return disc; }
    }
    void UpdateRectangle()
    {
        if (Rectangle == null) { return; }
        Rectangle.Width = RT.rect.width - padding.x * 2;
        Rectangle.Height = RT.rect.height - padding.y * 2;
        Rectangle.transform.position = RT.position;


    }
    void UpdateDisc()
    {
        if (Disc == null) { return; }
        Disc.Radius = (Mathf.Min(RT.rect.width, RT.rect.height) / 2) + radiusOffset;
    }
}
#endif