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

        public static bool LineRay(Vector2 a1, Vector2 a2, Vector2 rayOrigin, Vector2 rayDirection)
        {
            // 버그가 있다. ray 뒤로도 충돌이 가능하다.. 고치자.
            var v1 = rayOrigin - a1;
            var v2 = a2 - a1;
            var v3 = new Vector2(-rayDirection.y, rayDirection.x); // cw90

            var dot = Vector2.Dot(v2, v3);
            if (Mathf.Abs(dot) < float.Epsilon) // 수평
                return false;

            var t1 = Mathf.Abs(v2.x * v1.y) - (v2.y * v1.x) / dot;// Vector.CrossProduct(v2, v1) / dot;
            var t2 = Vector3.Dot(v1, v3) / dot;

            //Debug.Log(t1.ToString() + "    " + t2.ToString());

            if (t1 >= 0f && (t2 >= 0f && t2 <= 1f))
                return true;

            return false;
        }

        public static bool LineBox(Vector2 a1, Vector2 a2, Vector2 boxCenter, Vector2 size, float rotateAngle)
        {
            Vector2 boxSpaceStart = a1 - boxCenter;
            Vector2 boxSpaceEnd = a2 - boxCenter;

            float rad = rotateAngle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            Vector2 originStart = new Vector2();
            Vector2 originEnd = new Vector2();
            originStart.x = (boxSpaceStart.x * cos) + (boxSpaceStart.y * -sin);
            originStart.y = (boxSpaceStart.x * sin) + (boxSpaceStart.y * cos);
            originEnd.x = (boxSpaceEnd.x * cos) + (boxSpaceEnd.y * -sin);
            originEnd.y = (boxSpaceEnd.x * sin) + (boxSpaceEnd.y * cos);

            float halfWidth = size.x * 0.5f;
            float halfHeight = size.y * 0.5f;

            int startHorz = 0, startVert = 0;
            if (Mathf.Abs(originStart.x) <= halfWidth) startHorz = 0;
            else startHorz = (originStart.x > 0f) ? 1 : -1;
            if (Mathf.Abs(originStart.y) <= halfHeight) startVert = 0;
            else startVert = (originStart.y > 0f) ? 1 : -1;
            int endHorz = 0, endVert = 0;
            if (Mathf.Abs(originEnd.x) <= halfWidth) endHorz = 0;
            else endHorz = (originEnd.x > 0f) ? 1 : -1;
            if (Mathf.Abs(originEnd.y) <= halfHeight) endVert = 0;
            else endVert = (originEnd.y > 0f) ? 1 : -1;

            if ((startHorz == endHorz) && (startVert == endVert)) return false; // 같은편
            if ((startHorz == 0 && startVert == 0) || (endHorz == 0 && endVert == 0)) return true; // 박스안

            if ((startHorz == 0) && (endHorz == 0) && (startVert * endVert < 0)) return true; // 수평 다른편
            if ((startVert == 0) && (endVert == 0) && (startHorz * endHorz < 0)) return true; // 수직 다른편

            if (startHorz > 0)
            {
                if (startVert > 0) // ++  rt / lt, rb
                {
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, -halfHeight), originEnd) > 0f) return false;
                }
                else if (startVert < 0) // +-  rb / rt, lb
                {
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, -halfHeight), originEnd) > 0f) return false;
                }
                else // +0  rc / rt, rb
                {
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, -halfHeight), originEnd) > 0f) return false;
                }
            }
            else if (startHorz < 0)
            {
                if (startVert > 0) // -+  lt / lb, rt
                {
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, -halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, halfHeight), originEnd) > 0f) return false;
                }
                else if (startVert < 0) // --  lb / rb, lt
                {
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, -halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, halfHeight), originEnd) > 0f) return false;
                }
                else // -0  lc / lb, lt
                {
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, -halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, halfHeight), originEnd) > 0f) return false;
                }
            }
            else //if (startHorz == 0)
            {
                if (startVert > 0) // 0+  ct / lt, rt
                {
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, halfHeight), originEnd) > 0f) return false;
                }
                else if (startVert < 0) // 0-  cb / rb, lb   //else { } // 00
                {
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, -halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, -halfHeight), originEnd) > 0f) return false;
                }
            }
            return true;
        }

        public static bool RayBox(Vector2 rayStart, Vector2 direction, Vector2 boxCenter, Vector2 size, float rotateAngle)
        {
            Vector2 boxSpaceStart = rayStart - boxCenter;
            Vector2 boxSpaceEnd = direction - boxCenter;

            float rad = rotateAngle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            Vector2 originStart = new Vector2();
            Vector2 originEnd = new Vector2();
            originStart.x = (boxSpaceStart.x * cos) + (boxSpaceStart.y * -sin);
            originStart.y = (boxSpaceStart.x * sin) + (boxSpaceStart.y * cos);
            originEnd.x = (boxSpaceEnd.x * cos) + (boxSpaceEnd.y * -sin);
            originEnd.y = (boxSpaceEnd.x * sin) + (boxSpaceEnd.y * cos);

            float halfWidth = size.x * 0.5f;
            float halfHeight = size.y * 0.5f;

            int startHorz = 0, startVert = 0;
            if (Mathf.Abs(originStart.x) <= halfWidth) startHorz = 0;
            else startHorz = (originStart.x > 0f) ? 1 : -1;
            if (Mathf.Abs(originStart.y) <= halfHeight) startVert = 0;
            else startVert = (originStart.y > 0f) ? 1 : -1;
            int endHorz = 0, endVert = 0;
            if (Mathf.Abs(originEnd.x) <= halfWidth) endHorz = 0;
            else endHorz = (originEnd.x > 0f) ? 1 : -1;
            if (Mathf.Abs(originEnd.y) <= halfHeight) endVert = 0;
            else endVert = (originEnd.y > 0f) ? 1 : -1;

            if ((startHorz == 0 && startVert == 0) || (endHorz == 0 && endVert == 0)) return true; // 박스안
            if ((startHorz == 0) && (endHorz == 0) && (startVert * endVert < 0)) return true; // 수평 다른편
            if ((startVert == 0) && (endVert == 0) && (startHorz * endHorz < 0)) return true; // 수직 다른편

            if (startHorz > 0)
            {
                if (startVert > 0) // ++  rt / lt, rb
                {
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, -halfHeight), originEnd) > 0f) return false;
                }
                else if (startVert < 0) // +-  rb / rt, lb
                {
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, -halfHeight), originEnd) > 0f) return false;
                }
                else // +0  rc / rt, rb
                {
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, -halfHeight), originEnd) > 0f) return false;
                }
            }
            else if (startHorz < 0)
            {
                if (startVert > 0) // -+  lt / lb, rt
                {
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, -halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, halfHeight), originEnd) > 0f) return false;
                }
                else if (startVert < 0) // --  lb / rb, lt
                {
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, -halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, halfHeight), originEnd) > 0f) return false;
                }
                else // -0  lc / lb, lt
                {
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, -halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, halfHeight), originEnd) > 0f) return false;
                }
            }
            else //if (startHorz == 0)
            {
                if (startVert > 0) // 0+  ct / lt, rt
                {
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, halfHeight), originEnd) > 0f) return false;
                }
                else if (startVert < 0) // 0-  cb / rb, lb   //else { } // 00
                {
                    if (Misc.SignedArea(originStart, new Vector2(halfWidth, -halfHeight), originEnd) < 0f) return false;
                    if (Misc.SignedArea(originStart, new Vector2(-halfWidth, -halfHeight), originEnd) > 0f) return false;
                }
            }
            return true;
        }

        public static bool DotCircle(Vector2 dotPosition, Vector2 circlePosition, float radius)
        {
            Vector2 circleSpaceDot = dotPosition - circlePosition;
            if (circleSpaceDot.magnitude <= radius)
                return true;
            return false;
        }

        public static bool LineCircle(Vector2 a1, Vector2 a2, Vector2 circlePosition, float radius)
        {
            Vector2 circleSpaceStart = a1 - circlePosition;
            Vector2 circleSpaceEnd = a2 - circlePosition;

            if (circleSpaceStart.magnitude <= radius) return true;
            if (circleSpaceEnd.magnitude <= radius) return true;

            Vector2 lineDirection = circleSpaceEnd - circleSpaceStart;
            if (Vector2.Dot(circleSpaceStart, lineDirection) >= 0f) return false;
            if (Vector2.Dot(circleSpaceEnd, lineDirection) <= 0f) return false;

            float dLen = lineDirection.magnitude;
            float dotdPerp = circleSpaceStart.x * circleSpaceEnd.y - circleSpaceEnd.x * circleSpaceStart.y;
            float di = (radius * radius) * (dLen * dLen) - (dotdPerp * dotdPerp);

            if (di < 0) return false;
            return true;
        }
    }

    public static class Collision3D
    {

    }
}