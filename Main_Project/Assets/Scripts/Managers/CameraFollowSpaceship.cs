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
            desiredPos = spaceship.position;
            desiredPos -= spaceship.forward * offsetFront.z;
            desiredPos += spaceship.up * offsetFront.y;
            desiredPos += spaceship.right * offsetFront.x; 
            
            desiredPos.x = Mathf.Clamp(desiredPos.x, movingSpaceship.trigger.position.x - 10, movingSpaceship.trigger.position.x + 10);
            desiredPos.z = Mathf.Clamp(desiredPos.z, movingSpaceship.trigger.position.z - 10, movingSpaceship.trigger.position.z + 10);
            desiredPos.y = Mathf.Clamp(desiredPos.y, movingSpaceship.trigger.position.y - 10, movingSpaceship.trigger.position.y + 10);
            

     
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;

            transform.LookAt(movingSpaceship.trigger);
           
        }

        else
        {
            desiredPos = spaceship.position + offset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;

            transform.LookAt(spaceship);
        }
    }


}
