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

    static AddRequest addRequest;
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
        if (addRequest != null)
        {
            Debug.Log("Dutil is already updating.");
            return;
        }
        Debug.Log("Updating Dutil...");
        addRequest = UnityEditor.PackageManager.Client.Add("https://github.com/DannyBoyThomas/Unity-Dutil.git");
        EditorApplication.update += UpdateProgress;
    }
    static void UpdateProgress()
    {
        if (addRequest.IsCompleted)
        {
            if (addRequest.Status == UnityEditor.PackageManager.StatusCode.Success)
                Debug.Log("Dutil updated succesfully");
            else if (addRequest.Status >= UnityEditor.PackageManager.StatusCode.Failure)
                Debug.Log(addRequest.Error.message);

            EditorApplication.update -= UpdateProgress;
            addRequest = null;
        }
    }
    [InitializeOnLoadMethod]
    static void Huzzah()
    {
        Debug.Log("Huzzah!");
    }
}
