using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public enum CurveType { Bezier, BezierSpline, BSpline };

[ExecuteInEditMode]
public class Curve : MonoBehaviour
{

    //[SerializeField]
    //public List<ControlPoint> controlPoints = new List<ControlPoint>(); // The control points.

    [HideInInspector]
    public Color lineColor = new Color( 1, 1, 1, 1 );

    [HideInInspector]
    public int lineSteps = 12;

    [HideInInspector]
    public int order = 2;


    //[HideInInspector]
    //public int controlPointCount;

    //[HideInInspector]
    //public List<ControlPoint> myPointsList = new List<ControlPoint>();

   


    public virtual void DrawCurve() { }

    public ActivePoints activePoints;


    public void OnDrawGizmos()
    {
        DrawCurve();
    }

    public virtual void OnValidate()
    {
        //controlPoints.Clear();

        activePoints = this.GetComponent<ActivePoints>();
        if (activePoints == null)
        {
            activePoints = gameObject.AddComponent<ActivePoints>();
        }

                if (activePoints.activePoints.Count < activePoints.controlPointCount)
        {
            for (int i = activePoints.activePoints.Count; i < activePoints.controlPointCount; i++)
            {
                GameObject go = new GameObject( "Control Point " + i );
                go.transform.SetParent( gameObject.transform );
                go.transform.position = gameObject.transform.position;
                //go.hideFlags = HideFlags.HideInInspector;
                var cp = go.AddComponent<ControlPoint>();
                
                activePoints.activePoints.Add( cp );
            }
        }

        //for (int i = 0; i < activePoints.controlPointCount; i++)
        //{
        //    controlPoints.Add( activePoints.activePoints[i] );
        //}
        //for (int k = activePoints.controlPointCount; k < activePoints.activePoints.Count; k++)
        //{
        //    controlPoints.Remove( activePoints.activePoints[k] );
        //}
    }

    public void ResetPoints()
    {
        if (activePoints != null)
        {
            foreach (var ob in activePoints.activePoints)
            {
                if(ob!= null)
                    DestroyImmediate( ob.gameObject );
            }

            activePoints.activePoints.Clear();
           //controlPoints.Clear();
            activePoints.controlPointCount = 0;
            //controlPointCount = 0;
            order = 1;
        }
    }

    public bool ContainsNull(List<ControlPoint> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == null)
            {
                return true;
            }
        }

        return false;
    }

}

[ExecuteInEditMode]
public class ActivePoints : MonoBehaviour
{
    [HideInInspector]
    public List<ControlPoint> activePoints = new List<ControlPoint>();
    [HideInInspector]
    public int controlPointCount;


}