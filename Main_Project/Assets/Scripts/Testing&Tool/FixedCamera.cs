using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour {

    [Tooltip("The dummy player's camera transform (must be dummy's child")]
    public Transform dummyCam; // The dummy camera is used to avoid 3-dimensional inverse revolutions

    private Vector3 _camOffset; // Difference in position between the main camera and the dummy one

    
    void Start () {
        _camOffset = transform.position - dummyCam.position;
    }

    private void LateUpdate()
    {
        transform.SetPositionAndRotation(dummyCam.position + _camOffset, dummyCam.rotation); 
    }
}
