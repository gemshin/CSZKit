using UnityEngine;

public static partial class UnityExtensions
{
    public static Vector2 NoYAxis(this Vector3 p) { return new Vector2(p.x, p.z); }

    //public static Vector2 ToVec2(this Point p)
    //{
    //    return new Vector2(p.x, p.y);
    //}

    //public static Vector3 ToVec3(this Point p, float centerValue = 0)
    //{
    //    return new Vector3(p.x, centerValue, p.y);
    //}

    public static void FromString(this Vector2 t, string s)
    {
        string[] tmp = s.Replace(" ", "").Split(',');
        t.x = float.Parse(tmp[0]);
        t.y = float.Parse(tmp[1]);
    }
    public static void FromString(this Vector3 t, string s)
    {
        string[] tmp = s.Replace(" ", "").Split(',');
        t.x = float.Parse(tmp[0]);
        t.y = float.Parse(tmp[1]);
        t.z = float.Parse(tmp[2]);
    }

    public static string ToStringNoBracket(this Vector2 v) { return v.ToString("F3").Replace("(", "").Replace(")", ""); }
    public static string ToStringNoBracket(this Vector3 v) { return v.ToString("F3").Replace("(", "").Replace(")", ""); }
    public static Vector2 ToVector2NoYaxis(this Vector3 v) { return new Vector2(v.x, v.z); }

    public static void SetPositionX(this Transform t, float x) { t.position = new Vector3(x, t.position.y, t.position.z); }
    public static void SetPositionY(this Transform t, float y) { t.position = new Vector3(t.position.x, y, t.position.z); }
    public static void SetPositionZ(this Transform t, float z) { t.position = new Vector3(t.position.x, t.position.y, z); }
    public static float GetPositionX(this Transform t) { return t.position.x; }
    public static float GetPositionY(this Transform t) { return t.position.y; }
    public static float GetPositionZ(this Transform t) { return t.position.z; }
    public static bool HasRigidbody(this GameObject go) { return (go.GetComponent<Rigidbody>() != null); }
    public static bool HasAnimation(this GameObject go) { return (go.GetComponent<Animation>() != null); }
    public static void SetSpeed(this Animation anim, float newSpeed) { anim[anim.clip.name].speed = newSpeed; }

    public static int ColorToInt(this Color c)
    {
        int retVal = 0;
        retVal |= Mathf.RoundToInt(c.r * 255f) << 24;
        retVal |= Mathf.RoundToInt(c.g * 255f) << 16;
        retVal |= Mathf.RoundToInt(c.b * 255f) << 8;
        retVal |= Mathf.RoundToInt(c.a * 255f);
        return retVal;
    }

    public static string ToHexString(this Color color)
    {
        return color.ColorToInt().ToString("X8");
    }

    public static string ToXmlColorString(this string message, Color color)
    {
        return string.Format("<color=#{0}>{1}</color>", color.ToHexString(), message);
    }
}