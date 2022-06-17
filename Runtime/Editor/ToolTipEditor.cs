using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using UnityEditor;
[CustomEditor(typeof(ToolTip))]
public class ToolTipEditor : Editor
{
    /*  public override void OnInspectorGUI()
     {
         DrawDefaultInspector();
         var myScript = target as ToolTip;

         // myScript.showAfterHoverDelay = GUILayout.Toggle(myScript.flag, "Flag");

         if (myScript.showAfterHoverDelay)
             myScript.hoverDelay = EditorGUILayout.Slider("Hover Delay", myScript.hoverDelay, 0.1f, 2);

     }
 } */
}