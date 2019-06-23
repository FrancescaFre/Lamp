using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerF : MonoBehaviour
{

    public float moveSpeed = 15f;
    private Vector3 moveDir;
    private Rigidbody rb;

    public bool moving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        if (moveDir != Vector3.zero)
            moving = true;
        else
            moving = false;


    }

    private void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        // rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
        Vector3 mov;

        if (vertical > 0) // Pressing ↓ made the player literally jump backwards (so it's needed the _screenUp variable)
            mov = (BasicCamera.instance.transform.forward * vertical + BasicCamera.instance.transform.right * horizontal).normalized * moveSpeed * Time.deltaTime; // Takes camera axes to correct the direction
        else
            mov = (BasicCamera.instance.transform.up * vertical + BasicCamera.instance.transform.right * horizontal).normalized * moveSpeed * Time.deltaTime;

        rb.MovePosition(rb.position + mov);

        //transform.LookAt((transform.position + (transform.forward * vertical)), transform.up);

         
    }
}