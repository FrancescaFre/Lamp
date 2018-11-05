using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointsMakerEditor : Editor {
    public GameObject prefab;
    public Transform father;
    GameObject go;
    RaycastHit hit;

    private void OnSceneGUI()
    {
        Vector2 guiPosition = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(guiPosition);
        Physics.Raycast(ray, out hit);
        /*
        Event e = Event.current;
        Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
        Vector3 mouseP = r.origin;
        */

        if (Input.GetMouseButtonDown(0)) {
            go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            go.transform.position = hit.point;
        }
    /*
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        
        if (Physics.Raycast(ray, out hit))
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("hit position " + hit.point);
                go = Instantiate(prefab);
                go.transform.position = hit.point;
                go.transform.SetParent(father, false);
            }
    */
    }

    

    #region GIZMO
    private void OnDrawGizmos()
    {
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
