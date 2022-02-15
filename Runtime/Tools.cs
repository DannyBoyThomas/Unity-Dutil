using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Dutil;
using UnityEditor.PackageManager.Requests;
using System.Threading.Tasks;

public class Tools
{
    static bool isDutilUpdating = false;
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
        if (isDutilUpdating)
        {
            Debug.Log("Dutil is already updating.");
            return;
        }
        isDutilUpdating = true;
        Debug.Log("Updating Dutil...");
        Task.Run(async () =>
       {
           AddRequest req = UnityEditor.PackageManager.Client.Add("https://github.com/DannyBoyThomas/Unity-Dutil.git");
           while (!req.IsCompleted)
           {
               await Task.Delay(300);
               Debug.Log(".");
           }
           string res = req.Status == UnityEditor.PackageManager.StatusCode.Success ? " updated successfully." : " failed to update.";
           Debug.Log("Dutil" + res);
           isDutilUpdating = false;
       });


    }
}
