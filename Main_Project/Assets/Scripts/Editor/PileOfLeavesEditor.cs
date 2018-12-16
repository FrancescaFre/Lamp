using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

[CustomEditor(typeof (PileOfLeaves))]
public class PileOfLeavesEditor : Editor {

    void OnSceneGUI()
    {
        PileOfLeaves pile = (PileOfLeaves)target;
        Handles.color = Color.green;
        Handles.DrawWireArc(pile.transform.position, pile.transform.up, pile.transform.forward, 360, pile.radiusIsSneaking);
        Handles.color = Color.white;
        Handles.DrawWireArc(pile.transform.position, pile.transform.up, pile.transform.forward, 360, pile.radiusNotSneaking);
    }
}
