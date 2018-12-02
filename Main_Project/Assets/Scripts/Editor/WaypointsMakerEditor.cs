using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointsMakerEditor : Editor {
   // public GameObject prefab;
    Transform father;
    public GameObject go;


    #region GIZMO
    private void OnDrawGizmos()
    {
        father = go.transform;
        if (father.childCount > 0)
        {
            Vector3 startPosition = father.GetChild(0).position;
            Vector3 previousPosition = startPosition;

            foreach (Transform waypoint in father)//prendo tutti i figli in teoria
            {
                Gizmos.DrawSphere(waypoint.position, .3f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine(previousPosition, startPosition);
        }
    }

    #endregion

}
