using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dutil
{
    //test
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
    }
}