using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BSpline : Curve
{

    //private int order = 2; // Degree of the curve



    //private List<Vector3> cachedControlPoints; // cached control points
    public List<int> nV; // Node vector

    [HideInInspector]
    public string nodeVector;

    private float step;

    // Recursive deBoor algorithm.
    public Vector3 deBoor(int r, int i, float u)
    {
        
        if(i  == activePoints.activePoints.Count)
            return activePoints.activePoints[i-1].transform.position;
        if (r == 0)
        {
            return activePoints.activePoints[i].transform.position;
        }
        else
        {

            float pre = (u - nV[i + r]) / (nV[i + order + 1] - nV[i + r]); // Precalculation
            Vector3 part1 = (deBoor( r - 1, i, u ) * (1 - pre));
            Vector3 part2 = (deBoor( r - 1, i + 1, u ) * (pre));
            return part1 + part2;
        }

    }

    public void createNodeVector()
    {


        int knoten = 0;



        for (int i = 0; i < (order + activePoints.controlPointCount + 1); i++) // n+m+1 = nr of nodes
        {
            if (i > order)
            {
                if (i <= activePoints.controlPointCount)
                {

                    nV.Add( ++knoten );
                }
                else
                {
                    nV.Add( knoten );
                }
            }
            else
            {
                nV.Add( knoten );
            }
        }



        nodeVector = string.Join( ", ", nV );

    }






    public override void DrawCurve()
    {
        if (activePoints.activePoints != null && !ContainsNull( activePoints.activePoints ))
        {

            if (activePoints.controlPointCount <= 0)
            {
                return;
            }




            // Initialize node vector.
            nV = new List<int>();
            createNodeVector();


            // Draw the bspline lines
            Gizmos.color = lineColor;

            //Vector3 start = activePoints.activePoints[0].transform.position;
            Vector3 start = deBoor( order, 0, 0 );
            Vector3 end = Vector3.zero;
            step = ((float) nV[order + activePoints.controlPointCount] / lineSteps);
            float i;
            for (i = 0.0f; i < nV[order + activePoints.controlPointCount]; i += step)
            {

                for (int j = 0; j < activePoints.controlPointCount; j++)
                {
                    if (i >= j)
                    {
                        end = deBoor( order, j, i );
                        continue;
                    }
                }


                Gizmos.DrawLine( start, end );
                start = end;

            }
        }
    }

    public float GetMaxProgressValue()
    {
        if (activePoints.controlPointCount == 0)
            return 0f;
        return (float) nV[order + activePoints.controlPointCount];
    }

    public override void OnValidate()
    {
        base.OnValidate();


        if (activePoints.controlPointCount > 0 && order <= 0)
        {
            order = activePoints.controlPointCount / 2;
        }

        if (order > activePoints.controlPointCount)
        {
            order = activePoints.controlPointCount - 1;
        }

        nV = new List<int>();
        createNodeVector();


    }

    public Vector3 GetPoint(float progress)
    {

        Vector3 end = new Vector3();
        for (int j = 0; j < activePoints.controlPointCount; j++)
        {
            if (progress >= j)
            {

                end = deBoor( order, j, progress );
                //return end;

            }
        }
        return end;
    }

    public Vector3 GetDirection(float progress)
    {
        //TBD - 1. derivate of de boor function

        return GetPoint( progress + step );
    }
}
