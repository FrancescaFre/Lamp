using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSpaceship : MonoBehaviour {

    public Transform spaceship;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void FixedUpdate()
    {
        Vector3 desiredPos = transform.position = spaceship.position + offset ;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = smoothedPos;

        transform.LookAt(spaceship);
    }


}
