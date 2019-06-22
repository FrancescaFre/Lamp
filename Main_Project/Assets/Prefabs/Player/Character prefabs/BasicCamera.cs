using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCamera : MonoBehaviour {
    public static BasicCamera instance;

    public float cameraSpeed = 5f; 
    public Transform player;
    public Transform planet;

    public float smoothSpeed = 0.425f, input;
    public Vector3 offset;
    Vector3 desiredPos, temp_right, temp_up;

    private void Awake() {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update() {

        temp_up = ( player.position - planet.position).normalized;
        temp_right = Vector3.Cross(temp_up, player.forward).normalized;

        input = Input.GetAxis(Controllers.PS4_RStick_X) + Input.GetAxis("Mouse X");
        Quaternion camTurn = Quaternion.AngleAxis(input * cameraSpeed, temp_up);

        desiredPos = player.position;
        //desiredPos += player.forward * offset.z;
       // desiredPos += temp_up * offset.y;
        //desiredPos += temp_right * offset.x;


        desiredPos += (camTurn * player.forward) * offset.z ;
        desiredPos += temp_up * offset.y;
        desiredPos += (camTurn * temp_right) * offset.x;


        //Vector3 smoothedPos = Vector3.Slerp(transform.position, desiredPos, smoothSpeed* Time.deltaTime);
        //transform.position = smoothedPos;


        transform.position = desiredPos;
        //transform.position = desiredPos;

        Debug.Log("desired " + desiredPos + "\ncamturn " + transform.position);

        transform.LookAt(player, temp_up);
    }
    
}