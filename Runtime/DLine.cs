using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public List<Vector3> points;

    int animationDir = 1;
    bool animate = false;
    float progress = 0;
    float Progress { get { return progress; } set { progress = Mathf.Clamp01(value); } }
    void Start()
    {
        CreateRenderer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!animate) { return; }
        Animate();
    }
    LineRenderer CreateRenderer()
    {
        if (lineRenderer != null) { lineRenderer; }

        GameObject g = new GameObject("Renderer");
        lineRenderer = g.AddComponent<LineRenderer>();
        g.transform.SetParent(transform);

        lineRenderer.numCapVertices = 12;
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.magenta;
        lineRenderer.startWidth = lineRenderer.endWidth = .3f;
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.receiveShadows = false;
        return lineRenderer;
    }
    public void Setup(List<Vector3> _points)
    {
        points = _points;
    }
    public void ShowInstantly()
    {
        animate = false;
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
    public void HideInstantly()
    {
        animate = false;
        lineRenderer.positionCount = 0;
    }

    public void ShowAnimated()
    {
        animate = true;
        animationDir = 1;
    }
    public void HideAnimated()
    {
        animate = true;
        animationDir = -1;
    }
    void Animate()
    {
        Progress += Time.deltaTime;
        int progressCount = Mathf.RoundToInt(Mathf.Lerp(0, points.Count, progress));
        List<Vector3> relativePoints = points.GetRange(0, progressCount);
        lineRenderer.positionCount = progressCount;
        lineRenderer.SetPositions(relativePoints.ToArray());
        if ((animationDir == 1 && Progress == 1) || (animationDir == -1 && Progress == 0))
        {
            animate = false;
        }
    }
}
