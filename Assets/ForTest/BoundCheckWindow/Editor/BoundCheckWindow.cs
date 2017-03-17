using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using ZKit;

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
    Vector3[] _boundVertices2D_triangle = new Vector3[3]; // only Triangle
    float _boundRotation2D = 0f;
    #endregion

    #region 3D Bound var (Cube, Capsule, Sphere)
    Vector3 _boundPosition3D = Vector3.zero;
    Vector3 _boundSize3D = Vector3.one;
    Quaternion _boundRotation3D = Quaternion.identity;
    #endregion

    float _checkerAngle = 45f;
    #region 2D Checker (Dot, Line, Ray, Box, LerpBox, Triangle, Circle, Sector)
    Vector2 _checkerPosition2D_A = Vector2.zero;
    Vector2 _checkerPosition2D_B = Vector2.right;
    Vector2 _checkerSize2D = Vector2.one;
    Vector3[] _checkerVertices2D_triangle = new Vector3[3]; // only Triangle
    #endregion

    #region 3D Checker (Dot, Line, Ray, Cube, Sphere)
    Vector3 _checkerPosition3D_A = Vector3.zero;
    Vector3 _checkerPosition3D_B = Vector3.right;
    Vector3 _checkerDirection3D = new Vector3(0f, 0f, 1f);
    Vector3 _checkerSize3D = Vector3.one;
    Quaternion _checkerRotation3D = Quaternion.identity;
    #endregion

    bool _boundVerticeTriangle2D_Foldout = true;
    bool _checkerVerticeTriangle2D_Foldout = true;

    bool _isIn = false;

    readonly Texture2D _pixGreen;
    readonly Texture2D _pixRed;

    BoundCheckWindow()
    {
        _boundVertices2D_triangle[0] = _checkerVertices2D_triangle[0] = new Vector2(0f, 0.5f);
        _boundVertices2D_triangle[1] = _checkerVertices2D_triangle[1] = new Vector2(-0.5f, -0.25f);
        _boundVertices2D_triangle[2] = _checkerVertices2D_triangle[2] = new Vector2(0.5f, -0.25f);

        Color[] pix = new Color[1];
        pix[0] = new Color(0f, 0.5f, 0.75f);
        _pixGreen = new Texture2D(1, 1);
        _pixGreen.SetPixels(pix);
        _pixGreen.Apply();

        pix = new Color[1];
        pix[0] = new Color(1f, 0.5f, 0.5f);
        _pixRed = new Texture2D(1, 1);
        _pixRed.SetPixels(pix);
        _pixRed.Apply();
    }

    [MenuItem("TEST/BoundCheck")]
    static void Init()
    {
        BoundCheckWindow window = (BoundCheckWindow)EditorWindow.GetWindow(typeof(BoundCheckWindow), false, "BoundCheck");
        window.Show();
    }

    #region
    void Start() {}
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

    void Update()
    {
        _isIn = false;
        if (_dimension == DimensionType._2D)
        {
            if(_bound2d == BoundType2D.Line)
            {
                if(_checker2d == CheckerType2D.Line)
                    _isIn = ZKit.Math.Collision2D.Line(_boundPosition2D_A, _boundPosition2D_B, _checkerPosition2D_A, _checkerPosition2D_B);
            }
        }
        else
        {

        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        //HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        EditorGUI.BeginChangeCheck();
        if (_dimension == DimensionType._2D)
        {
            Vector2 center = (_boundVertices2D_triangle[0] + _boundVertices2D_triangle[1] + _boundVertices2D_triangle[2]) / 3;
            Vector2 newCenter = Handles.FreeMoveHandle(center, Quaternion.identity, HandleUtility.GetHandleSize(center) * 0.1f, Vector3.zero, Handles.DotCap);
            if (center != newCenter)
                for(int i = 0; i < 3; ++i)
                    _boundVertices2D_triangle[i] += (Vector3)(newCenter - center);
            _boundPosition2D_A = newCenter;

            switch (_bound2d)
            {
                case BoundType2D.Line:
                    Handles.DrawLine(_boundPosition2D_A, _boundPosition2D_B);
                    _boundPosition2D_B = Handles.FreeMoveHandle(_boundPosition2D_B, Quaternion.identity, HandleUtility.GetHandleSize(_boundPosition2D_B) * 0.1f, Vector3.zero, Handles.DotCap);
                    break;
                case BoundType2D.Box:
                    Handles.matrix = Matrix4x4.TRS(_boundPosition2D_A, Quaternion.identity, _boundSize2D);
                    HandlesExtension.DrawSquare();
                    Handles.matrix = Matrix4x4.identity;
                    break;
                case BoundType2D.Circle:
                    Handles.matrix = Matrix4x4.TRS(_boundPosition2D_A, Quaternion.identity, _boundSize2D);
                    HandlesExtension.DrawCircle();
                    Handles.matrix = Matrix4x4.identity;
                    break;
                case BoundType2D.Sector:
                    Handles.matrix = Matrix4x4.TRS(_boundPosition2D_A, Quaternion.identity, _boundSize2D);
                    HandlesExtension.DrawSector(_boundAngle);
                    Handles.matrix = Matrix4x4.identity;
                    break;
                case BoundType2D.Triangle:
                    _boundVertices2D_triangle[0] = Handles.FreeMoveHandle(_boundVertices2D_triangle[0]
                        , Quaternion.identity, HandleUtility.GetHandleSize(_boundVertices2D_triangle[0]) * 0.08f, Vector3.zero, Handles.SphereCap);
                    _boundVertices2D_triangle[1] = Handles.FreeMoveHandle(_boundVertices2D_triangle[1]
                        , Quaternion.identity, HandleUtility.GetHandleSize(_boundVertices2D_triangle[1]) * 0.08f, Vector3.zero, Handles.SphereCap);
                    _boundVertices2D_triangle[2] = Handles.FreeMoveHandle(_boundVertices2D_triangle[2]
                        , Quaternion.identity, HandleUtility.GetHandleSize(_boundVertices2D_triangle[2]) * 0.08f, Vector3.zero, Handles.SphereCap);
                    Handles.DrawPolyLine(_boundVertices2D_triangle);
                    Handles.DrawLine(_boundVertices2D_triangle[2], _boundVertices2D_triangle[0]);
                    break;
            }

            center = (_checkerVertices2D_triangle[0] + _checkerVertices2D_triangle[1] + _checkerVertices2D_triangle[2]) / 3;
            newCenter = Handles.FreeMoveHandle(center, Quaternion.identity, HandleUtility.GetHandleSize(center) * 0.1f, Vector3.zero, Handles.DotCap);
            if (center != newCenter)
                for (int i = 0; i < 3; ++i)
                    _checkerVertices2D_triangle[i] += (Vector3)(newCenter - center);
            _checkerPosition2D_A = newCenter;
            switch (_checker2d)
            {
                //case CheckerType2D.Dot:
                //    break;
                case CheckerType2D.Line:
                    _checkerPosition2D_B = Handles.FreeMoveHandle(_checkerPosition2D_B, Quaternion.identity, HandleUtility.GetHandleSize(_checkerPosition2D_B) * 0.1f, Vector3.zero, Handles.DotCap);
                    Handles.DrawLine(_checkerPosition2D_A, _checkerPosition2D_B);
                    break;
                case CheckerType2D.Ray:
                    _checkerPosition2D_B = Handles.FreeMoveHandle(_checkerPosition2D_B, Quaternion.identity, HandleUtility.GetHandleSize(_checkerPosition2D_B) * 0.1f, Vector3.zero, Handles.DotCap);
                    Vector3 dirTmp = _checkerPosition2D_B - _checkerPosition2D_A;
                    Handles.ArrowCap(0, _checkerPosition2D_A, Quaternion.FromToRotation(Vector3.forward, dirTmp), HandleUtility.GetHandleSize(_checkerPosition2D_A) * 1.1f);
                    Handles.DrawLine(_checkerPosition2D_A, (Vector3)_checkerPosition2D_A + dirTmp * 100f);
                    break;
                case CheckerType2D.Box:
                    Handles.matrix = Matrix4x4.TRS(_checkerPosition2D_A, Quaternion.identity, _checkerSize2D);
                    HandlesExtension.DrawSquare();
                    Handles.matrix = Matrix4x4.identity;
                    break;
                case CheckerType2D.LerpBox:
                    break;
                case CheckerType2D.Circle:
                    Handles.matrix = Matrix4x4.TRS(_checkerPosition2D_A, Quaternion.identity, _checkerSize2D);
                    HandlesExtension.DrawCircle();
                    Handles.matrix = Matrix4x4.identity;
                    break;
                case CheckerType2D.Sector:
                    Handles.matrix = Matrix4x4.TRS(_checkerPosition2D_A, Quaternion.identity, _checkerSize2D);
                    HandlesExtension.DrawSector(_checkerAngle);
                    Handles.matrix = Matrix4x4.identity;
                    break;
                case CheckerType2D.Triangle:
                    _checkerVertices2D_triangle[0] = Handles.FreeMoveHandle(_checkerVertices2D_triangle[0]
                        , Quaternion.identity, HandleUtility.GetHandleSize(_checkerVertices2D_triangle[0]) * 0.08f, Vector3.zero, Handles.SphereCap);
                    _checkerVertices2D_triangle[1] = Handles.FreeMoveHandle(_checkerVertices2D_triangle[1]
                        , Quaternion.identity, HandleUtility.GetHandleSize(_checkerVertices2D_triangle[1]) * 0.08f, Vector3.zero, Handles.SphereCap);
                    _checkerVertices2D_triangle[2] = Handles.FreeMoveHandle(_checkerVertices2D_triangle[2]
                        , Quaternion.identity, HandleUtility.GetHandleSize(_checkerVertices2D_triangle[2]) * 0.08f, Vector3.zero, Handles.SphereCap);
                    Handles.DrawPolyLine(_checkerVertices2D_triangle);
                    Handles.DrawLine(_checkerVertices2D_triangle[2], _checkerVertices2D_triangle[0]);
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
            #region Bound UI
            EditorGUILayout.BeginVertical("box");
            string labelTmp = "Position";
            if (_bound2d == BoundType2D.Line) labelTmp += " A";
            Vector2 posGap = _boundPosition2D_A;
            _boundPosition2D_A = EditorGUILayout.Vector2Field(labelTmp, _boundPosition2D_A);
            posGap = _boundPosition2D_A - posGap;
            for (int i = 0; i < 3; ++i)
                _boundVertices2D_triangle[i] += (Vector3)posGap;

            if (_bound2d == BoundType2D.Line)
                _boundPosition2D_B = EditorGUILayout.Vector2Field("Position B", _boundPosition2D_B);
            else if (_bound2d == BoundType2D.Triangle)
            {
                if (_boundVerticeTriangle2D_Foldout = EditorGUILayout.Foldout(_boundVerticeTriangle2D_Foldout, "Vertices"))
                {
                    ++EditorGUI.indentLevel;
                    for (int i = 0; i < 3; ++i)
                        _boundVertices2D_triangle[i] = EditorGUILayout.Vector2Field("Vertex " + i, _boundVertices2D_triangle[i]);
                    --EditorGUI.indentLevel;
                }
            }
            else if (_bound2d == BoundType2D.Circle)
            {
                // 일단 원. 타원은 나중에.
                _boundSize2D.x = _boundSize2D.y = EditorGUILayout.FloatField("Radius", _boundSize2D.x);
            }
            else if (_bound2d == BoundType2D.Sector)
            {
                _boundSize2D.x = _boundSize2D.y = EditorGUILayout.FloatField("Radius", _boundSize2D.x);
                _boundAngle = EditorGUILayout.FloatField("Angle", _boundAngle);
            }
            else
                _boundSize2D = EditorGUILayout.Vector2Field("Size", _boundSize2D);
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical(); 
            #endregion
            EditorGUILayout.Separator();
            _checker2d = (CheckerType2D)EditorGUILayout.EnumPopup("Checker Type", _checker2d);
            #region Checker
            EditorGUILayout.BeginVertical("box");
            labelTmp = "Position";
            if (_checker2d == CheckerType2D.Line) labelTmp += " A";
            posGap = _checkerPosition2D_A;
            _checkerPosition2D_A = EditorGUILayout.Vector2Field(labelTmp, _checkerPosition2D_A);
            posGap = _checkerPosition2D_A - posGap;
            for (int i = 0; i < 3; ++i)
                _checkerVertices2D_triangle[i] += (Vector3)posGap;

            if (_checker2d == CheckerType2D.Line)
                _checkerPosition2D_B = EditorGUILayout.Vector2Field("Position B", _checkerPosition2D_B);
            else if ( _checker2d == CheckerType2D.Ray)
                _checkerPosition2D_B = EditorGUILayout.Vector2Field("Dir Pos", _checkerPosition2D_B);
            else if (_checker2d == CheckerType2D.Triangle)
            {
                if (_checkerVerticeTriangle2D_Foldout = EditorGUILayout.Foldout(_checkerVerticeTriangle2D_Foldout, "Vertices"))
                {
                    ++EditorGUI.indentLevel;
                    for (int i = 0; i < 3; ++i)
                        _checkerVertices2D_triangle[i] = EditorGUILayout.Vector2Field("Vertex " + i, _checkerVertices2D_triangle[i]);
                    --EditorGUI.indentLevel;
                }
            }
            else if (_checker2d == CheckerType2D.Circle)
            {
                // 일단 원. 타원은 나중에.
                _checkerSize2D.x = _checkerSize2D.y = EditorGUILayout.FloatField("Radius", _checkerSize2D.x);
            }
            else if (_checker2d == CheckerType2D.Sector)
            {
                _checkerSize2D.x = _checkerSize2D.y = EditorGUILayout.FloatField("Radius", _checkerSize2D.x);
                _checkerAngle = EditorGUILayout.FloatField("Angle", _checkerAngle);
            }
            else if(_checker2d != CheckerType2D.Dot)
                _checkerSize2D = EditorGUILayout.Vector2Field("Size", _checkerSize2D);
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            #endregion
        }
        else // 3D
        {
            EditorGUILayout.Separator();
            _bound3d = (BoundType3D)EditorGUILayout.EnumPopup("Bound Type", _bound3d);
            EditorGUILayout.Separator();
            _checker3d = (CheckerType3D)EditorGUILayout.EnumPopup("Checker Type", _checker3d);
        }
        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal("box");
        GUIStyle gs = new GUIStyle(GUI.skin.box);
        gs.fixedWidth = 10; gs.fixedHeight = 14;
        if (_isIn)  gs.normal.background = _pixGreen;
        else        gs.normal.background = _pixRed;
        GUILayout.Box(_pixGreen, gs);
        GUIStyle alignCenter = new GUIStyle("label");
        alignCenter.alignment = TextAnchor.MiddleCenter;
        EditorGUILayout.LabelField("In : " + _isIn.ToString(), alignCenter);
        GUILayout.Box(_pixGreen, gs);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck()) SceneView.RepaintAll();
    }
}