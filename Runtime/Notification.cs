using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using TMPro;
namespace Dutil
{
#if SHAPES_INSTALLED
    public class Notification : MonoBehaviour
    {
        static float offScreenX = 480;
        static float onScreenX = -445;
        System.Action callback;
        static bool IsCurrentlyShowing = false;
        static Queue<NotifcationData> queue = new Queue<NotifcationData>();
        static NotifcationData current;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        static void OnNotificationAdded()
        {
            if (!IsCurrentlyShowing)
            {
                IsCurrentlyShowing = true;
                current = queue.Dequeue();
                Show(current);
            }
        }
        static void OnNotificationClosed()
        {
            if (queue.Count > 0)
            {
                 current = queue.Dequeue();
                Show(current);
            }
            else
            {
                IsCurrentlyShowing = false;
            }
        }
        public static void ForceCloseCurrent()
        {
              IsCurrentlyShowing = false;
            if (current != null && current.Instance != null)
            {
                current.Instance?.OnAccept();
            }
            current =null;
          
        }
        public void Initialise(string title, string message, System.Action OnAccept)
        {
            string text = "<b>" + title + "</b>\n\n" + message;
            GetComponentInChildren<TextMeshProUGUI>().text = text;
            callback = OnAccept;
        }
        public void OnAccept()
        {
            //slide right
            callback?.Invoke();

            RectTransform rt = GetComponent<RectTransform>();
            float startingX = rt.anchoredPosition.x;
            AutoLerp.Begin(.15f, startingX, offScreenX, (t, v) =>
            {
                rt.anchoredPosition = new Vector2(v, rt.anchoredPosition.y);
            }).OnComplete((t) =>
            {
                OnNotificationClosed();
                Destroy(gameObject);
            });
        }
        static void Show(NotifcationData data)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Notification"));
            Transform t = data.parent ?? GameObject.Find("Canvas").transform;
            if(t==null)
            {
                Debug.LogError("No Canvas found");
                return;
            }
            go.transform.SetParent(t);
            RectTransform rt = go.GetOrAddComponent<RectTransform>();
            //anchor to middle right of canvas
            rt.anchorMin = new Vector2(1, .5f);
            rt.anchorMax = new Vector2(1, .5f);
            rt.pivot = new Vector2(1, .5f);
            rt.anchoredPosition = new Vector2(offScreenX, 200);
            Notification note =  go.GetComponent<Notification>();
           note.Initialise(data.title, data.message, data.onAccept);
           data.Instance = note;
            rt.transform.localScale = Vector3.one;
            rt.transform.localPosition = rt.localPosition.WithZ(-0.1f);
            //slide left
            AutoLerp.Begin(.15f, offScreenX, onScreenX, (t, v) =>
            {
                  if (rt != null)
                rt.anchoredPosition = new Vector2(v, rt.anchoredPosition.y);
            });
        }
        public static void Create(string title, string message, System.Action onAccept, Transform parent = null)
        {

            NotifcationData data = new NotifcationData(title, message, onAccept, parent);
            queue.Enqueue(data);
            OnNotificationAdded();
        }
       
    }
     class NotifcationData
        {
            public string title;
            public string message;
            public System.Action onAccept;
            public Transform parent;
            Notification instance;
            public NotifcationData(string title, string message, System.Action onAccept, Transform parent)
            {
                this.title = title;
                this.message = message;
                this.onAccept = onAccept;
                this.parent = parent;
            }
            public Notification Instance { get => instance; set => instance = value; }
        }
#endif
}