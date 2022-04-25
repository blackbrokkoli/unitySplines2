using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( Bezier ) )]
public class BezierInspector : CurveInspector
{

    private Bezier bezier;

    public override void OnInspectorGUI()

    {
        base.OnInspectorGUI();
        bezier = target as Bezier;

        serializedObject.Update();
        EditorGUILayout.LabelField( "order", bezier.activePoints.controlPointCount - 1 > 0? "" + (bezier.activePoints.controlPointCount - 1) : "less than two points available" );
        serializedObject.ApplyModifiedProperties();


    }

}