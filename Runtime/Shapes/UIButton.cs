using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//has Shapes Package or shapes hdrp
#if SHAPES_INSTALLED && TMPRO_INSTALLED
using Shapes;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
namespace Dutil
{
    public enum UiHoverType { Tint, Curve, Scale }
    [ExecuteInEditMode]
    public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Vector2 size = new Vector2(180, 60);
        public Vector2 padding = new Vector2(20, 0);
        public string text = "Button";
        public Color textColor = Color.white;
        public Color backgroundColor = Colours.Green;

        [Range(0, 1)]
        public float gradientWeight = 0;
        [Min(0)]
        public float backgroundShadowOffset = 4;
        public float backgroundCornerRadius = 6;
        public UiHoverType hoverType = UiHoverType.Tint;
        Rectangle background, backgroundShadow;
        TextMeshProUGUI textObj;
        Button button;
        public UnityEvent onClick = new UnityEvent();
        Vector2 lastPadding = new Vector2(20, 0);
        bool shouldRefresh = false;
        float lastGradientWeight;
        Color disableColor;
        bool disabled = false;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Reset()
        {
            transform.localScale = Vector3.one;
            RectTransform.localScale = Vector3.one;
            Text = "Hello World";
            TextColor = Color.white;
            BackgroundColor = Colours.Green;
            size = new Vector2(180, 60);
            padding = lastPadding = new Vector2(20, 0);
            BackgroundShadowOffset = 4;
            BackgroundCornerRadius = 6;
            RectTransform.sizeDelta = size;
            gradientWeight = lastGradientWeight = 0;
            disableColor = Colours.Hex("595959");
            SetColors();
            Refresh();
        }



        public TextMeshProUGUI TextObj
        {
            get
            {
                TextMeshProUGUI res = textObj ??= Background.transform.Find("Text")?.GetComponent<TextMeshProUGUI>();
                if (res == null)
                {
                    GameObject g = new GameObject("Text");
                    g.transform.SetParent(Background.transform);
                    g.transform.localScale = Vector3.one;
                    g.transform.localPosition = Vector3.zero;
                    res = g.AddComponent<TextMeshProUGUI>();
                    res.text = text;
                    res.alignment = TextAlignmentOptions.Center;
                    res.fontSize = 20;
                    res.color = Color.black;
                    res.enableAutoSizing = true;
                    res.fontSizeMax = 32;
                    res.raycastTarget = false;
                    RectTransform rt = g.GetOrAddComponent<RectTransform>();
                    SetRectTransformToFillAtCentre(rt);
                    rt.offsetMin = new Vector2(padding.x, padding.y);
                    rt.offsetMax = new Vector2(-padding.x, -padding.y);
                    rt.localPosition = (new Vector3(0, 0, -.1f));
                }
                textObj = res;
                return res;
            }
        }
        public Rectangle Background
        {
            get
            {
                Rectangle res = background ??= BackgroundShadow.transform.Find("Background")?.GetComponent<Rectangle>();
                if (res == null)
                {
                    GameObject g = new GameObject("Background");
                    res = g.AddComponent<Rectangle>();
                    res.transform.SetParent(BackgroundShadow.transform);
                    res.Color = backgroundColor;
                    res.transform.localScale = Vector3.one;
                    res.transform.localRotation = Quaternion.identity;
                    res.transform.localPosition = new Vector3(0, 2, -.01f);
                    res.Width = size.x;
                    res.Height = size.y;
                    res.FillColorStart = StartGrad;
                    res.FillColorEnd = EndGrad;
                    res.FillLinearStart = new Vector2(0, size.y / 2f);
                    res.FillLinearEnd = new Vector2(0, -size.y / 2f);
                    SetRectTransformToFillAtCentre(g.GetOrAddComponent<RectTransform>());
                }
                background = res;
                return res;
            }
        }
        public Rectangle BackgroundShadow
        {
            get
            {
                Rectangle res = backgroundShadow ??= transform.Find("BackgroundShadow")?.GetComponent<Rectangle>();
                if (res == null)
                {
                    GameObject g = new GameObject("BackgroundShadow");
                    res = g.AddComponent<Rectangle>();
                    res.transform.SetParent(transform);
                    res.Color = DarkerBG;
                    res.transform.localScale = Vector3.one;
                    res.transform.localRotation = Quaternion.identity;
                    res.transform.localPosition = Vector3.zero;
                    res.Width = size.x;
                    res.Height = size.y;
                    SetRectTransformToFillAtCentre(g.GetOrAddComponent<RectTransform>());
                }
                backgroundShadow = res;
                return res;
            }
        }
        public string Text { get => text; set { text = value; TextObj.text = value; } }
        public Color TextColor { get => textColor; set { textColor = value; TextObj.color = value; } }
        public Color BackgroundColor
        {
            get => backgroundColor; set
            {
                backgroundColor = value; Background.Color = value;
                BackgroundShadow.Color = DarkerBG;
            }
        }
        public float BackgroundShadowOffset
        {
            get => backgroundShadowOffset; set
            {
                backgroundShadowOffset = value;
                Background.transform.localPosition = new Vector3(0, value / 2f, -.01f);
            }
        }
        public float BackgroundCornerRadius
        {
            get => backgroundCornerRadius; set
            {
                float newVal = Mathf.Max(0, value);
                Background.Type = BackgroundShadow.Type = newVal > 0 ? Rectangle.RectangleType.RoundedSolid : Rectangle.RectangleType.HardSolid;
                backgroundCornerRadius = newVal; Background.CornerRadius = newVal; BackgroundShadow.CornerRadius = newVal;
            }
        }
        public RectTransform RectTransform { get => gameObject.GetOrAddComponent<RectTransform>(); }
        Color DarkerBG { get => backgroundColor.Darken(gradientWeight * .75f); }
        Color LighterBG { get => backgroundColor.Lighten(gradientWeight * .75f); }
        Color StartGrad { get => LighterBG; }
        Color EndGrad { get => DarkerBG; }
        void OnValidate()
        {
            shouldRefresh = true;
        }
        void Refresh()
        {
            if (text != TextObj.text)
            {
                TextObj.text = text;
            }
            if (textColor != TextObj.color)
            {
                TextObj.color = textColor;
            }
            if (backgroundColor != Background.Color)
            {
                SetColors();
            }

            if (size.y != RectTransform.sizeDelta.y || size.x != RectTransform.sizeDelta.x)
            {
                RectTransform.sizeDelta = new Vector2(size.x, size.y);
                Rectangle rect = Background.GetComponent<Rectangle>();
                rect.FillLinearStart = new Vector2(0, size.y / 2f);
                rect.FillLinearEnd = new Vector2(0, -size.y / 2f);
            }
            if (size.x != Background.Width)
            {
                Background.Width = size.x;
                BackgroundShadow.Width = size.x;
                //     RectTransform rt = Background.GetComponent<RectTransform>();
                //     rt.sizeDelta = new Vector2(size.x, rt.sizeDelta.y);
                //     RectTransform rt2 = BackgroundShadow.GetComponent<RectTransform>();
                //     rt2.sizeDelta = new Vector2(size.x, rt2.sizeDelta.y);
                // }
            }
            if (size.y - BackgroundShadowOffset != Background.Height)
            {
                Background.Height = size.y - BackgroundShadowOffset;
                BackgroundShadow.Height = size.y;

                RectTransform rt = Background.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, size.y - BackgroundShadowOffset);
                rt.offsetMax = new Vector2(rt.offsetMax.x, 0);
                rt.offsetMin = new Vector2(rt.offsetMin.x, 0);

            }


            if (backgroundShadowOffset != Background.transform.localPosition.y)
            {
                BackgroundShadowOffset = backgroundShadowOffset;
            }
            if (backgroundCornerRadius != Background.CornerRadius)
            {
                BackgroundCornerRadius = backgroundCornerRadius;
            }
            if (lastPadding != padding)
            {
                lastPadding = padding;
                RectTransform rt = TextObj.GetComponent<RectTransform>();
                rt.offsetMin = new Vector2(padding.x, padding.y);
                rt.offsetMax = new Vector2(-padding.x, -padding.y);
                DrawRectWithPadding();
            }
            if (lastGradientWeight != gradientWeight)
            {
                SetColors();
                lastGradientWeight = gradientWeight;
            }

        }
        void SetColors()
        {
            Rectangle rect = Background.GetComponent<Rectangle>();
            bool flat = gradientWeight == 0;
            rect.UseFill = !flat;
            BackgroundColor = backgroundColor;
            rect.FillColorStart = StartGrad;
            rect.FillColorEnd = EndGrad;
            BackgroundShadow.Color = DarkerBG.Darken();
            rect.UpdateMesh();
            //repaint scene
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }
        void SetRectTransformToFillAtCentre(RectTransform rt)
        {
            //stretch
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(.5f, .5f);
            rt.offsetMin = rt.offsetMax = Vector2.zero;
        }

        void DrawRectWithPadding()
        {
            float duration = .01f;
            Color col = BackgroundColor.Opposite();
            Vector2 size = RectTransform.sizeDelta - padding * 2;
            Vector2 pos = Centre(RectTransform);
            Vector3[] corners = new Vector3[4];
            TextObj.GetComponent<RectTransform>().GetWorldCorners(corners);
            Debug.DrawLine(corners[0], corners[1], col, duration);
            Debug.DrawLine(corners[1], corners[2], col, duration);
            Debug.DrawLine(corners[2], corners[3], col, duration);
            Debug.DrawLine(corners[3], corners[0], col, duration);
        }
        Vector2 Centre(RectTransform rt)
        {
            return rt.position + new Vector3(rt.rect.width / 2, rt.rect.height / 2, 0);

        }
        void SetTop(RectTransform rt, float val = 0)
        {
            //top =val
            rt.offsetMax = new Vector2(rt.offsetMax.x, val);
        }
        void SetBottom(RectTransform rt, float val = 0)
        {
            //bottom =val
            rt.offsetMin = new Vector2(rt.offsetMin.x, val);
        }
        void SetLeft(RectTransform rt, float val = 0)
        {
            //left =val
            rt.offsetMin = new Vector2(val, rt.offsetMin.y);
        }
        void SetRight(RectTransform rt, float val = 0)
        {
            //right =val
            rt.offsetMax = new Vector2(val, rt.offsetMax.y);
        }
        //Interaction
        Color beforeHoverColor;
        float beforeHoverScale;
        float beforeHoverCurve;

        Color beforeTextColor;

        float hoverScale = 1.1f;
        float hoverCurve = 30;
        Color hoverColor;
        float hoverDuration = .1f;
        float hoverCompletion = 0;
        bool isHovering = false;
        void Start()
        {

            beforeHoverColor = BackgroundColor;
            beforeHoverScale = transform.localScale.x;
            beforeHoverCurve = BackgroundCornerRadius;
            beforeTextColor = textColor;

            transform.localScale = Vector3.one;
            RectTransform.localScale = Vector3.one;
            RectTransform.sizeDelta = size;
            lastGradientWeight = gradientWeight;

            hoverColor = BackgroundColor.Darken(.1f);
            disableColor = Colours.Hex("595959");



            SetColors();
            Refresh();
        }
        void Update()
        {
            if (!allowHovering) { return; }
            if (!Application.isPlaying) { return; }
            if (disabled) { return; }
            int dir = isHovering ? 1 : -1;
            float multi = hoverType == UiHoverType.Curve ? 2 : 1;
            hoverCompletion += Time.deltaTime * dir / (hoverDuration * multi);
            hoverCompletion = Mathf.Clamp01(hoverCompletion);
            if (hoverType == UiHoverType.Tint)
            {
                BackgroundColor = Color.Lerp(beforeHoverColor, hoverColor, hoverCompletion);
            }
            if (hoverType == UiHoverType.Scale)
            {
                transform.localScale = Vector3.Lerp(Vector3.one * beforeHoverScale, Vector3.one * hoverScale, hoverCompletion);
            }
            if (hoverType == UiHoverType.Curve)
            {
                BackgroundCornerRadius = Mathf.Lerp(beforeHoverCurve, hoverCurve, hoverCompletion);
            }
        }



        public void OnPointerEnter(PointerEventData eventData)
        {
            if (disabled) { return; }
            isHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (disabled) { return; }
            isHovering = false;
            OnPointerUp(eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (disabled) { return; }
            OnClicked();
        }

        bool allowHovering = true;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (disabled) { return; }
            if (hoverType == UiHoverType.Tint)
            {
                allowHovering = false;
                isHovering = false;
            }
            BackgroundColor = beforeHoverColor.Darken();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (disabled) { return; }
            BackgroundColor = beforeHoverColor;
            if (hoverType == UiHoverType.Tint)
            {
                allowHovering = true;
            }
        }
        void OnClicked()
        {
            if (disabled) { return; }
            onClick.Invoke();
        }
        void LateUpdate()
        {
            if (shouldRefresh)
            {
                Refresh();
                shouldRefresh = false;
            }
        }
        AutoLerpTask disableTask;

        public void Disable(bool dis)
        {
            disabled = dis;
            int v = dis ? 0 : 1;
            BackgroundColor = Color.Lerp(disableColor, beforeHoverColor, v);
            SetColors();
            TextColor = Color.Lerp(disableColor.Lighten(.5f), beforeTextColor, v);
        }
    }
}
#endif