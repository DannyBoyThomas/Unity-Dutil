using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using Unity.Collections;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class GradientBackground : MonoBehaviour
{
    public Color startColor = Colours.Cyan;
    public Color endColor = Colours.Blue;
    [Range(1, 360)]
    public int direction = 250;


    //public Gradient gradient;
    GameObject quadObj;
    Camera cam;
    void Start()
    {
        GameObject c = Quad;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnValidate()
    {
        Refresh();

    }
    [ContextMenu("Refresh")]
    public void Refresh()
    {
        /*  gradient = new Gradient();
         gradient.colorKeys = new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 1.0f) };
         gradient.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }; */
        Quad.transform.localScale = new Vector2(GetWidth(), GetHeight());
        quadObj.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_ColOne", startColor);
        quadObj.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_ColTwo", endColor);

        float sinAngle = Mathf.Sin(direction * Mathf.Deg2Rad);//.Map(-1, 1, 0, 1);
        float cosAngle = Mathf.Cos(direction * Mathf.Deg2Rad);//.Map(-1, 1, 0, 1);
        quadObj.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Sin", sinAngle);
        quadObj.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Cos", cosAngle);

    }
    GameObject Quad
    {
        get
        {
            if (quadObj == null)
            {
                quadObj = Cam.transform.Find("Gradient Background")?.gameObject ?? null;
                if (quadObj != null) { return quadObj; }
                quadObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quadObj.name = "Gradient Background";
                quadObj.transform.SetParent(Cam.transform);
                quadObj.transform.position = Cam.transform.position + Cam.transform.forward * 500;
                quadObj.transform.localRotation = Quaternion.identity;
                //set material

                quadObj.GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Dutil/Gradient"));
                quadObj.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                quadObj.GetComponent<MeshRenderer>().receiveShadows = false;
                //Set gradient colors
                endColor = Colours.Blue.Shade(6);
                quadObj.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_ColOne", startColor);
                quadObj.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_ColTwo", endColor);

            }
            return quadObj;
        }
    }
    Camera Cam
    {
        get
        {
            if (cam == null)
            {
                cam = GetComponent<Camera>();
            }
            return cam;
        }
    }
    float GetHeight()
    {
        Vector3 pos = Cam.ViewportToWorldPoint(new Vector3(0.5f, 1, 510));
        Vector3 offset = pos - Cam.transform.position;
        Vector3 localOffset = Cam.transform.InverseTransformDirection(offset);
        return (localOffset.y) * 2;
    }
    float GetWidth()
    {
        Vector3 pos = Cam.ViewportToWorldPoint(new Vector3(1, 0.5f, 510));
        Vector3 offset = pos - Cam.transform.position;
        Vector3 localOffset = Cam.transform.InverseTransformDirection(offset);
        return (localOffset.x) * 2;
    }
    void OnDestroy()
    {
        if (Quad != null)
        {
            Quad.EasyDestroy();
        }
    }
    public void TransitionTo(Color startCol, Color endCol, float time = 1.4f)
    {
        AutoLerp.Begin(time, startColor, startCol, (t, v) =>
        {
            startColor = v;
        }, true);
        AutoLerp.Begin(time, endColor, endCol, (t, v) =>
        {
            endColor = v;
            Refresh();
        }, true);

    }

}