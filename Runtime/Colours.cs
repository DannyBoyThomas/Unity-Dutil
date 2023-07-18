using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif
public static class Colours
{
    public static Color TEst = new Color(0, 0, 0, 0);
    public static Color Red { get { return new Color(.95f, .26f, .21f); } }
    public static Color Pink { get { return new Color(.91f, .11f, .39f); } }
    public static Color Purple { get { return new Color(.61f, .15f, .69f); } }
    public static Color DeepPurple { get { return new Color(.4f, .22f, .71f); } }
    public static Color Indigo { get { return new Color(.24f, .31f, .71f); } }
    public static Color Blue { get { return new Color(.13f, .59f, .95f); } }
    public static Color Cyan { get { return new Color(0, .73f, .83f); } }
    public static Color Teal { get { return new Color(0, .59f, .53f); } }
    public static Color Green { get { return new Color(.3f, .69f, .31f); } }
    public static Color Yellow { get { return new Color(1, .92f, .23f); } }
    public static Color Orange { get { return new Color(1, .59f, 0); } }
    public static Color Grey { get { return new Color(.62f, .62f, .62f); } }
    public static Color BlueGrey { get { return new Color(.38f, .49f, .55f); } }
    public static Color Brown { get { return new Color(.47f, .33f, .28f); } }
    public static List<Color> All = new List<Color>() { Red, Pink, Purple, DeepPurple, Indigo, Blue, Cyan, Teal, Green, Yellow, Orange, Grey, BlueGrey, Brown };
    /*   public static Dictionary<Color, List<Color>> Shades = new Dictionary<Color, List<Color>>() {
          {Red, new List<Color>(){Hex("#ffebee"),Hex("ffcdd2"),Hex("ef9a9a"),Hex("e57373"),Hex("ef5350"),Hex("e53935"),Hex("d32f2f"),Hex("c62828"),Hex("b71c1c")}},
          {Pink, new List<Color>(){Hex("fce4ec"),Hex("f8bbd0"),Hex("f48fb1"),Hex("f06292"), Hex("ec407a"),Hex("d81b60"),Hex("c2185b"),Hex("ad1457"),Hex("880e4f")}},
          {Purple, new List<Color>(){Hex("f3e5f5"),Hex("e1bee7"),Hex("ce93d8"),Hex("ba68c8"),Hex("ab47bc"),Hex("8e24aa"),Hex("7b1fa2"),Hex("6a1b9a"),Hex("4a148c")}},
          {DeepPurple, new List<Color>(){Hex("ede7f6"),Hex("d1c4e9"),Hex("b39ddb"),Hex("9575cd"),Hex("7e57c2"),Hex("5e35b1"),Hex("512da8"),Hex("4527a0"),Hex("311b92")}},
          {Indigo, new List<Color>(){Hex("e8eaf6"),Hex("c5cae9"),Hex("9fa8da"),Hex("7986cb"),Hex("5c6bc0"),Hex("3949ab"),Hex("303f9f"),Hex("283593"),Hex("1a237e")}},
          {Blue, new List<Color>(){Hex("e3f2fd"),Hex("bbdefb"),Hex("90caf9"),Hex("64b5f6"),Hex("42a5f5"),Hex("1e88e5"),Hex("1976d2"),Hex("1565c0"),Hex("0d47a1")}},
  {Cyan,new List<Color>(){Hex("e0f7fa"),Hex("b2ebf2"),Hex("80deea"),Hex("4dd0e1"),Hex("26c6da"),Hex("00acc1"),Hex("0097a7"),Hex("00838f"),Hex("006064")}},
  {Teal, new List<Color>(){Hex("e0f2f1"),Hex("b2dfdb"),Hex("80cbc4"),Hex("4db6ac"),Hex("26a69a"),Hex("00897b"),Hex("00796b"),Hex("00695c"),Hex("#004d40")}},
  {Green, new List<Color>(){Hex("e8f5e9"),Hex("c8e6c9"),Hex("a5d6a7"),Hex("81c784"),Hex("66bb6a"),Hex("43a047"),Hex("388e3c"),Hex("2e7d32"),Hex("1b5e20")}},
  {Yellow, new List<Color>(){Hex("fffde7"), Hex("fff9c4"),Hex("fff59d"),Hex("fff176"),Hex("ffee58"),Hex("fdd835"),Hex("fbc02d"),Hex("f9a825"),Hex("f57f17")}},
  {Orange, new List<Color>(){Hex("fff3e0"), Hex("ffe0b2"),Hex("ffcc80"),Hex("ffb74d"),Hex("ffa726"),Hex("fb8c00"),Hex("f57c00"),Hex("ef6c00"),Hex("e65100")}},
  {Grey, new List<Color>(){Hex("fafafa"),Hex("f5f5f5"),Hex("eeeeee"),Hex("e0e0e0"),Hex("9e9e9e"),Hex("757575"),Hex("616161"),Hex("424242"),Hex("212121")}},
  {BlueGrey, new List<Color>(){Hex("eceff1"),Hex("cfd8dc"),Hex("b0bec5"),Hex("90a4ae"),Hex("78909c"),Hex("546e7a"),Hex("455a64"),Hex("37474f"),Hex("263238")}},
  {Brown, new List<Color>(){Hex("efebe9"),Hex("d7ccc8"),Hex("bcaaa4"),Hex("a1887f"),Hex("8d6e63"),Hex("6d4c41"),Hex("5d4037"),Hex("4e342e"),Hex("3e2723")}}
       };
   */
    public static Color Hex(string hex)
    {
        string code = hex;
        code = code.Replace("#", "");
        code = code.Replace(" ", "");
        code = code.Replace("0x", "");
        if (code.Length == 3)
        {
            code = "" + code[0] + code[0] + code[1] + code[1] + code[2] + code[2];
        }
        if (code.Length != 6)
        {
            Debug.Log("Invalid Hex Code");
        }

        Color col;
        if (ColorUtility.TryParseHtmlString("#" + code, out col))
        {
            return col;
        }
        return Color.white;
    }
    public static Color Random()
    {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }
    public static Color Hue(float hue)
    {
        return Color.HSVToRGB(hue, 1, 1);
    }


}
