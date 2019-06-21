using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollowSpaceship : MonoBehaviour {

    public Transform spaceship;
    private MovingSpaceShip movingSpaceship; 
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public Vector3 offsetFront;

    private void FixedUpdate()
    {
        movingSpaceship = spaceship.GetComponent<MovingSpaceShip>(); 
        Vector3 desiredPos;
        if (movingSpaceship.inFrontOf)
        {
            //  desiredPos = transform.position = spaceship.localPosition + offsetFront;
            desiredPos = (transform.position + movingSpaceship.trigger.position);
            desiredPos.x = Mathf.Clamp(desiredPos.x, -5, 5);
            desiredPos.y = Mathf.Clamp(desiredPos.y, -5, 5);
            desiredPos.z = Mathf.Clamp(desiredPos.z, -5, 5);
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;

            transform.LookAt(movingSpaceship.trigger);
        }
        else
        {
            desiredPos = transform.position = spaceship.position + offset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;

            transform.LookAt(spaceship);
        }
    }


}
