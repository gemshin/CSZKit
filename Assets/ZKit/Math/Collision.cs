using UnityEngine;
using System;
using System.Collections;

namespace ZKit.Math
{
    public static class Collision2D
    {
        /// <summary>
        /// 두 선(a, b)의 교차 여부를 구한다.
        /// </summary>
        /// <returns>교차여부</returns>
        public static bool Line(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            Vector2 a = a2 - a1;
            Vector2 b = b2 - b1;
            float aDotbPerp = a.x * b.y - a.y * b.x;

            if (aDotbPerp == 0) // 수평.
                return false;

            Vector2 c = b1 - a1;
            float t = (c.x * b.y - c.y * b.x) / aDotbPerp;
            if (t < 0 || t > 1)
                return false;

            float u = (c.x * a.y - c.y * a.x) / aDotbPerp;
            if (u < 0 || u > 1)
                return false;

            return true;
        }

        /// <summary>
        /// 두 선(a, b)의 교차 여부와 교차지점을 구한다.
        /// </summary>
        /// <param name="intersection">(out) 교차지점</param>
        /// <returns>교차여부</returns>
        public static bool Line(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            Vector2 a = a2 - a1;
            Vector2 b = b2 - b1;
            float aDotbPerp = a.x * b.y - a.y * b.x;

            if (aDotbPerp == 0) // 수평.
                return false;

            Vector2 c = b1 - a1;
            float t = (c.x * b.y - c.y * b.x) / aDotbPerp;
            if (t < 0 || t > 1)
                return false;

            float u = (c.x * a.y - c.y * a.x) / aDotbPerp;
            if (u < 0 || u > 1)
                return false;

            intersection = a1 + t * a;

            return true;
        }
    }

    public static class Collision3D
    {

    }
}