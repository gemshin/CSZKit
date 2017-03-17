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
    }
}
