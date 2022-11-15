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
            return new CameraZone(bottomLeft, topLeft, topRight, bottomRight);
        }
        /// <summary>
        /// Looks at all child colliders of the gameobject and checks if any are partially in view of the camera.
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="g"></param>
        /// <param name="padding">Extend the frustrum of the camera. (Not real, for frustrum check only)</param>
        /// <returns></returns>
        public static bool IsObjectPartiallyInView(this Camera cam, GameObject g, float padding = 0.2f)
        {
            Vector3 right = cam.transform.right;
            Vector3 up = cam.transform.up;
            Vector3 fwd = cam.transform.forward;
            List<Collider> cols = g.GetComponentsInChildren<Collider>().ToList();
            if (cols.Count <= 0) { D.Log("No colliders found on object: " + g.name); return false; }
            foreach (Collider col in cols)
            {
                float depthFromCamera = Vector3.Dot(cam.transform.forward, g.transform.position - cam.transform.position);
                Vector3 rightMost = col.bounds.ClosestPoint(cam.transform.position + fwd * depthFromCamera + right * 10) + right * padding;
                Vector3 leftMost = col.bounds.ClosestPoint(cam.transform.position + fwd * depthFromCamera - right * 10) - right * padding;
                Vector3 topMost = col.bounds.ClosestPoint(cam.transform.position + fwd * depthFromCamera + up * 10) + up * padding;
                Vector3 bottomMost = col.bounds.ClosestPoint(cam.transform.position + fwd * depthFromCamera - up * 10) - up * padding;

                List<Vector3> points = new List<Vector3>() { rightMost, leftMost, topMost, bottomMost };
                CameraZone camZone = cam.GetViewAtDistance(depthFromCamera);
                bool isInView = points.Any(x => camZone.IsInBounds(x) && D.LineOfSight(cam.transform.position, x, cols));
                if (isInView) { return true; }
            }

            return false;
        }
        /// <summary>
        /// Looks at all child colliders of the gameobject and checks if they are fully in view of the camera.
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="g"></param>
        /// <param name="padding">Extend the frustrum of the camera. (Not real, for frustrum check only)</param>
        /// <returns></returns>
        public static bool IsObjectFullyInView(this Camera cam, GameObject g, float padding = 0)
        {
            Vector3 right = cam.transform.right;
            Vector3 up = cam.transform.up;
            Vector3 fwd = cam.transform.forward;
            List<Collider> cols = g.GetComponentsInChildren<Collider>().ToList();
            if (cols.Count <= 0) { D.Log("No colliders found on object: " + g.name); return false; }
            foreach (Collider col in cols)
            {
                float depthFromCamera = Vector3.Dot(cam.transform.forward, g.transform.position - cam.transform.position);
                Vector3 rightMost = col.bounds.ClosestPoint(cam.transform.position + fwd * depthFromCamera + right * 10) + right * padding;
                Vector3 leftMost = col.bounds.ClosestPoint(cam.transform.position + fwd * depthFromCamera - right * 10) - right * padding;
                Vector3 topMost = col.bounds.ClosestPoint(cam.transform.position + fwd * depthFromCamera + up * 10) + up * padding;
                Vector3 bottomMost = col.bounds.ClosestPoint(cam.transform.position + fwd * depthFromCamera - up * 10) - up * padding;

                List<Vector3> points = new List<Vector3>() { rightMost, leftMost, topMost, bottomMost };
                CameraZone camZone = cam.GetViewAtDistance(depthFromCamera);
                bool isInView = points.TrueForAll(x => camZone.IsInBounds(x) && D.LineOfSight(cam.transform.position, x, cols));
                if (!isInView) { return false; }

            }
            return true;
        }
    }
}