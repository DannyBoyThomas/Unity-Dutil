using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dutil
{

    public class Toast
    {
        struct ToastStruct
        {
            public string message;
            public Color color;
            public ToastStruct(string _message, Color _color)
            {
                message = _message;
                color = _color;
            }
        }
        static Text textObj;
        static List<ToastStruct> messages = new List<ToastStruct>();
        public static void Show(string message)
        {
            Show(message, Color.white);
        }
        public static void Show(string message, Color color)
        {
            messages.Add(new ToastStruct(message, color));
            if (messages.Count == 1)
            {
                DisplayToast();
            }
        }
        static void DisplayToast()
        {
            if (messages.Count == 0)
            {
                return;
            }
            GetTextObj().text = messages[0].message;
            GetTextObj().color = messages[0].color;
            float animationTime = .4f;
            float readTime = 4f;
            AutoLerp.Begin(animationTime, -80, 20, (task, value) =>
            {
                GetTextObj().rectTransform.anchoredPosition = new Vector2(0, value);
            }, true, true);
            Schedule.Add(readTime + animationTime, (t) =>
            {
                AutoLerp.Begin(animationTime, 20, -80, (task, value) =>
                {
                    GetTextObj().rectTransform.anchoredPosition = new Vector2(0, value);
                }, true, true).OnComplete((t) => { messages.Pop(); if (messages.Count > 0) DisplayToast(); });

            });
        }

        static Text GetTextObj()
        {
            if (textObj != null)
            {
                return textObj;
            }
            GameObject myGO = new GameObject();
            myGO.name = "Dutil Canvas";
            Canvas myCanvas = myGO.AddComponent<Canvas>();
            myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            myGO.AddComponent<CanvasScaler>();
            myGO.AddComponent<GraphicRaycaster>();


            GameObject myText = new GameObject();
            myText.transform.parent = myGO.transform;
            myText.name = "Toast";

            Text text = myText.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.text = "wobble";
            text.fontSize = 20;
            text.alignment = TextAnchor.MiddleCenter;
            textObj = text;

            // Text position
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0.5f, 0);
            rectTransform.anchorMin = new Vector2(0.5f, 0);
            rectTransform.anchorMax = new Vector2(0.5f, 0);
            rectTransform.anchoredPosition = new Vector2(0, -80);
            rectTransform.sizeDelta = new Vector2(800, 80);
            return textObj;
        }
    }

}