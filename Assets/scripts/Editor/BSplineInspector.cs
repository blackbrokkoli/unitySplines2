using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( BSpline ))]
public class BSplineInspector : CurveInspector
{

    private BSpline bspline;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        bspline = target as BSpline;

        EditorGUI.BeginChangeCheck();
        var val = EditorGUILayout.IntSlider( new GUIContent( "order" ), bspline.order, 1, bspline.activePoints != null? (bspline.activePoints.controlPointCount >1? bspline.activePoints.controlPointCount - 1 : 1 ) : 0);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject( bspline, "order" );
            EditorUtility.SetDirty( bspline );
            bspline.order = val;

        }

        EditorGUILayout.LabelField( "nodeVector", "" + bspline.nodeVector );

    }
}
