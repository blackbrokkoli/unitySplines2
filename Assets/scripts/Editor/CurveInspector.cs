using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( Curve ), true )]
public class CurveInspector : Editor
{
    //public CurveType curveType = CurveType.Bezier;
    public Curve curve;



    public override void OnInspectorGUI()

    {

        curve = target as Curve;

        if (GUILayout.Button( "Remove all active points" ))
        {
            Undo.RecordObject( curve, "Add Curve" );
            EditorUtility.SetDirty( curve );
            curve.ResetPoints();
        }


        EditorGUI.BeginChangeCheck();
        Color color = EditorGUILayout.ColorField( "Spline Color", curve.lineColor );
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject( curve, "ColorField Spline" );
            EditorUtility.SetDirty( curve );
            curve.lineColor = color;

        }

        EditorGUI.BeginChangeCheck();
        var val = EditorGUILayout.IntSlider( new GUIContent( "LineSteps" ), curve.lineSteps, 1, 100 );
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject( curve, "IntSlider lineSteps" );
            EditorUtility.SetDirty( curve );
            curve.lineSteps = val;

        }

        EditorGUI.BeginChangeCheck();
        var val3 = EditorGUILayout.IntField( "ControlPoint Count", curve.activePoints != null ? curve.activePoints.controlPointCount : 0 );
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject( curve, "IntSlider lineSteps" );
            EditorUtility.SetDirty( curve.activePoints );
            if (curve.activePoints != null)
            {
                curve.activePoints.controlPointCount = val3;
            }

            //curve.controlPointCount = val3;
            curve.OnValidate();

        }

        serializedObject.Update();
        EditorGUIUtility.LookLikeInspector();
        //ListIterator( "controlPoints" );       //serializedObject.ApplyModifiedProperties();
        //ListIterator( "controlPointCount" );
        serializedObject.ApplyModifiedProperties();



    }


    public void ListIterator(string listName)
    {
        //List object
        SerializedProperty listIterator = serializedObject.FindProperty( listName );
        Rect drawZone = GUILayoutUtility.GetRect( 0f, 16f );
        bool showChildren = EditorGUI.PropertyField( drawZone, listIterator );
        bool toBeContinued = listIterator.NextVisible( showChildren );

        if (toBeContinued)
        {
            //List size
            drawZone = GUILayoutUtility.GetRect( 0f, 16f );
            showChildren = EditorGUI.PropertyField( drawZone, listIterator );
            toBeContinued = listIterator.NextVisible( showChildren );
            //Elements
        }
        int listElement = 0;
        while (toBeContinued)
        {
            drawZone = GUILayoutUtility.GetRect( 0f, 16f );
            showChildren = EditorGUI.PropertyField( drawZone, listIterator );
            toBeContinued = listIterator.NextVisible( showChildren );
            listElement++;
        }

    }

}