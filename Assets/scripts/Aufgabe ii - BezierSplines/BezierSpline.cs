using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class BezierSpline : MonoBehaviour
{

    [SerializeField]
    //private Vector3[] points;
    public List<Vector3> points;

    [SerializeField]
    //private Vector3[] points;
    public List<Bezier> test;
    //private BezierCurveRot[] curveObj;

    [SerializeField]
    private BezierControlPointMode[] modes;

    [SerializeField]
    private bool loop;

    [SerializeField]
    public Color color = new Color( 1, 1, 1, 1 );

    [SerializeField]
    public int lineWidth = 2;

    //private void Update()
    //{
    //    points.Clear();
    //    foreach(var objects in curveObj)
    //    {
    //        foreach(var point in objects.editorPoints)
    //        {
    //            points.Add( point );
    //        }
    //    }
    //}

    private void Start()
    {
        if (modes == null)
        {
            modes = new BezierControlPointMode[3];
        }

        if (points == null)
        {
            points = new List<Vector3>() { gameObject.transform.position + new Vector3( 10, 0, 10 ) };


            AddCurve( 0 );
        }
    }

    public bool Loop
    {
        get
        {
            return loop;
        }
        set
        {
            loop = value;
            if (value == true)
            {
                modes[modes.Length - 1] = modes[0];
                SetControlPoint( 0, points[0] );
            }
        }
    }

    public int ControlPointCount
    {
        get
        {
            return points.Count;
        }
    }

    public Vector3 GetControlPoint(int index)
    {
        return points[index];
    }

    public void SetControlPoint(int index, Vector3 point)
    {
        if (index % 3 == 0)
        {
            Vector3 delta = point - points[index];
            if (loop)
            {
                if (index == 0)
                {
                    points[1] += delta;
                    points[points.Count - 2] += delta;
                    points[points.Count - 1] = point;
                }
                else if (index == points.Count - 1)
                {
                    points[0] = point;
                    points[1] += delta;
                    points[index - 1] += delta;
                }
                else
                {
                    points[index - 1] += delta;
                    points[index + 1] += delta;
                }
            }
            else
            {
                if (index > 0)
                {
                    points[index - 1] += delta;
                }
                if (index + 1 < points.Count)
                {
                    points[index + 1] += delta;
                }
            }
        }
        points[index] = point;
        EnforceMode( index );
    }

    public BezierControlPointMode GetControlPointMode(int index)
    {
        return modes[(index + 1) / 3];
    }

    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        int modeIndex = (index + 1) / 3;
        modes[modeIndex] = mode;
        if (loop)
        {
            if (modeIndex == 0)
            {
                modes[modes.Length - 1] = mode;
            }
            else if (modeIndex == modes.Length - 1)
            {
                modes[0] = mode;
            }
        }
        EnforceMode( index );
    }

    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1) / 3;
        BezierControlPointMode mode = modes[modeIndex];
        if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1))
        {
            return;
        }

        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            if (fixedIndex < 0)
            {
                fixedIndex = points.Count - 2;
            }
            enforcedIndex = middleIndex + 1;
            if (enforcedIndex >= points.Count)
            {
                enforcedIndex = 1;
            }
        }
        else
        {
            fixedIndex = middleIndex + 1;
            if (fixedIndex >= points.Count)
            {
                fixedIndex = 1;
            }
            enforcedIndex = middleIndex - 1;
            if (enforcedIndex < 0)
            {
                enforcedIndex = points.Count - 2;
            }
        }

        Vector3 middle = points[middleIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];
        if (mode == BezierControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance( middle, points[enforcedIndex] );
        }
        points[enforcedIndex] = middle + enforcedTangent;
    }

    public int CurveCount
    {
        get
        {
            return (points.Count - 1) / 3;
        }
    }

    public Vector3 GetPoint(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Count - 4;
        }
        else
        {
            t = Mathf.Clamp01( t ) * CurveCount;
            i = (int) t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint( Bezier2.GetPoint( points[i], points[i + 1], points[i + 2], points[i + 3], t ) );
    }

    public Vector3 GetVelocity(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Count - 4;
        }
        else
        {
            t = Mathf.Clamp01( t ) * CurveCount;
            i = (int) t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint( Bezier2.GetFirstDerivative( points[i], points[i + 1], points[i + 2], points[i + 3], t ) ) - transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity( t ).normalized;
    }

    public void AddCurve(int index)
    {
        int curveIndex = (index) / 3 * 3;
        Vector3 addPositionAt = points[curveIndex];
        //Vector3 point = points[points.Count - 1];
        //Array.Resize(ref points, points.Count + 3);
        points.Capacity = points.Count + 3;
        addPositionAt.x += -1f;
        addPositionAt.z += 1f;
        points.Insert( curveIndex + 1, addPositionAt );
        addPositionAt.x += 2f;
        //addPositionAt.z += 1f;
        points.Insert( curveIndex + 2, addPositionAt );
        addPositionAt.x += -1f;
        addPositionAt.x += 1f;
        points.Insert( curveIndex + 3, addPositionAt );

        Array.Resize( ref modes, modes.Length + 1 );
        modes[modes.Length - 1] = modes[modes.Length - 2];
        EnforceMode( points.Count - 4 );

        if (loop)
        {
            points[points.Count - 1] = points[0];
            modes[modes.Length - 1] = modes[0];
            EnforceMode( 0 );
        }

        if (!loop)
        {
            modes[0] = BezierControlPointMode.Free;
            modes[modes.Length - 1] = BezierControlPointMode.Free;
        }
    }

    public void RemoveCurve(int index)
    {
        int curveIndex = (index - 1) / 3;
        if (CurveCount > 1)
        {

            points.RemoveAt( curveIndex * 3 );
            points.RemoveAt( curveIndex * 3 );
            points.RemoveAt( curveIndex * 3 );
        }
    }

    public void Reset()
    {
        if (points != null)
        {
            points.Clear();
            points.Add( new Vector3( 10f, 0f, 0f ) );
            points.Add( new Vector3( 10f, 0f, 10f ) );
            points.Add( new Vector3( 20f, 0f, -20f ) );
            points.Add( new Vector3( 20f, 0f, -10f ) );
        }
        //      points = new Vector3[] {
        //	new Vector3(1f, 0f, 0f),
        //	new Vector3(2f, 0f, 0f),
        //	new Vector3(3f, 0f, 0f),
        //	new Vector3(4f, 0f, 0f)
        //};
        modes = new BezierControlPointMode[] {
            BezierControlPointMode.Mirrored,
            BezierControlPointMode.Mirrored
        };
    }

    public void OnDrawGizmos()


    {
        Vector3 p0 = GetControlPoint( 0 ) + transform.localPosition;
        for (int i = 1; i < ControlPointCount; i += 3)
        {
            Vector3 p1 = GetControlPoint( i ) + transform.localPosition;
            Vector3 p2 = GetControlPoint( i + 1 ) + transform.localPosition;
            Vector3 p3 = GetControlPoint( i + 2 ) + transform.localPosition;
            //Handles.color = Color.gray;
            //Handles.DrawLine( p0, p1 );
            //Handles.DrawLine( p2, p3 );

            Handles.DrawBezier( p0, p3, p1, p2, color, null, lineWidth );
            p0 = p3;
        }
    }
}