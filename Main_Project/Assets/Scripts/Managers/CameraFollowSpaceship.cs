using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSpaceship : MonoBehaviour {
    public static CameraFollowSpaceship instance;

    public Transform spaceship;
    private MovingSpaceShip movingSpaceship;
    public float smoothSpeed = 0.125f;
    public Vector3 offset, offsetFront;

    private void Awake() {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void FixedUpdate() {
        movingSpaceship = spaceship.GetComponent<MovingSpaceShip>();
        Vector3 desiredPos, temp_forward, temp_right;
        if (movingSpaceship.inFrontOf) {
            temp_forward = (movingSpaceship.trigger.position - spaceship.position).normalized;
            temp_right = Vector3.Cross(spaceship.up, temp_forward).normalized;

            desiredPos = spaceship.position;
            desiredPos -= temp_forward * offsetFront.z;
            desiredPos += spaceship.up * offsetFront.y;
            desiredPos += temp_right * offsetFront.x;

            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;

            transform.LookAt(movingSpaceship.trigger);
        }
        else {
            desiredPos = spaceship.position + offset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;

            transform.LookAt(spaceship);
        }
    }
}