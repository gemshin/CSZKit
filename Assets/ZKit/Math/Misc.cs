using UnityEngine;
using System.Collections;

namespace ZKit.Math
{
    public class Misc
    {
        /// <summary>
        /// 선ab에서 점p의 위치를 찾는다. 양수면 왼쪽, 음수면 오른쪽에 위치
        /// </summary>
        /// <returns>점p는 선ab에 대하여 양수면 왼쪽, 음수면 오른쪽에 위치, 0이면 선위에 위치</returns>
        static public float SignedArea(Vector2 a, Vector2 b, Vector2 p)
        {
            return (b.x - a.x) * (p.y - a.y) - (p.x - a.x) * (b.y - a.y);
        }

        /// <summary>
        /// 원점을 기준으로 한 line에서 점p의 위치를 찾는다. 양수면 왼쪽, 음수면 오른쪽에 위치
        /// </summary>
        /// <param name="line">원점을 기준으로 한 선의 끝점</param>
        /// <returns>점p는 line에 대하여 양수면 왼쪽, 음수면 오른쪽에 위치, 0이면 선위에 위치</returns>
        static public float SignedArea(Vector2 line, Vector2 p)
        {
            return line.x * p.y - line.y * p.x;
        }

        /// <summary>
        /// 두 점 p, q 가 선ab를 기준으로 같은 편에 있으면 양수, 다른 편에 있으면 음수를 반환.
        /// </summary>
        static public float Side(Vector2 p, Vector2 q, Vector2 a, Vector2 b)
        {
            return SignedArea(a, b, p) * SignedArea(a, b, q);
        }
        /// <summary>
        /// 점에서 부터 원의 외접점까지의 선을 구한다.
        /// </summary>
        /// <param name="circlePosition">원의 위치</param>
        /// <param name="circleRadius">원의 반지름</param>
        /// <param name="point">점 좌표</param>
        /// <param name="tangentR">점기준 우측 외접선</param>
        /// <param name="tangentL">점기준 좌측 외접선</param>
        /// <returns>점이 원 안에 있으면 외접선을 구할수 없다 False. 구할수 있다 True</returns>
        public static bool GetExTangentOnCircle(Vector2 circlePosition, float circleRadius, Vector2 point, out Vector2 tangentR, out Vector2 tangentL)
        {
            tangentR = tangentL = Vector2.zero;
            Vector2 pointSpaceCircle = circlePosition - point;
            float len = pointSpaceCircle.magnitude;
            if (len <= circleRadius) return false;
            float tanAng = Mathf.Asin(circleRadius / len);
            float circleAng = Mathf.Atan2(pointSpaceCircle.y, pointSpaceCircle.x);

            tangentR = circlePosition + new Vector2(Mathf.Sin(circleAng - tanAng), -Mathf.Cos(circleAng - tanAng)) * circleRadius;
            tangentL = circlePosition + new Vector2(-Mathf.Sin(circleAng + tanAng), Mathf.Cos(circleAng + tanAng)) * circleRadius;
            return true;
        }
        /// <summary>
        /// 원의 외접선 각도를 구한다. vec(0,1) 기준 각도.
        /// </summary>
        /// <param name="circlePosition">윈의 위치</param>
        /// <param name="circleRadius">원의 반지름</param>
        /// <param name="point">점 좌표</param>
        /// <param name="tanRadR">점기준 우측 외접선 각도(라디안)</param>
        /// <param name="tanRadL">점기준 좌측 외접선 각도(라디안)</param>
        /// <param name="halfLimit"></param>
        /// <returns>점이 원 안에 있으면 외접선을 구할수 없다 False. 구할수 있다 True</returns>
        public static bool GetExTangentAngleOnCircle(Vector2 circlePosition, float circleRadius, Vector2 point, out float tanRadR, out float tanRadL, bool halfLimit = false)
        {
            tanRadR = tanRadL = 0f;
            Vector2 pointSpaceCircle = circlePosition - point;
            float len = pointSpaceCircle.magnitude;
            if (len <= circleRadius) return false;
            float tanAng = Mathf.Asin(circleRadius / len);
            float circleAng = Mathf.Atan2(pointSpaceCircle.x, pointSpaceCircle.y);

            tanRadR = circleAng + tanAng;
            tanRadL = circleAng - tanAng;

            if (tanRadR < 0f) tanRadR += 2f * Mathf.PI;
            if (tanRadL < 0f) tanRadL += 2f * Mathf.PI;

            if (halfLimit)
            {
                tanRadR = tanRadR > Mathf.PI ? -(2f * Mathf.PI - tanRadR) : tanRadR;
                tanRadL = tanRadL > Mathf.PI ? -(2f * Mathf.PI - tanRadL) : tanRadL;
            }
            return true;
        }

        /// <summary>
        /// center를 기준으로 target 좌표를 바꾼다.
        /// </summary>
        /// <param name="center">중점이 될 좌표.</param>
        /// <param name="centerRotateAngle">중점의 회전값</param>
        /// <param name="target">변환할 대상 좌표.</param>
        /// <returns></returns>
        public static Vector2 TransformCoord(Vector2 center, float centerRotateAngle, Vector2 target)
        {
            Vector2 newTarget = target - center;

            float rad = centerRotateAngle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            float x = (newTarget.x * cos) + (newTarget.y * -sin);
            float y = (newTarget.x * sin) + (newTarget.y * cos);
            newTarget.x = x;
            newTarget.y = y;

            return newTarget;
        }
    }
}
