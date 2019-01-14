using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovTest1 : MonoBehaviour
{
    public float walkSpeed = 5f;

    private Rigidbody _rig;
    private float horizInput, vertInput;

    void Start()
    {
        _rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        horizInput = Input.GetAxis(Controllers.Horizontal);
        vertInput = Input.GetAxis(Controllers.Vertical);
    }

    private void FixedUpdate()
    {
        if (vertInput == 0 && horizInput == 0)
            return;

        MovePlayer1();
    }

    private void MovePlayer1()
    {
        Vector3 movement = (transform.forward * vertInput + transform.right * horizInput) * walkSpeed * Time.deltaTime;

        if (horizInput != 0)
        {
            _rig.MoveRotation(Quaternion.LookRotation(movement, transform.up));
            //transform.LookAt(transform.localPosition + movement, transform.up);
        }

        _rig.MovePosition(_rig.position + movement);
    }
}
