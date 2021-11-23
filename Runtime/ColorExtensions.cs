using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dutil
{
    public static class ColorExtensions
    {

        public static Color Pastel(this Color color)
        {
            float newSaturation = 0.45f;
            float newValue = .75f;
            Vector3 hsv = color.HSV();
            return Color.HSVToRGB(hsv.x, newSaturation, newValue);
        }
        public static Vector3 HSV(this Color color)
        {
            float h = 0, s = 0, v = 0;
            Color.RGBToHSV(color, out h, out s, out v);
            return new Vector3(h, s, v);
        }
        public static List<Color> Split(this Color color, int num)
        {
            List<Color> list = new List<Color>();
            Vector3 hsv = color.HSV();
            float incr = 1 / (float)num;
            for (int i = 0; i < num; i++)
            {
                float offset = incr * i;
                float newH = (hsv.x + offset) % 1;
                list.Add(Color.HSVToRGB(newH, hsv.y, hsv.z));
            }
            return list;
        }
        public static Color Opposite(this Color color)
        {
            return color.Split(2)[1];
        }
    }
}