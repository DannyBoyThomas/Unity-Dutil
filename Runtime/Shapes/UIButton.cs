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
    public enum UIButtonIconPosition { None, Left, Right }
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
        public float backgroundShadowOffset = 7;
        public float backgroundCornerRadius = 7;
        public UiHoverType hoverType = UiHoverType.Tint;
        public bool holdToAccept = false;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIfGroup("holdToAccept")]
#endif
        public float holdDuration = 1;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIfGroup("holdToAccept")]
#endif
        public string holdKey;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIfGroup("holdToAccept")]
#endif
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIfGroup("holdToAccept")]
#endif
        public bool showKey = false;
        public bool disabled = false;

        Rectangle background, backgroundShadow, progressObj;
        TextMeshProUGUI textObj, keyTextObj;
        Button button;
        public UnityEvent onClick = new UnityEvent();
        public UnityEvent<bool> OnDisableChangeEvent = new UnityEvent<bool>();
        Vector2 lastPadding = new Vector2(20, 0);
        bool shouldRefresh = false;
        float lastGradientWeight;
        Color disableColor;
        float holdProgress = 0;
        float holdDir = -1;
        float holdCollapseSpeed = 3;
        bool canCompleteHoldClick = true;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Reset()
        {
            transform.localScale = Vector3.one;
            RectTransform.localScale = Vector3.one;
            Text = "Click Me";
            TextColor = Color.white;
            BackgroundColor = Colours.Green;
            size = new Vector2(180, 60);
            padding = lastPadding = new Vector2(20, 0);
            BackgroundShadowOffset = 7;
            BackgroundCornerRadius = 7;
            RectTransform.sizeDelta = size;
            gradientWeight = lastGradientWeight = 0;
            disableColor = Colours.Hex("595959");
            SetColors();
            Refresh();
        }
        public TextMeshProUGUI KeyTextObj
        {
            get
            {
                TextMeshProUGUI res = keyTextObj ??= Background.transform.Find("KeyText")?.GetComponent<TextMeshProUGUI>();
                if (res == null)
                {
                    GameObject g = new GameObject("KeyText");
                    g.transform.SetParent(Background.transform);
                    g.transform.localScale = Vector3.one;
                    g.transform.localPosition = Vector3.zero;
                    res = g.AddComponent<TextMeshProUGUI>();
                    res.text = holdKey;
                    res.alignment = TextAlignmentOptions.Center;
                    res.fontSize = 20;

                    RectTransform rt = g.GetOrAddComponent<RectTransform>();
                    rt.localPosition = (new Vector3(0, 0, -.1f));

                    //anchor to left and offset by (-80,0)
                    rt.anchorMin = new Vector2(0, 0.5f);
                    rt.anchorMax = new Vector2(0, .5f);
                    rt.pivot = new Vector2(0, .5f);
                    float offset = -60;
                    rt.offsetMin = new Vector2(offset, 0);
                    rt.offsetMax = new Vector2(offset, 0);
                    rt.sizeDelta = Vector2.one * 40;

                    //Add Rectangle as child
                    GameObject g2 = new GameObject("KeyTextBackground");
                    g2.transform.SetParent(g.transform);
                    Rectangle bgRect = g2.AddComponent<Rectangle>();
                    bgRect.Color = Colours.Hex("3C3C3C");
                    bgRect.CornerRadius = 4;
                    bgRect.Type = Rectangle.RectangleType.RoundedSolid;
                    RectTransform rt2 = g2.GetOrAddComponent<RectTransform>();
                    rt2.localScale = Vector3.one;
                    rt2.localPosition = Vector3.zero.WithZ(.1f);
                    SetRectTransformToFillAtCentre(rt2);
                    ShapeConformsToRectTransform conform = g2.AddComponent<ShapeConformsToRectTransform>();


                }
                keyTextObj = res;
                return res;
            }
        }

        public Rectangle ProgressObj
        {
            get
            {

                Rectangle res = progressObj ??= transform.Find("Progress")?.GetComponent<Rectangle>();
                if (res == null)
                {
                    GameObject g = new GameObject("Progress");
                    res = g.AddComponent<Rectangle>();
                    res.transform.SetParent(transform);
                    res.transform.localScale = Vector3.one;
                    res.transform.localRotation = Quaternion.identity;
                    res.transform.localPosition = new Vector3(0, 0, -.01f);
                    res.Width = size.x;
                    res.Height = 8.5f;
                    res.BlendMode = ShapesBlendMode.Opaque;
                    //anchor to bottom
                    res.Type = Rectangle.RectangleType.RoundedSolid;
                    res.CornerRadius = 4;
                    res.Color = BackgroundColor.Lighten();
                    RectTransform rt = g.GetOrAddComponent<RectTransform>();
                    //SetRectTransformToFillAtCentre(g.GetOrAddComponent<RectTransform>());
                    //pos =-9
                    // anchor to bototm
                    // rt.anchorMin = new Vector2(0, 0);
                    // rt.anchorMax = new Vector2(1, 0);
                    // rt.pivot = new Vector2(.5f, 0);
                    // rt.offsetMin = new Vector2(0, -9);
                    // rt.offsetMax = new Vector2(0, -9);
                    rt.sizeDelta = new Vector2(0, 7f);
                    g.AddComponent<ShapeConformsToRectTransform>();
                    g.SetActive(holdToAccept);
                    //NEW

                    //stretch across the top
                    //anchor to top
                    rt.anchorMin = new Vector2(0, 1);
                    rt.anchorMax = new Vector2(1, 1);
                    rt.pivot = new Vector2(.5f, 1);
                    //move y down by half od sizedelta
                    float y = 7;
                    rt.localPosition = new Vector3(0, 0, -.1f);
                    rt.offsetMin = new Vector2(0, -y / 2f);
                    rt.offsetMax = new Vector2(0, -y / 2f);
                    rt.sizeDelta = new Vector2(0, y);
                    res.BlendMode = ShapesBlendMode.Transparent;
                    res.Color = Color.white.WithAlpha(.5f);
                    float rad = 7;
                    res.CornerRadiusMode = Rectangle.RectangleCornerRadiusMode.PerCorner;
                    res.CornerRadii = new Vector4(0, rad, rad, 0);

                }
                progressObj = res;
                return res;
            }
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
                    res.BlendMode = ShapesBlendMode.Opaque;
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
                    res.BlendMode = ShapesBlendMode.Opaque;
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
                backgroundColor = value;
                Background.Color = value;
                ProgressObj.GetComponent<Rectangle>().Color = value.Lighten();
                BackgroundShadow.Color = DarkerBG.Darken();
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

            ProgressObj?.gameObject.SetActive(holdToAccept);
            if (KeyTextObj != null && KeyTextObj.enabled)
            {
                KeyTextObj?.gameObject.SetActive(holdToAccept && showKey && holdKey.Length > 0);
                KeyTextObj?.SetText(holdKey?.ToUpper());

                //offset x -40
                KeyTextObj.GetComponent<RectTransform>().offsetMin = new Vector2(-60, 0);
                KeyTextObj.GetComponent<RectTransform>().offsetMax = new Vector2(-60, 0);
                KeyTextObj.GetComponent<RectTransform>().sizeDelta = Vector2.one * 40;
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
            //BackgroundShadow.Color = flat ? BackgroundColor.Darken() : DarkerBG.Darken();
            rect.UpdateMesh();
            //update shapes
            rect.meshOutOfDate = true;
            rect.OnValidate();
            //repaint scene
#if UNITY_EDITOR
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#endif
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
        void AnchorToBottomOfParent(RectTransform rt)
        {
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 0);
            rt.pivot = new Vector2(.5f, 0);
            rt.offsetMin = new Vector2(0, 0);
            rt.offsetMax = new Vector2(0, 0);
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
        bool isPreppedFromStart = false;
        void Start()
        {
            Prepare();
        }
        void Prepare()
        {
            if (isPreppedFromStart) { return; }
            isPreppedFromStart = true;
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
            if (disabled)
            {
                Disable(true);
            }


            SetColors();
            Refresh();

        }
        void Update()
        {
            if (!Application.isPlaying) { return; }
            if (disabled) { return; }
            Prepare();
            if (holdToAccept)
            {
                if (canCompleteHoldClick)
                {
                    if (holdProgress >= 1)
                    {
                        OnClicked();
                        canCompleteHoldClick = false;
                    }

                }
                Progress += Time.deltaTime * holdDir / holdDuration;
                Progress = Mathf.Clamp01(holdProgress);
                if (Input.GetMouseButtonUp(0))
                {
                    holdDir = -holdCollapseSpeed;
                    canCompleteHoldClick = true;
                }
                //is keycode string down
                if (holdKey.Length > 0)
                {
                    if (Input.GetKeyDown(holdKey))
                    {
                        holdDir = 1;
                    }
                    if (Input.GetKeyUp(holdKey))
                    {
                        holdDir = -holdCollapseSpeed;
                        Progress -= .0001f;
                        canCompleteHoldClick = true;
                    }
                }
            }
            if (!allowHovering) { return; }

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

        float Progress
        {
            get => holdProgress;
            set
            {
                holdProgress = value;
                //ProgressObj.Width = size.x * holdProgress;
                float newWidth = size.x * holdProgress;
                float height = 7;
                float half = height / 2f;
                RectTransform rt = ProgressObj.GetComponent<RectTransform>();
                //anchor to top 
                rt.anchorMin = new Vector2(0.5f, 1);
                rt.anchorMax = new Vector2(0.5f, 1);
                rt.pivot = new Vector2(.5f, 1);

                float leftX = -size.x / 2f;
                float centreX = leftX + newWidth / 2f;
                //move y down by half od sizedelta posy = -half
                //rt.localPosition = new Vector3(centreX, -size.y, -.1f);
                rt.offsetMin = new Vector2(-newWidth / 2f, -half);
                rt.offsetMax = new Vector2(newWidth / 2f, -half);
                rt.sizeDelta = new Vector2(newWidth, height);
                rt.localPosition = new Vector3(centreX, (size.y / 2f) - (height / 2f), -.1f);


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
            if (disabled || holdToAccept) { return; }
            OnClicked();
        }

        bool allowHovering = true;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (disabled) { return; }
            holdDir = 1;
            if (hoverType == UiHoverType.Tint)
            {
                allowHovering = false;
                isHovering = false;
            }
            BackgroundColor = beforeHoverColor.Darken();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            canCompleteHoldClick = true;
            if (disabled) { return; }
            holdDir = -holdCollapseSpeed;
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
            // Debug.Log("Clicked");
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
            if (!Application.isPlaying) { return; }
            Prepare();
            disabled = dis;
            int v = dis ? 0 : 1;
            BackgroundColor = Color.Lerp(disableColor, beforeHoverColor, v);
            SetColors();
            TextColor = Color.Lerp(disableColor.Lighten(.5f), beforeTextColor, v);
            OnDisableChangeEvent.Invoke(dis);
        }
    }
}
#endif