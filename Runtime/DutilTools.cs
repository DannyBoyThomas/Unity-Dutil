using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager.Requests;

//using UnityEditor;
using System.Linq;
using Dutil;



public class DutilTools
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
    static SearchRequest searchRequest;
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
                Debug.Log("Dutil updated successfully");
                Debug.Log("You may need to restart your code editor for changes to take effect.");
                EditorPrefs.SetString("d_last_git_hash", addRequest.Result.git.hash);
                Debug.Log("Setting hash to" + addRequest.Result.git.hash);
            }
            else if (addRequest.Status >= UnityEditor.PackageManager.StatusCode.Failure)
                Debug.Log(addRequest.Error.message);

            EditorApplication.update -= UpdateProgress;
            addRequest = null;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    [MenuItem("Dutil/Check For Update")]
    static void CheckForUpdate()
    {
        if (searchRequest != null)
        {
            Debug.Log("Dutil is already checking for update.");
            return;
        }
        searchRequest = UnityEditor.PackageManager.Client.Search("com.dannyboythomas.dutil");
        EditorApplication.update += SearchProgress;

    }
    static void SearchProgress()
    {
        if (searchRequest.IsCompleted)
        {
            if (searchRequest.Status == UnityEditor.PackageManager.StatusCode.Success)
            {
                if (IsNewGitVersion())
                {

                    Debug.Log("Dutil is out of date. Update available.");
                }
                else
                {
                    Debug.Log("Dutil is up to date.");
                }
            }
            else if (searchRequest.Status >= UnityEditor.PackageManager.StatusCode.Failure)
                Debug.Log(searchRequest.Error.message);

            EditorApplication.update -= SearchProgress;
            searchRequest = null;
        }
    }
    static bool IsNewGitVersion()
    {
        //check if git hash is same as last time
        string lastGitHash = EditorPrefs.GetString("d_last_git_hash", "");
        return !searchRequest.Result.ToList().Any(x => x.git.hash == lastGitHash);
    }

    [MenuItem("Dutil/Beautify %&B")]
    public static void Beautify()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.Log("No gameobject selected");
            return;
        }
        Undo.RecordObjects(Selection.gameObjects.ToList().ToArray(), "Beautify");
        foreach (var item in Selection.gameObjects)
        {
            Material mat = item.GetComponent<Renderer>().sharedMaterial;
            Color col = mat.color;
            mat.shader = Shader.Find("Dutil/Half Lambert");
            mat.color = col;
            mat.SetFloat("_WrapAmount", .55f);
        }

    }

}
#endif