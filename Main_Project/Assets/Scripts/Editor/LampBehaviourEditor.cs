using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor (typeof (LampBehaviour))]
public class LampBehaviourEditor : Editor {

    void OnSceneGUI()
    {
        LampBehaviour lamp = (LampBehaviour)target;
        Handles.color = Color.magenta;
        Handles.DrawWireArc(lamp.transform.position, lamp.transform.up, lamp.transform.forward, 360, lamp.radius);
    }
}
