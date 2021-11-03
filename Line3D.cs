using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dutil
{
    public class Line3D
    {
        Vector3 pointA, pointB;

        public Line3D(Vector3 startPoint, Vector3 endPoint)
        {
            pointA = startPoint;
            pointB = endPoint;
        }
        public Vector3 Lerp(float t)
        {
            return Vector3.Lerp(pointA, pointB, t);
        }
        public Vector3 NearestPoint(Vector3 point)
        {
            Vector3 AP = point - pointA;
            Vector3 AB = pointB - pointA;

            float ABAPproduct = Vector3.Dot(AP, AB);
            float distance = ABAPproduct / AB.magnitude;

            return distance < 0 ? pointA : distance > 1 ? pointB : point + AB * distance;
        }
        public void Draw()
        {
            Gizmos.DrawLine(pointA, pointB);
        }
    }
}