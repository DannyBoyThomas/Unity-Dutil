using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager.Requests;
#endif
//using UnityEditor;
using System.Linq;
using Dutil;



public class Tools
{
    public static bool AutoInsertDutil
    {
        get
        {
            return EditorPrefs.GetBool("d_auto_insert", false);
        }
        set
        {
            EditorPrefs.SetBool("d_auto_insert", value);
        }
    }
    static AddRequest addRequest;
    [MenuItem("Dutil/Group %&g")]
    public static void Group()
    {
        Transform originalParent = Selection.activeTransform.parent;

        Vector3 centre = Selection.gameObjects.ToList().Select(x => x.transform.position).ToList().Average();
        GameObject g = new GameObject("Group");
        Undo.RegisterCreatedObjectUndo(g, "new group");
        Selection.gameObjects.ToList().ForEach(x => Undo.SetTransformParent(x.transform, g.transform, "Setting new parent"));
        Undo.SetTransformParent(g.transform, originalParent, "Setting original parent");
        Selection.objects = new Object[] { g };

    }
    [MenuItem("Dutil/Auto Insert")]
    public static void ToggleInsertDutil()
    {

        AutoInsertDutil = !AutoInsertDutil;
        Menu.SetChecked("Dutil/Auto Insert", AutoInsertDutil);
        EditorPrefs.SetBool("d_auto_insert", AutoInsertDutil);
    }
    [MenuItem("Dutil/Logging")]
    public static void ToggleLogging()
    {
        D.AllowLogging = !D.AllowLogging;
        Menu.SetChecked("Dutil/Logging", D.AllowLogging);
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
            {
                Debug.Log("Dutil updated succesfully");
                Debug.Log("You may need to restart your code editor for changes to take effect.");
            }
            else if (addRequest.Status >= UnityEditor.PackageManager.StatusCode.Failure)
                Debug.Log(addRequest.Error.message);

            EditorApplication.update -= UpdateProgress;
            addRequest = null;
        }
    }

}
