using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dutil
{
    public static class VectorExtensions
    {

        //Vector 3
        public static Vector2 XY(this Vector3 vec) => new Vector2(vec.x, vec.y);
        public static Vector2 XZ(this Vector3 vec) => new Vector2(vec.x, vec.z);
        public static Vector2 YZ(this Vector3 vec) => new Vector2(vec.y, vec.z);
        public static Vector3 WithX(this Vector3 vec, float x) => new Vector3(x, vec.y, vec.z);
        public static Vector3 WithY(this Vector3 vec, float y) => new Vector3(vec.x, y, vec.z);
        public static Vector3 WithZ(this Vector3 vec, float z) => new Vector3(vec.x, vec.y, z);
        public static Vector3 SetX(this ref Vector3 vec, float x) { vec.x = x; return vec; }
        public static Vector3 SetY(this ref Vector3 vec, float y) { vec.y = y; return vec; }
        public static Vector3 SetZ(this ref Vector3 vec, float z) { vec.z = z; return vec; }
        public static Vector3 OffsetX(this Vector3 vec, float x) => new Vector3(vec.x + x, vec.y, vec.z);
        public static Vector3 OffsetY(this Vector3 vec, float y) => new Vector3(vec.x, vec.y + y, vec.z);
        public static Vector3 OffsetZ(this Vector3 vec, float z) => new Vector3(vec.x, vec.y, vec.z + z);
        public static Vector3 Confine(this Vector3 vec, Vector3 BL, Vector3 TR)
        {
            float x = Mathf.Min(Mathf.Max(vec.x, BL.x), TR.x);
            float y = Mathf.Min(Mathf.Max(vec.y, BL.y), TR.y);
            float z = Mathf.Min(Mathf.Max(vec.z, BL.z), TR.z);
            return new Vector3(x, y, z);
        }
        public static Vector3Int Round(this Vector3 vec) => new Vector3Int(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y), Mathf.RoundToInt(vec.z));


        //Vector 2
        public static Vector3 XY(this Vector2 vec, float z = 0) => new Vector3(vec.x, vec.y, z);
        public static Vector3 XZ(this Vector2 vec, float y = 0) => new Vector3(vec.x, y, vec.y);
        public static Vector3 YZ(this Vector2 vec, float x = 0) => new Vector3(x, vec.x, vec.y);
        public static Vector2 WithX(this Vector2 vec, float x) => new Vector2(x, vec.y);
        public static Vector2 WithY(this Vector2 vec, float y) => new Vector2(vec.x, y);
        public static Vector2 SetX(this ref Vector2 vec, float x) { vec.x = x; return vec; }
        public static Vector2 SetY(this ref Vector2 vec, float y) { vec.y = y; return vec; }
        public static Vector2 OffsetX(this Vector2 vec, float x) => new Vector2(vec.x + x, vec.y);
        public static Vector2 OffsetY(this Vector2 vec, float y) => new Vector2(vec.x, vec.y + y);
        public static Vector2 GetRight(this Vector2 vec) => new Vector2(vec.y, -vec.x);
        public static Vector2 GetLeft(this Vector2 vec) => vec.GetRight() * -1;
        public static float RandomFromRange(this Vector2 vec) => Random.Range(vec.x, vec.y);
        public static Vector2 Confine(this Vector2 vec, Vector2 BL, Vector2 TR)
        {
            float x = Mathf.Min(Mathf.Max(vec.x, BL.x), TR.x);
            float y = Mathf.Min(Mathf.Max(vec.y, BL.y), TR.y);
            return new Vector2(x, y);
        }
        public static Vector2Int Round(this Vector2 vec) => new Vector2Int(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y));
        /// <summary>
        /// It returns the angle of a vector in degrees
        /// </summary>
        /// <param name="Vector2">The vector you want to get the angle of.</param>
        public static float Angle(this Vector2 vec) => (360 - Vector2.SignedAngle(Vector2.up, vec)) % 360;
        /// <summary>
        /// It rotates a vector by a given angle around the Z-axis
        /// </summary>
        /// <param name="Vector2">The vector you want to rotate.</param>
        /// <param name="angle">The angle to rotate the vector by.</param>
        public static Vector2 Rotate(this Vector2 vec, float angle) => Quaternion.Euler(0, 0, -angle) * vec;


        //Vector2Int
        public static Vector2 ToVector2(this Vector2Int vec) => new Vector2(vec.x, vec.y);
        public static Vector3 ToVector3(this Vector2Int vec) => new Vector3(vec.x, vec.y, 0);
        public static Vector3Int XY(this Vector2Int vec, int z = 0) => new Vector3Int(vec.x, vec.y, z);
        /// <summary>
        /// It returns the angle of a vector in degrees
        /// </summary>
        /// <param name="Vector2">The vector you want to get the angle of.</param>
        public static float Angle(this Vector2Int vec) => (360 - Vector2.SignedAngle(Vector2.up, vec)) % 360;

        //Vector3Int
        public static Vector2Int XY(this Vector3Int vec) => new Vector2Int(vec.x, vec.y);
        public static Vector2Int XZ(this Vector3Int vec) => new Vector2Int(vec.x, vec.z);
        public static Vector2Int YZ(this Vector3Int vec) => new Vector2Int(vec.y, vec.z);
        public static Vector3 ToVector3(this Vector3Int vec) => new Vector3(vec.x, vec.y, vec.z);
    }
}