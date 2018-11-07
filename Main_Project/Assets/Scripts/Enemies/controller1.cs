using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller1 : MonoBehaviour
{
    public float moveSpeed = 15f;
    private Vector3 moveDir;
    private Rigidbody rb;
    public Look look;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        look = GetComponent<Look>();
    }
    
    private void Update()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        Debug.Log("control "+moveDir);
        look.MoveVector=moveDir;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
    }
}