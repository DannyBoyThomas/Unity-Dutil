using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Dutil;
using UnityEditor.PackageManager;
public class Tools
{
    [MenuItem("Dutil/Group %&g")]
    public static void Group()
    {
        Vector3 centre = Selection.gameObjects.ToList().Select(x => x.transform.position).ToList().Average();

        GameObject g = new GameObject("Group");
        Undo.RegisterCreatedObjectUndo(g, "new group");
        Selection.gameObjects.ToList().ForEach(x => Undo.SetTransformParent(x.transform, g.transform, "Setting new parent"));
        Selection.objects = new Object[] { g };

    }
    [MenuItem("Dutil/Update %&u")]
    public static void UpdateDutil()
    {
        UnityEditor.PackageManager.Client.Add("https://github.com/DannyBoyThomas/Unity-Dutil.git");

    }
}
