using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dutil
{
    public enum DeathType { ScaleDown, ScaleDownX, ScaleDownY, ScaleDownZ };
    public enum Direction2D { Up, Right, Down, Left };
    public enum Direction3D { Up, Right, Down, Left, Forward, Backward };
    public class DStructs
    {

    }
    public struct ArrayItem<T>
    {
        public int index;
        public T value;
        public void Print()
        {
            Debug.Log(index + ": " + value);
        }
    }
    public struct Array2DItem<T>
    {
        public Vector2Int index;
        public T value;
        public void Print()
        {
            Debug.Log(index + ": " + value);
        }
    }
    public struct CameraZone
    {
        Vector3 BottomLeft, TopRight, TopLeft, BottomRight;
        float distance;
        public CameraZone(Vector3 bottomLeft, Vector3 topLeft, Vector3 topRight, Vector3 bottomRight, float depth = 10)
        {
            this.BottomLeft = bottomLeft;
            this.TopRight = topRight;
            this.TopLeft = topLeft;
            this.BottomRight = bottomRight;
            distance = depth;
        }
        public CameraZone WithPadding(int pixels)
        {
            Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(pixels, pixels, distance));
            Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - pixels, Screen.height - pixels, distance));
            Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(pixels, Screen.height - pixels, distance));
            Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - pixels, pixels, distance));
            return new CameraZone(bottomLeft, topLeft, topRight, bottomRight);
        }
        public CameraZone WithPadding(int pixels, float depth)
        {
            Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(pixels, pixels, depth));
            Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - pixels, Screen.height - pixels, depth));
            Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(pixels, Screen.height - pixels, depth));
            Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - pixels, pixels, depth));
            return new CameraZone(bottomLeft, topLeft, topRight, bottomRight);
        }
        public Vector3 Centre
        {
            get
            {
                return (BottomLeft + TopRight) / 2;
            }
        }
        public Vector3 Size
        {
            get
            {
                return TopRight - BottomLeft;
            }
        }
        public float Height { get { return TopRight.y - TopLeft.y; } }
        public float Width { get { return TopRight.x - TopLeft.x; } }
        public Vector3 Left { get { return (TopLeft + BottomLeft) / 2; } }
        public Vector3 Right { get { return (TopRight + BottomRight) / 2; } }

        public Vector3 Top { get { return (TopRight + TopLeft) / 2; } }
        public Vector3 Bottom { get { return (BottomRight + BottomLeft) / 2; } }

        public bool IsInBounds(Vector3 point)
        {
            return IsBetweenTwoValues(point.x, BottomLeft.x, TopRight.x) && IsBetweenTwoValues(point.y, BottomLeft.y, TopRight.y);

        }
        bool IsBetweenTwoValues(float value, float value1, float value2)
        {

            return value >= Mathf.Min(value1, value2) && value <= Mathf.Max(value1, value2);
        }
        float ConvertPixelsToMetersAtDistance(float pixels, float distance)
        {
            return pixels * (distance / Camera.main.pixelHeight);
        }

        public void DrawWithGizmos()
        {
            if (BottomLeft == null || TopRight == null)
            {
                return;
            }
            List<Vector3> points = new List<Vector3>(){
                BottomLeft,
                TopLeft,
                TopRight,
                BottomRight
            };
            points.DrawWithGizmos(true);
            float sphereSize = .1f;
            Gizmos.DrawSphere(BottomLeft, sphereSize);
            Gizmos.DrawSphere(TopRight, sphereSize);
            Gizmos.DrawSphere(BottomRight, sphereSize);
            Gizmos.DrawSphere(TopLeft, sphereSize);
            Gizmos.DrawSphere(Left, sphereSize);
            Gizmos.DrawSphere(Right, sphereSize);
            Gizmos.DrawSphere(Top, sphereSize);
            Gizmos.DrawSphere(Bottom, sphereSize);


        }


    }

}