using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
namespace Dutil
{
    [RequireComponent(typeof(Collider2D))]
    public class ToolTip : MonoBehaviour
    {
        [Multiline]
        public string text = "I'm a ToolTip";
        [Header("Camera")]
        public bool getCameraById = false;
        [ConditionalHideProperty("getCameraById")]
        public Camera cam;
        Camera camFromID;
        [ConditionalShowProperty("getCameraById")]
        public string camTrackingId = "";
        public Color color = Color.white;
        public Color backgroundColor = new Color(0.03f, 0.07f, .21f, 0.7f);
        [Header("Dimensions")]
        public Direction2D offsetDirection = Direction2D.Left;
        public float offsetAmount = .1f;
        [Range(0, 1f)]
        public float cornerRadius = .1f;
        public float maxWidth = 600;
        [Header("Refine")]
        public bool showAfterHoverDelay = true;
        [ConditionalShowProperty("showAfterHoverDelay")]
        [Range(.1f, 2)]
        public float hoverDelay = .5f;
        public bool fade = true;
        public Font font;
        public int fontSize = 14;
        [Header("Other")]
        public bool forceKeepOnScreen = true;
        public bool preview = false;


        Transform tooltip;
        bool wasPreviewing = false;
        bool hovered = false;
        float timeHovered = 0;
        bool showing = false;
        float currentOpacity = 0;
        // Start is called before the first frame update

        void Reset()
        {
            cam = Cam();
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            if (hovered)
            {
                timeHovered += Time.unscaledDeltaTime;
            }
            int targetOpacity = showing ? 1 : 0;
            if (fade)
            {
                currentOpacity = Mathf.Lerp(currentOpacity, targetOpacity, Time.unscaledDeltaTime * 10);
            }
            else
            {
                currentOpacity = targetOpacity;
            }
            if (text.Trim().Length == 0)
            {
                hovered = false;
                showing = false;
                timeHovered = 0;
                currentOpacity = 0;
            }
            if (preview || (showAfterHoverDelay ? timeHovered > hoverDelay : hovered))
            {
                GetToolTip().gameObject.SetActive(true);
                showing = true;
                GetToolTip().GetComponent<RectTransform>().anchoredPosition = ConvertWorldPositionToCanvasPosition();
                GetText().text = text;
                GetText().font = font;
                GetText().fontSize = fontSize;
                GetText().color = color.WithAlpha(color.a * currentOpacity);
                Image img = GetToolTip().GetComponent<Image>();
                img.material.SetColor("_Color", backgroundColor.WithAlpha(backgroundColor.a * currentOpacity));
                img.material.SetFloat("_Width", GetToolTip().GetComponent<RectTransform>().sizeDelta.x);
                img.material.SetFloat("_Height", GetToolTip().GetComponent<RectTransform>().sizeDelta.y);
                img.material.SetFloat("_Radius", cornerRadius);

                Justify();
            }
            else if (wasPreviewing)
            {

                //GetToolTip().GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1000);
            }
            else
            {
                showing = false;
                if (fade)
                {
                    Image img = GetToolTip().GetComponent<Image>();
                    if (currentOpacity > .05f)
                    {
                        GetText().color = color.WithAlpha(color.a * currentOpacity);
                        img.material.SetColor("_Color", backgroundColor.WithAlpha(backgroundColor.a * currentOpacity));
                    }
                    else
                    {
                        GetText().color = color.WithAlpha(0);
                        img.material.SetColor("_Color", backgroundColor.WithAlpha(0));
                        GetToolTip().gameObject.SetActive(false);
                    }
                }
                else
                {
                    GetToolTip().gameObject.SetActive(false);
                }
            }

            wasPreviewing = preview;
        }
        Camera Cam()
        {

            if (getCameraById)
            {
                if (camFromID == null)
                {
                    camFromID = D.TrackFirst<GameObject>(camTrackingId).GetComponent<Camera>();
                }
                return camFromID;
            }
            else
            {
                return cam;
            }
        }
        Text GetText()
        {
            if (tooltip == null)
            {
                GetToolTip();
            }
            return tooltip.GetComponentInChildren<Text>();
        }

        Transform GetToolTip()
        {
            if (tooltip != null)
            {
                return tooltip;
            }
            GameObject myGO;
            GameObject existingCanvas = D.TrackFirst<GameObject>("Dutil Canvas");
            if (existingCanvas == null)
            {
                myGO = new GameObject();
                myGO.name = "Dutil Canvas";
                Canvas myCanvas = myGO.AddComponent<Canvas>();
                myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                CanvasScaler scaler = myGO.AddComponent<CanvasScaler>();
                //scaler.referenceResolution = new Vector2(1920, 1080);
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                myGO.AddComponent<GraphicRaycaster>();
                D.Track("Dutil Canvas", myGO);
            }
            else
            {
                myGO = existingCanvas;
            }

            GameObject bgPanel = new GameObject("Tool Tip");
            bgPanel.transform.SetParent(myGO.transform);
            RectTransform rt = bgPanel.AddComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = Vector2.zero;
            Image img = bgPanel.AddComponent<Image>();
            img.material = new Material(Shader.Find("Dutil/RoundedCorners"));


            GameObject myText = new GameObject();
            myText.transform.parent = bgPanel.transform;
            myText.name = "Tool Tip Text";

            Text text = myText.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            ContentSizeFitter csf = myText.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            text.alignment = TextAnchor.MiddleCenter;

            LayoutElement le = myText.AddComponent<LayoutElement>();
            le.preferredWidth = maxWidth;
            le.flexibleHeight = 20;

            text.fontSize = fontSize;

            // Text position
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            //rectTransform.anchorMin = rectTransform.anchorMax = Vector2.zero;
            rectTransform.anchoredPosition = new Vector2(0, 0);
            //rectTransform.sizeDelta = new Vector2(600, 120);

            tooltip = bgPanel.transform;
            return bgPanel.transform;
        }

        public void OnMouseExit()
        {
            hovered = false;
            if (!fade)
            {
                GetToolTip().gameObject.SetActive(false);
            }
            timeHovered = 0;
        }

        public void OnMouseOver()
        {
            hovered = true;

        }
        Vector2 ConvertWorldPositionToCanvasPosition()
        {
            Collider2D col = GetComponent<Collider2D>();
            Vector2 pos = col.bounds.ClosestPoint(col.transform.position + (offsetDirection.ToVector().XY() * 10));
            pos += offsetDirection.ToVector() * offsetAmount;
            //worldpoint to screen point
            Vector2 screenPos = Cam()?.WorldToScreenPoint(pos) ?? Vector2.one * -10;
            Debug.Log(screenPos);
            return screenPos;
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
        void Justify()
        {

            //GetText().alignment = TextAnchor.MiddleCenter;
            switch (offsetDirection)
            {
                case Direction2D.Left:
                    //GetText().alignment = TextAnchor.MiddleLeft;
                    GetToolTip().GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                    break;
                case Direction2D.Right:
                    //GetText().alignment = TextAnchor.MiddleLeft;
                    GetToolTip().GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
                    break;
                case Direction2D.Up:
                    //GetText().alignment = TextAnchor.LowerLeft;
                    GetToolTip().GetComponent<RectTransform>().pivot = new Vector2(.5f, 0f);
                    break;
                case Direction2D.Down:
                    //GetText().alignment = TextAnchor.UpperLeft;
                    GetToolTip().GetComponent<RectTransform>().pivot = new Vector2(.5f, 1.0f);
                    break;

            }

            if (forceKeepOnScreen)
            {
                Vector2 size = FitToText();
                float width = size.x;
                float height = size.y;


                float padding = 10;
                float left = (GetText().rectTransform.position.x - width * .5f) - padding;
                float right = left + width + padding * 2;
                float bottom = (GetText().rectTransform.position.y - height * .5f) - padding;
                float top = bottom + height + padding * 2;
                Debug.DrawLine(new Vector3(left, bottom, 0), new Vector3(right, bottom, 0), Color.red);
                Debug.DrawLine(new Vector3(left, top, 0), new Vector3(right, top, 0), Color.red);
                Debug.DrawLine(new Vector3(left, bottom, 0), new Vector3(left, top, 0), Color.red);
                Debug.DrawLine(new Vector3(right, bottom, 0), new Vector3(right, top, 0), Color.red);
                //keep all in bounds
                if (left < 0)
                {
                    GetToolTip().GetComponent<RectTransform>().anchoredPosition += new Vector2(0 - left, 0);
                }
                if (right > Screen.width)
                {
                    GetToolTip().GetComponent<RectTransform>().anchoredPosition += new Vector2(Screen.width - right, 0);
                }
                if (top > Screen.height)
                {
                    GetToolTip().GetComponent<RectTransform>().anchoredPosition += new Vector2(0, Screen.height - top);
                }
                if (bottom < 0)
                {
                    GetToolTip().GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -bottom);
                }


            }
        }
        Vector2 FitToText()
        {
            //get width of rect transform
            float width = GetText().rectTransform.rect.width;
            float height = GetText().rectTransform.rect.height;
            int count = GetText().text.Length;

            float multi = count < 10 ? 15 : 8.5f;
            float prefWidth = Mathf.Min(count * multi, maxWidth);
            GetText().GetComponent<LayoutElement>().preferredWidth = prefWidth;
            GetToolTip().GetComponent<RectTransform>().sizeDelta = GetText().rectTransform.rect.size + Vector2.one * 20;
            return GetText().rectTransform.rect.size;

        }
    }

}