using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dutil
{

    public static class CameraExtensions
    {
        /// <summary>
        /// It returns the bounding box of the viewport in world space.
        /// </summary>
        /// <param name="Camera">The camera you want to get the view of.</param>
        /// <param name="distance">The distance from the camera to the point you want to get the view
        /// of.</param>
        public static CameraZone GetViewAtDistance(this Camera cam, float distance)
        {

            Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, distance));
            Vector3 bottomRight = cam.ViewportToWorldPoint(new Vector3(1, 0, distance));
            Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, distance));
            Vector3 topLeft = cam.ViewportToWorldPoint(new Vector3(0, 1, distance));
            return new CameraZone(bottomLeft, topLeft, topRight, bottomRight, distance);
        }
        /// <summary>
        /// Looks at all child colliders of the gameobject and checks if any are partially in view of the camera.
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="g"></param>
        /// <param name="padding">Extend the frustrum of the camera. Pixels (Not real, for frustrum check only)</param>
        /// <returns></returns>
        public static bool IsObjectPartiallyInView(this Camera cam, GameObject g, int padding = 20)
        {

            List<Collider> cols = g.GetComponentsInChildren<Collider>().ToList();
            if (cols.Count <= 0) { D.Log("No colliders found on object: " + g.name); return false; }
            foreach (Collider col in cols)
            {
                Vector3 closest = col.bounds.ClosestPoint(cam.transform.position);
                List<Vector3> outerPoints = D.PointsOnSphere(12, 10).Select(x => x + col.transform.position).ToList();
                List<Vector3> points = outerPoints.Select(x => col.bounds.ClosestPoint(x)).ToList();
                bool isInView = points.Any(x => cam.IsInBounds(x, padding) && D.LineOfSight(cam.transform.position, x, cols));
                if (isInView) { return true; }
            }

            return false;
        }
        public static bool IsInBounds(this Camera cam, Vector3 point, int paddingInPixels = 0)
        {
            Vector3 screenPoint = cam.WorldToScreenPoint(point);
            if (screenPoint.z < 0) { return false; }
            if (screenPoint.x < 0 - paddingInPixels || screenPoint.x > Screen.width + paddingInPixels) { return false; }
            if (screenPoint.y < 0 - paddingInPixels || screenPoint.y > Screen.height + paddingInPixels) { return false; }
            return true;
        }
        /// <summary>
        /// Looks at all child colliders of the gameobject and checks if they are fully in view of the camera.
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="g"></param>
        /// <param name="padding">Extend the frustrum of the camera. (Not real, for frustrum check only)</param>
        /// <returns></returns>
        public static bool IsObjectFullyInView(this Camera cam, GameObject g, int padding = 0)
        {

            List<Collider> cols = g.GetComponentsInChildren<Collider>().ToList();
            if (cols.Count <= 0) { D.Log("No colliders found on object: " + g.name); return false; }
            foreach (Collider col in cols)
            {
                List<Vector3> outerPoints = D.PointsOnSphere(12, 10).Select(x => x + col.transform.position).ToList();
                List<Vector3> points = outerPoints.Select(x => col.bounds.ClosestPoint(x)).ToList();
                bool isInView = points.TrueForAll(x => cam.IsInBounds(x, padding) && D.LineOfSight(cam.transform.position, x, cols));
                if (!isInView) { return false; }

            }
            return true;
        }
    }
}