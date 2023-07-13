using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR && SHAPES_INSTALLED && TMPRO_INSTALLED
using UnityEditor;
using Shapes;
using TMPro;

namespace Dutil
{
    public partial class DutilTools
    {

        [MenuItem("GameObject/Dutil/Button", false, 1)]
        public static void CreateButton()
        {
            GameObject g = new GameObject("Button");
            g.AddComponent<RectTransform>();
            Undo.RegisterCreatedObjectUndo(g, "new button");

            //set parent
            if (Selection.activeGameObject != null)
            {
                g.transform.SetParent(Selection.activeGameObject.transform);
            }

            g.transform.localPosition = Vector3.zero;
            g.GetComponent<RectTransform>().localPosition = Vector3.zero;
            Image img = g.AddComponent<Image>();
            img.sprite = null;
            img.color = Color.clear;
            g.AddComponent<UIButton>().Reset();
            Selection.objects = new Object[] { g };


        }
    }
}
#endif
