using System.Collections.Generic;
using UnityEditor;
using UnityEngine;




public class Bezier : Curve
{



    // Update is called once per frame
    private Vector3 algorithm(List<ControlPoint> points, int n, int i, float t)
    {
        var point = points[i].gameObject.transform;
        //if (t == 1f)
        //    return points[points.Count - 1].position;
        Vector3 formula = point.position * (faculty( n ) / (faculty( i ) * faculty( n - i )) * Mathf.Pow( 1 - t, (float) n - i ) * Mathf.Pow( t, i ));

        i = i + 1;
        if (i == points.Count)
        {
            return formula;
        }

        return formula + algorithm( points, n, i, t );
    }

    private int faculty(int value)
    {
        if (value == 0)
        {
            return 1;
        }

        var res = value;
        for (int i = 1; i < value; i++)
        {
            res *= i;
        }
        return res;
    }


    public Vector3 GetPoint(float t)
    {
        return algorithm( activePoints.activePoints, activePoints.activePoints.Count - 1, 0, t );
        //return algorithm( controlPoints, controlPoints.Count - 1, 0, t );
    }

    //Vector3 GetDirection(float t)
    //{
    //    Vector3 res = algortihmDerivate( points, points.Count, 0, t );
    //    return res;

    //}

    //Vector3 algortihmDerivate(List<Transform> points, int n, int i, float t)
    //{
    //    var m = (faculty( n ) / (faculty( i ) * faculty( n - i )));

    //    Vector3 formula = new Vector3( 0, 0, 0 );
    //    if (t-1 > 0)
    //        formula = points[i].position * (m * Mathf.Pow( (float) 1 - t, (float) n - i ) * (Mathf.Pow( (float) t, (float) i - 1 ) == Mathf.Infinity? 0: Mathf.Pow( (float) t, (float) i - 1 )) *(n*t-i))/(t-1);

    //    i++;
    //    if (i == points.Count)
    //        return formula;

    //    return formula + algortihmDerivate( points, n, i, t );

    //}

    public override void OnValidate()
    {
        base.OnValidate();
        order = activePoints.controlPointCount - 1;
        //controlPoints = activePoints.activePoints;
        //controlPointCount = activePoints.controlPointCount;
        

        

    }






    public override void DrawCurve()
    {
        if ( activePoints.activePoints != null && !ContainsNull( activePoints.activePoints ))

        { 
        if (activePoints.activePoints.Count >= 2)
        {

            Handles.color = Color.gray;

            for (int i = 1; i < activePoints.activePoints.Count; i++)
            {
                Handles.DrawLine( activePoints.activePoints[i - 1].transform.position, activePoints.activePoints[i].transform.position );

            }

            Vector3 lineStart = GetPoint( 0f );
            Handles.color = Color.green;
            //Handles.DrawLine( lineStart, lineStart + GetDirection( 0f ) * 3 );
            for (int i = 1; i <= lineSteps; i++)
            {
                Vector3 lineEnd = GetPoint( i / (float) lineSteps );
                Handles.color = lineColor; // Color.blue;
                Handles.DrawLine( lineStart, lineEnd );
                Handles.color = Color.green;
                //Handles.DrawLine( lineEnd, lineEnd + GetDirection( i / (float) lineSteps ) );
                lineStart = lineEnd;
            }
        }
        }
    }
}


//using System.Collections.Generic;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;


//public class NewBezier : MonoBehaviour
//{
//    [SerializeField]
//    public List<GameObject> controlPoints = new List<GameObject>();

//    [HideInInspector]
//    public Color color = new Color( 1, 1, 1, 1 );

//    [HideInInspector]
//    public int lineSteps = 12;

//    [HideInInspector]
//    public int _variable = 2;


//    // Start is called before the first frame update
//    private void Start()
//    {

//    }

//    // Update is called once per frame
//    private Vector3 algorithm(List<GameObject> points, int n, int i, float t)
//    {
//        var point = points[i].transform;
//        //if (t == 1f)
//        //    return points[points.Count - 1].position;
//        Vector3 formula = point.position * (faculty( n ) / (faculty( i ) * faculty( n - i )) * Mathf.Pow( 1 - t, (float) n - i ) * Mathf.Pow( t, i ));

//        i = i + 1;
//        if (i == points.Count)
//        {
//            return formula;
//        }

//        return formula + algorithm( points, n, i, t );
//    }

//    private int faculty(int value)
//    {
//        if (value == 0)
//        {
//            return 1;
//        }

//        var res = value;
//        for (int i = 1; i < value; i++)
//        {
//            res *= i;
//        }
//        return res;
//    }


//    public Vector3 GetPoint(float t)
//    {

//        return algorithm( controlPoints, controlPoints.Count - 1, 0, t );
//    }

//    //Vector3 GetDirection(float t)
//    //{
//    //    Vector3 res = algortihmDerivate( points, points.Count, 0, t );
//    //    return res;

//    //}

//    //Vector3 algortihmDerivate(List<Transform> points, int n, int i, float t)
//    //{
//    //    var m = (faculty( n ) / (faculty( i ) * faculty( n - i )));

//    //    Vector3 formula = new Vector3( 0, 0, 0 );
//    //    if (t-1 > 0)
//    //        formula = points[i].position * (m * Mathf.Pow( (float) 1 - t, (float) n - i ) * (Mathf.Pow( (float) t, (float) i - 1 ) == Mathf.Infinity? 0: Mathf.Pow( (float) t, (float) i - 1 )) *(n*t-i))/(t-1);

//    //    i++;
//    //    if (i == points.Count)
//    //        return formula;

//    //    return formula + algortihmDerivate( points, n, i, t );

//    //}

//    private bool ContainsNull(List<GameObject> list)
//    {
//        for (int i = 0; i < list.Count; i++)
//            if (list[i] == null)
//                return true;

//        return false;
//    }



//    private void OnDrawGizmos()
//    {
//        if (controlPoints != null && !ContainsNull( controlPoints ))
//        {

//            if (controlPoints.Count >= 2)
//            {

//                Handles.color = Color.gray;

//                for (int i = 1; i < controlPoints.Count; i++)
//                {
//                    Handles.DrawLine( controlPoints[i - 1].transform.position, controlPoints[i].transform.position );

//                }

//                Vector3 lineStart = GetPoint( 0f );
//                Handles.color = Color.green;
//                //Handles.DrawLine( lineStart, lineStart + GetDirection( 0f ) * 3 );
//                for (int i = 1; i <= lineSteps; i++)
//                {
//                    Vector3 lineEnd = GetPoint( i / (float) lineSteps );
//                    Handles.color = color; // Color.blue;
//                    Handles.DrawLine( lineStart, lineEnd );
//                    Handles.color = Color.green;
//                    //Handles.DrawLine( lineEnd, lineEnd + GetDirection( i / (float) lineSteps ) );
//                    lineStart = lineEnd;
//                }
//            }
//        }
//    }
//}
