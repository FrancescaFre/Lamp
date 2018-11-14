using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovToCamera : MonoBehaviour
{

    [Range(5, 10)]
    public float walkSpeed = 8f;
    public Transform cam;

    private Vector3 _moveDir = Vector3.zero;
    private Rigidbody _rig;

    private Vector3 camForward, camRight;
    private Vector2 input;

    // Use this for initialization
    void Start()
    {
        _rig = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1);

        camForward = cam.forward;
        camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 movement = (camForward * input.y + camRight * input.x) * walkSpeed * Time.deltaTime;
        //Vector3 movement = transform.TransformDirection(camForward * input.y + camRight * input.x) * walkSpeed * Time.deltaTime;

        _rig.MovePosition(_rig.position + movement);
        //_rig.rotation = Quaternion.Euler(camForward.z * input.x, transform.position.y, camRight.x * input.y);
        
        transform.LookAt(transform.localPosition + movement.y * Vector3.up);
    }
}
