using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyFOV))]
public class EnemyFOVEditor : Editor
{

    void OnSceneGUI()
    {
        EnemyFOV fow = (EnemyFOV) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, fow.transform.up, fow.transform.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, true);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, true);
        
        //"fow.transform.rotation.normalized * " is added to follow the orientation of the transform 
        Handles.DrawLine(fow.transform.position, fow.transform.position + fow.transform.rotation * viewAngleA * fow.viewRadius);
        Handles.color = Color.blue;
        Handles.DrawLine(fow.transform.position, fow.transform.position + fow.transform.rotation * viewAngleB * fow.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }

}