using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using System;

namespace Dutil
{
    public static class BetterGizmos
    {
        public static void DrawLine(this List<Vector3> points)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
        }
        public static void DrawLine(this List<Vector3> points, Color color)
        {
            Gizmos.color = color;
            DrawLine(points);
        }
        public static void DrawLine(this List<Vector3> points, Color startColor, Color endColor)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                Gizmos.color = Color.Lerp(startColor, endColor, (float)i / (points.Count - 1));
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
        }
        public static void DrawCircle(this Vector3 center, Vector3 normal, float radius, float thickness = .1f, float progress = 1)
        {
            List<Vector3> points = new List<Vector3>();
            int numOfPoints = (int)radius.Map(0, 10, 360, 360 * 3);
            progress = Mathf.Clamp01(progress);
            int pointsToDraw = (int)(numOfPoints * progress);
            for (int i = 0; i <= pointsToDraw; i++)
            {
                float incr = 1 / (float)numOfPoints * 360;
                float angle = i * incr;
                points.Add(center + Quaternion.AngleAxis(angle, normal) * Vector3.up * radius);
            }
            if (progress == 1)
            {
                float incr = 1 / (float)numOfPoints * 360;
                float angle = 1 * incr;
                points.Add(center + Quaternion.AngleAxis(0, normal) * Vector3.up * radius);
            }

            DrawLine(points, Colours.Orange, thickness, progress == 1);
        }


        public static void DrawLine(this List<Vector3> points, Color fromColor, float thickness, bool closeLoop)
        {
            if (points.Count < 2)
            {
                return;
            }
            //increase resolution
            List<Vector3> newPoints = new List<Vector3>();
            for (int i = 0; i < points.Count;)
            {
                newPoints.Add(points[i]);
                if (i < points.Count - 1)
                {
                    Vector3 dir = points[i + 1] - points[i];
                    float dist = dir.magnitude;
                    int count = Mathf.CeilToInt(dist / thickness);
                    for (int j = 1; j < count; j++)
                    {
                        newPoints.Add(points[i] + dir.normalized * j * thickness);
                    }
                }
                i++;
            }
            // //last point
            // Vector3 dir2 = points[points.Count - 1] - points[points.Count - 2];
            // float dist2 = dir2.magnitude;
            // int count2 = Mathf.CeilToInt(dist2 / thickness);
            // for (int j = 1; j < count2; j++)
            // {
            //     newPoints.Add(points[points.Count - 1] + dir2.normalized * j * thickness);
            // }

            if (closeLoop)
            {
                newPoints.Add(points[0]);
            }
            points = newPoints;
            List<Vector3> outerPoints = new List<Vector3>();
            Mesh mesh = new Mesh();
            List<int> triangles = new List<int>();
            List<Vector3> vertices = new List<Vector3>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector3 dir = points[i + 1] - points[i];
                Vector3 perp = Vector3.Cross(Camera.main.transform.forward, dir).normalized * thickness * .5f;
                vertices.Add(points[i] + perp);
                vertices.Add(points[i] - perp);
            }
            for (int i = 0; i < vertices.Count - 2; i += 2)
            {
                triangles.Add(i);
                triangles.Add(i + 2);
                triangles.Add(i + 1);
                triangles.Add(i + 1);
                triangles.Add(i + 2);
                triangles.Add(i + 3);
            }
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            Gizmos.color = fromColor;
            mesh.RecalculateNormals();

            Gizmos.DrawMesh(mesh, Vector3.zero);


        }

    }
}