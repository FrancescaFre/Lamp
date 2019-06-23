using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerF : MonoBehaviour
{

    public float moveSpeed = 15f;
    private Vector3 moveDir;
    private Rigidbody rb;

    public bool moving = false;


    Vector3 co_up, co_front, co_right, nextrot;

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


        co_front = this.transform.forward;
        co_right = BasicCamera.instance.transform.right;
        co_up = Vector3.Cross(co_front, co_right).normalized;

        

        Debug.Log(co_right);
        nextrot = transform.position + (co_right * horizontal*2) + (co_front * vertical * 2);

       // transform.LookAt(nextrot);
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Debug.Log(co_front);
        Gizmos.DrawLine(this.transform.position, this.transform.position+ co_front * 5f);
        //Gizmos.DrawSphere(co_front, 1.0f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(this.transform.position, this.transform.position + co_up * 5f);
        //Gizmos.DrawSphere(co_up, 1.0f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + co_right * 5f);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(nextrot, 0.1f);
    }
}