using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ToolCreateWaypoints : MonoBehaviour
{

    public GameObject prefab;
    GameObject go;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("hit position " + hitInfo.point);

                go = Instantiate(prefab);
                go.transform.position = hitInfo.point;
                go.transform.SetParent(transform, false);
            }
        }
    }

    #region GIZMO
    private void OnDrawGizmos()
    {
        if (transform.childCount > 0)
        {
            Vector3 startPosition = this.transform.GetChild(0).position;
            Vector3 previousPosition = startPosition;

            foreach (Transform waypoint in transform)//prendo tutti i figli in teoria
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



