using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager.Requests;
//using UnityEditor;
using System.Linq;
using Dutil;
using UnityEngine.UI;

namespace Dutil
{
    public partial class DutilTools
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

        [MenuItem("Dutil/Create/Centered HUD")]
        public static void CreateCentredHUD()
        {
            bool canvasCreated = false;
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                canvas = new GameObject("Canvas").AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.gameObject.AddComponent<CanvasScaler>();
                canvas.gameObject.AddComponent<GraphicRaycaster>();

                canvasCreated = true;
            }
            DUI_Canvas dCanvas = canvas.gameObject.GetOrAddComponent<DUI_Canvas>();
            dCanvas.autoRefresh = true;
            dCanvas.autoRefreshDelay = .5f;
            dCanvas.Align();

            GameObject hud = new GameObject("HUD");
            hud.transform.SetParent(canvas.transform);
            RectTransform rt = hud.AddComponent<RectTransform>();
            float w = rt.rect.width;
            float h = rt.rect.height;
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, .5f);
            rt.sizeDelta = new Vector2(w, h);
            rt.position = Vector3.zero;
            rt.anchoredPosition = Vector2.zero;
            hud.AddComponent<CanvasRenderer>();
            Image img = hud.AddComponent<Image>();
            img.color = Colours.Blue.WithAlpha(.5f);

            DUI_Height dHeight = hud.AddComponent<DUI_Height>();
            dHeight.forceToCenter = true;
            dHeight.useTargetPercentage = true;
            dHeight.targetHeightPercentage = 100;
            dHeight.useMinHeight = true;
            dHeight.minHeight = 400;
            dHeight.usePadding = true;
            dHeight.padding = 4;
            dHeight.paddingUnit = DUI_UnitType.Percentage;


            DUI_Width dWidth = hud.AddComponent<DUI_Width>();
            dWidth.forceToCenter = true;
            dWidth.useTargetPercentage = true;
            dWidth.targetWidthPercentage = 60;
            dWidth.useMinWidth = true;
            dWidth.minWidth = 400;
            dWidth.useMaxWidth = true;
            dWidth.maxWidth = 2560;
            dWidth.usePadding = true;
            dWidth.padding = 40;


            GameObject a = CreateCornerHUDPanel(new Vector2(0, 1), "Top Left", hud.transform);
            GameObject b = CreateCornerHUDPanel(new Vector2(1, 1), "Top Right", hud.transform);
            GameObject c = CreateCornerHUDPanel(new Vector2(0, 0), "Bottom Left", hud.transform);
            GameObject d = CreateCornerHUDPanel(new Vector2(1, 0), "Bottom Right", hud.transform);

            //create undo group
            if (canvasCreated)
            {
                Undo.RegisterCreatedObjectUndo(canvas.gameObject, "Create HUD");
            }
            else
            {
                Undo.RegisterCreatedObjectUndo(hud.gameObject, "Create HUD");
            }
        }
        static GameObject CreateCornerHUDPanel(Vector2 anchor, string name, Transform parent, Color? col = null)
        {
            if (col == null)
            {
                col = Colours.Green;
            }
            GameObject panel = new GameObject(name);
            panel.transform.SetParent(parent);
            RectTransform rt = panel.AddComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = rt.pivot = anchor;
            rt.position = Vector3.zero;
            rt.anchoredPosition = Vector2.zero;
            panel.AddComponent<CanvasRenderer>();
            Image img = panel.AddComponent<Image>();
            img.color = col.Value;

            DUI_Height dHeight = panel.AddComponent<DUI_Height>();
            dHeight.useTargetPercentage = true;
            dHeight.targetHeightPercentage = 8;
            dHeight.useMinHeight = true;
            dHeight.minHeight = 50;
            dHeight.useMaxHeight = true;
            dHeight.maxHeight = 200;
            dHeight.forceToCenter = false;

            DUI_Width dWidth = panel.AddComponent<DUI_Width>();
            dWidth.useTargetPercentage = true;
            dWidth.targetWidthPercentage = 38;
            dWidth.useMinWidth = true;
            dWidth.minWidth = 80;
            dWidth.useMaxWidth = true;
            dWidth.maxWidth = 800;
            dWidth.forceToCenter = false;

            rt.anchorMin = rt.anchorMax = rt.pivot = anchor;
            rt.position = Vector3.zero;
            rt.anchoredPosition = Vector2.zero;
            return panel;
        }

    }
}
#endif