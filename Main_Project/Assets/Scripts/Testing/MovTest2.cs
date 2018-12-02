using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovTest2 : MonoBehaviour
{
    public float walkSpeed = 5f;
    public Transform cam;
    public Transform topCam;

    private Rigidbody _rig;
    private Vector3 movement;
    private float horizInput, vertInput;
    private Vector3 screenForward, screenUp, screenRight;


    void Start()
    {
        _rig = GetComponent<Rigidbody>();
        movement = Vector3.zero;
    }

    /*
    private void Update()
    {
        horizInput = Input.GetAxis("Horizontal");
        vertInput = Input.GetAxis("Vertical");
        if (vertInput != 0 || horizInput != 0)
            MovePlayerParentCam();
    }*/

    private void FixedUpdate()
    {
        horizInput = Input.GetAxis("Horizontal");
        vertInput = Input.GetAxis("Vertical");
        if (vertInput != 0 || horizInput != 0)
            MovePlayerParentCam();
        //topCam.transform.parent = transform;
    }

    private void Update()
    {
        //topCam.SetParent(null);
        //transform.forward = (horizInput * screenRight + vertInput * screenUp).normalized;
    }

    private void LateUpdate()
    {
        //topCam.transform.parent = transform;
    }

    private void MovePlayerParentCam()
    {
        ComputeCamAxes();
        //movement = transform.TransformDirection(horizInput, 0, vertInput).normalized * walkSpeed * Time.deltaTime;
        movement = (horizInput * screenRight + vertInput * screenUp).normalized * walkSpeed * Time.deltaTime;

        //Vector3 uppete = topCam.up;

        //topCam.up = uppete;    

        //_rig.MoveRotation(Quaternion.RotateTowards(_rig.rotation, ))
        _rig.MovePosition(_rig.position + movement);
        
    }

    private void ComputeCamAxes()
    {
        screenForward = topCam.forward;
        screenRight = topCam.right;
        screenUp = topCam.up;
        screenForward.Normalize();
        screenRight.Normalize();
        screenUp.Normalize();
    }
}
