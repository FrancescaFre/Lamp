using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovTest2 : MonoBehaviour
{
    public float walkSpeed = 5f;
    public Transform cam;

    private Rigidbody _rig;
    private float horizInput, vertInput;


    void Start()
    {
        _rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        horizInput = Input.GetAxis("Horizontal");
        vertInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (vertInput == 0 && horizInput == 0)
            return;

        MovePlayerParentCam();
    }

    private void MovePlayerParentCam()
    {
        Vector3 movement = transform.TransformDirection(horizInput, 0, vertInput).normalized * walkSpeed * Time.deltaTime;

        if (horizInput != 0)
        {
            cam.SetParent(null);
            transform.LookAt(transform.localPosition + movement, transform.up);
            cam.SetParent(transform);
        }

        _rig.MovePosition(_rig.position + movement);
    }
}
