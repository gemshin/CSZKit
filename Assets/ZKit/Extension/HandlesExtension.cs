using UnityEngine;
using System.Collections;
using UnityEditor;

public static class HandlesExtension
{
    public static void DrawArc(float angle, uint segment = 32)
    {
        float angRad = angle * Mathf.Deg2Rad;
        float halfRad = angRad * 0.5f;
        Vector3[] vertArc = new Vector3[segment];
        for (int i = 0; i < segment; ++i)
        {
            float seg = angRad * i / (segment - 1f) - halfRad;
            vertArc[i] = new Vector2(Mathf.Sin(seg), Mathf.Cos(seg));
        }
        Handles.DrawPolyLine(vertArc);
    }

    public static void DrawCircle(uint segment = 32)
    {
        float twoPI = Mathf.PI * 2f;
        Vector3[] vertCircle = new Vector3[segment + 1];
        for (int i = 0; i < segment + 1; ++i)
            vertCircle[i] = new Vector2(Mathf.Sin(((float)i / segment) * twoPI), Mathf.Cos(((float)i / segment) * twoPI));
        Handles.DrawPolyLine(vertCircle);
    }

    public static void DrawSquare()
    {
        Vector3[] vertSquare = { new Vector3(-0.5f, 0.5f), new Vector3(0.5f, 0.5f)
                        , new Vector3(0.5f, -0.5f), new Vector3(-0.5f, -0.5f)
                        , new Vector3(-0.5f, 0.5f)};
        Handles.DrawPolyLine(vertSquare);
    }

    public static void DrawSector(float angle, uint segment = 32)
    {
        DrawArc(angle, segment);
        float half = angle * Mathf.Deg2Rad * 0.5f;
        Handles.DrawLine(Vector2.zero, new Vector2(Mathf.Sin(-half), Mathf.Cos(half)));
        Handles.DrawLine(Vector2.zero, new Vector2(Mathf.Sin(half), Mathf.Cos(half)));
    }
}
