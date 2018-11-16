using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovToCamera : MonoBehaviour
{

    [Range(0, 10)]
    public float walkSpeed;
    public Transform cam;
    public Transform referenceCam;

    private Rigidbody _rig;

    private float horizInput, vertInput;
    private Vector3 screenForward, screenRight;

    //private Transform prevCam;

    //private Transform fixedCam;
    //private Vector3 fixedPosition;

    // Use this for initialization
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
        //MovePlayerRotateCam();
    }

    private void LateUpdate()
    {
        //if (cam.transform.parent == null)
        //cam.SetParent(transform);

        //cam.localPosition = referenceCam.localPosition;
        //cam.localRotation = referenceCam.localRotation;
    }


    /*
     * WORKING COMBINATIONS:
     *          Vector3 movement = (transform.forward * vertInput + transform.right * horizInput) * walkSpeed * Time.deltaTime;
     *           •  _rig.MoveRotation(Quaternion.LookRotation(movement, transform.up));
     *           •  transform.LookAt(transform.localPosition + movement, transform.up);
     *           
     *          Vector3 movement = transform.TransformDirection(horizInput, 0, vertInput).normalized * walkSpeed * Time.deltaTime;
     *           •  cam.SetParent(null);
     *           •  transform.LookAt(transform.localPosition + movement, transform.up);
     *           •  cam.SetParent(transform);
     */
    /// <summary>
    /// Moves the player and ajusts rotation by unparenting the camera
    /// </summary>
    private void MovePlayerParentCam()
    {
        //ComputeCamAxes();
        //Vector3 movement = (screenForward * vertInput + screenRight * horizInput) * walkSpeed * Time.deltaTime; // Takes camera axes as correct input
        Vector3 movement = transform.TransformDirection(horizInput, 0, vertInput).normalized * walkSpeed * Time.deltaTime;

        //_rig.MovePosition(_rig.position + movement);

        if (horizInput != 0)
        {
            cam.SetParent(null);
            //transform.localRotation = Quaternion.LookRotation(movement, transform.up); // 1) Issues with curvature, but generally good
            //transform.rotation = Quaternion.LookRotation(movement, transform.up); // 1) SAME AS ↑

            //_rig.MoveRotation(Quaternion.LookRotation(movement, transform.up)); // 2) BROKEN

            //transform.LookAt(_rig.position + movement, transform.up); // 3) Issues with curvature, better than 1
            transform.LookAt(transform.localPosition + movement, transform.up); // 3) SAME AS ↑
            
            //transform.localRotation = Quaternion.LookRotation(transform.InverseTransformDirection(movement), transform.up); // 4) BROKEN
            //transform.localRotation = Quaternion.LookRotation(transform.InverseTransformVector(movement), transform.up); 4) SAME AS ↑

            cam.SetParent(transform);
            //cam.localPosition = referenceCam.localPosition;
            //cam.localRotation = referenceCam.localRotation;
        }

        _rig.MovePosition(_rig.position + movement); // better to rotate first then translate
    }

    private void MovePlayerRotateCam()
    {
        ComputeCamAxes();
        Vector3 movement = (screenForward * vertInput + screenRight * horizInput) * walkSpeed * Time.deltaTime; // Takes camera axes as correct input

        if (horizInput != 0)
        {
            //prevCam = cam;
            transform.LookAt(_rig.position + movement, transform.up);
        }

        _rig.MovePosition(_rig.position + movement); // better to rotate first then translate
    }

    /// <summary>
    /// Makes sure the player input moves the player in the camera direction
    /// </summary>
    private void ComputeCamAxes()
    {
        screenForward = cam.forward;
        screenRight = cam.right;
        screenForward.Normalize();
        screenRight.Normalize();
    }


}
