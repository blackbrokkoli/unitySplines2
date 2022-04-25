using UnityEngine;
using System.Collections;
using UnityEditor;

public class ControlPoint : MonoBehaviour
{

    public Color color = Color.red;
    [HideInInspector]
    public Vector3 cachedPosition;

    //public ControlPoint(int id)
    //{
    //    name = "Control Point " + id;
    //}

    void Start()
    {
        cachedPosition = transform.position;
        this.transform.hideFlags = HideFlags.HideInHierarchy;
    }

    void OnDrawGizmos()
    {

            cachedPosition = transform.position;

            // Draw control point
            Gizmos.color = color;
            Gizmos.DrawSphere( cachedPosition, 1f );
            Handles.Label( cachedPosition + new Vector3(1,1, 1), new GUIContent( name ) );
        
    }

}