using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class BoundCheckWindow : EditorWindow
{
    private enum DimensionType { _2D, _3D }
    private enum BoundType2D
    {
        Line,
        Box,
        Circle,
        Triangle,
        Sector,
    }
    private enum BoundType3D
    {
        Cube,
        Capsule,
        Sphere,
    }
    private enum CheckerType2D
    {
        Dot,
        Line,
        Ray,
        Box,
        LerpBox,
        Triangle,
        Circle,
        Sector,
    }
    private enum CheckerType3D
    {
        Dot,
        Line,
        Ray,
        Cube,
        Sphere,
    }

    const int SEGMENT = 32;
    readonly Vector3[] VERTICES_CIRCLE_2D;
    readonly Vector3[] VERTICES_BOX_2D =
            { new Vector3(-0.5f, 0.5f), new Vector3(0.5f, 0.5f)
            , new Vector3(0.5f, -0.5f), new Vector3(-0.5f, -0.5f)
            , new Vector3(-0.5f, 0.5f)};

    DimensionType _dimension = DimensionType._2D;
    BoundType2D _bound2d;
    BoundType3D _bound3d;
    CheckerType2D _checker2d;
    CheckerType3D _checker3d;

    float _boundAngle = 45f;
    #region 2D Bound var  (Line, Box, Circle, Triangle, Sector)
    Vector2 _boundPosition2D_A = Vector2.zero;
    Vector2 _boundPosition2D_B = Vector2.right;
    Vector2 _boundSize2D = Vector2.one;
    Vector3[] _boundVertices2D_trianle = new Vector3[3]; // only Triangle
    float _boundRotation2D = 0f;
    #endregion

    #region 3D Bound var (Cube, Capsule, Sphere)
    Vector3 _boundPosition3D = Vector3.zero;
    Vector3 _boundSize3D = Vector3.one;
    Quaternion _boundRotation3D = Quaternion.identity;
    #endregion

    float _checkerRadius = 0f;
    float _checkerAngle = 45f;
    #region 2D Checker (Dot, Line, Ray, Box, LerpBox, Triangle, Circle, Sector)
    Vector2 _checkerPosition2D_A = Vector2.zero;
    Vector2 _checkerPosition2D_B = Vector2.right;
    Vector2 _checkerDirection2D = new Vector2(0f, 1f);
    Vector2 _checkerSize2D = Vector2.one;
    Vector2[] _checkerVertices2D = new Vector2[3]; // only Triangle
    #endregion

    #region 3D Checker (Dot, Line, Ray, Cube, Sphere)
    Vector3 _checkerPosition3D_A = Vector3.zero;
    Vector3 _checkerPosition3D_B = Vector3.right;
    Vector3 _checkerDirection3D = new Vector3(0f, 0f, 1f);
    Vector3 _checkerSize3D = Vector3.one;
    Quaternion _checkerRotation3D = Quaternion.identity;
    #endregion

    bool _VerticeTriangle2D_Foldout = true;

    BoundCheckWindow()
    {
        VERTICES_CIRCLE_2D = new Vector3[SEGMENT + 1];
        for (int i = 0; i < SEGMENT + 1; ++i)
            VERTICES_CIRCLE_2D[i] = new Vector3(Mathf.Sin(((float)i / SEGMENT) * Mathf.PI * 2), Mathf.Cos(((float)i / SEGMENT) * Mathf.PI * 2));

        _boundVertices2D_trianle[0] = new Vector3(0f, 0.5f, 0f);
        _boundVertices2D_trianle[1] = new Vector3(-0.5f, -0.5f, 0f);
        _boundVertices2D_trianle[2] = new Vector3(0.5f, -0.5f, 0f);
    }


    [MenuItem("TEST/BoundCheck")]
    static void Init()
    {
        BoundCheckWindow window = (BoundCheckWindow)EditorWindow.GetWindow(typeof(BoundCheckWindow), false, "BoundCheck");
        window.Show();
    }

    #region
    void Start() {}
    void Update() {}
    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }
    void OnEnable()
    {
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }
    void OnDisable()
    {
        //SceneView.onSceneGUIDelegate -= OnSceneGUI;
    } 
    #endregion

    void OnSceneGUI(SceneView sceneView)
    {
        //HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        EditorGUI.BeginChangeCheck();
        if (_dimension == DimensionType._2D)
        {
            _boundPosition2D_A = Handles.FreeMoveHandle(_boundPosition2D_A, Quaternion.identity, HandleUtility.GetHandleSize(_boundPosition2D_A) * 0.1f, Vector3.zero, Handles.DotCap);
            switch (_bound2d)
            {
                case BoundType2D.Line:
                    Handles.DrawLine(_boundPosition2D_A, _boundPosition2D_B);
                    _boundPosition2D_B = Handles.FreeMoveHandle(_boundPosition2D_B, Quaternion.identity, HandleUtility.GetHandleSize(_boundPosition2D_B) * 0.1f, Vector3.zero, Handles.DotCap);
                    break;
                case BoundType2D.Box:
                    Handles.matrix = Matrix4x4.TRS(_boundPosition2D_A, Quaternion.identity, _boundSize2D);
                    Handles.DrawPolyLine(VERTICES_BOX_2D);
                    Handles.matrix = Matrix4x4.identity;
                    break;
                case BoundType2D.Circle:
                    Handles.matrix = Matrix4x4.TRS(_boundPosition2D_A, Quaternion.identity, _boundSize2D);
                    Handles.DrawPolyLine(VERTICES_CIRCLE_2D);
                    Handles.matrix = Matrix4x4.identity;
                    break;
                case BoundType2D.Sector:
                    break;
                case BoundType2D.Triangle:
                    Vector3 tmp = _boundVertices2D_trianle[0] + (Vector3)_boundPosition2D_A;
                    _boundVertices2D_trianle[0] = Handles.FreeMoveHandle(tmp, Quaternion.identity, HandleUtility.GetHandleSize(tmp) * 0.2f, Vector3.zero, Handles.SphereCap) - (Vector3)_boundPosition2D_A;
                    tmp = _boundVertices2D_trianle[1] + (Vector3)_boundPosition2D_A;
                    _boundVertices2D_trianle[1] = Handles.FreeMoveHandle(tmp, Quaternion.identity, HandleUtility.GetHandleSize(tmp) * 0.2f, Vector3.zero, Handles.SphereCap) - (Vector3)_boundPosition2D_A;
                    tmp = _boundVertices2D_trianle[2] + (Vector3)_boundPosition2D_A;
                    _boundVertices2D_trianle[2] = Handles.FreeMoveHandle(tmp, Quaternion.identity, HandleUtility.GetHandleSize(tmp) * 0.2f, Vector3.zero, Handles.SphereCap) - (Vector3)_boundPosition2D_A;
                    Handles.matrix = Matrix4x4.TRS(_boundPosition2D_A, Quaternion.identity, _boundSize2D);
                    Handles.DrawPolyLine(_boundVertices2D_trianle);
                    Handles.DrawLine(_boundVertices2D_trianle[2], _boundVertices2D_trianle[0]);
                    Handles.matrix = Matrix4x4.identity;
                    break;
            }
            switch(_checker2d)
            {
                case CheckerType2D.Dot:
                    break;
                case CheckerType2D.Line:
                    break;
                case CheckerType2D.Ray:
                    break;
                case CheckerType2D.Box:
                    break;
                case CheckerType2D.LerpBox:
                    break;
                case CheckerType2D.Circle:
                    break;
                case CheckerType2D.Sector:
                    break;
                case CheckerType2D.Triangle:
                    break;
            }
        }
        else //if(_dimension == DimensionType._3D)
        {

        }
        if (EditorGUI.EndChangeCheck()) Repaint();
    }

    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();
        _dimension = GUILayout.Toggle(_dimension == DimensionType._2D, "2D", new GUIStyle("button")) ? DimensionType._2D : DimensionType._3D;
        _dimension = GUILayout.Toggle(_dimension == DimensionType._3D, "3D", new GUIStyle("button")) ? DimensionType._3D : DimensionType._2D;
        EditorGUILayout.EndHorizontal();
        if(_dimension == DimensionType._2D)
        {
            EditorGUILayout.Separator();
            _bound2d = (BoundType2D)EditorGUILayout.EnumPopup("Bound Type", _bound2d);
            EditorGUILayout.BeginVertical("box");

            string label = "Position";
            if (_bound2d == BoundType2D.Line) label += " A";
            _boundPosition2D_A = EditorGUILayout.Vector2Field(label, _boundPosition2D_A);

            if (_bound2d == BoundType2D.Line)
                _boundPosition2D_B = EditorGUILayout.Vector2Field("Position B", _boundPosition2D_B);
            else if(_bound2d == BoundType2D.Triangle)
            {
                if (_VerticeTriangle2D_Foldout = EditorGUILayout.Foldout(_VerticeTriangle2D_Foldout, "Vertices"))
                {
                    ++EditorGUI.indentLevel;
                    for (int i = 0; i < 3; ++i)
                        _boundVertices2D_trianle[i] = EditorGUILayout.Vector2Field("Vertex " + i, _boundVertices2D_trianle[i]);
                    --EditorGUI.indentLevel;
                }
            }
            else
                _boundSize2D = EditorGUILayout.Vector2Field("Size", _boundSize2D);
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
            _checker2d = (CheckerType2D)EditorGUILayout.EnumPopup("Checker Type", _checker2d);
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }
        else
        {
            _bound3d = (BoundType3D)EditorGUILayout.EnumPopup("Bound Type", _bound3d);
            _checker3d = (CheckerType3D)EditorGUILayout.EnumPopup("Checker Type", _checker3d);
        }

        EditorGUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck()) SceneView.RepaintAll();
    }
}