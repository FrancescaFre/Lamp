using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Look : MonoBehaviour
{

    public float lookSpeed = 10;
    public  Vector3 MoveVector;
    private Vector3 directionInput;
    public Transform waypoint;
    private Vector3 dirY;
    private Rigidbody rb;

    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

//         MoveVector = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
      // MoveVector = Vector3.ProjectOnPlane(MoveVector, transform.up);

        Debug.Log(MoveVector);
        
       // if (MoveVector.x!=0 || MoveVector.z!=0)
            RotateTowardsMoveDirection();
         //InputListen();
        // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - prevLoc), Time.deltaTime * lookSpeed);
    }

    private void LateUpdate()
    {
        MoveVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        //RotateTowardsMoveDirection();
    }

    public void RotateTowardsMoveDirection()
    {
        if (MoveVector.x != 0 || MoveVector.z != 0)
          transform.LookAt(rb.position + MoveVector, transform.up);
        
        //  transform.LookAt(transform.position + transform.TransformDirection(MoveVector));
    }

    private void InputListen()
    {

        if (Input.GetKey(KeyCode.A))
            directionInput = transform.InverseTransformVector(new Vector3(-1f * Time.deltaTime, 0f, 0f));
        if (Input.GetKey(KeyCode.D))
            directionInput = transform.InverseTransformVector(new Vector3(+1f * Time.deltaTime, 0f, 0f));
        if (Input.GetKey(KeyCode.W))
            directionInput = transform.InverseTransformVector(new Vector3(0f, 0f, 1f * Time.deltaTime));
        if (Input.GetKey(KeyCode.S))
            directionInput = transform.InverseTransformVector(new Vector3(0f, 0f, -1f * Time.deltaTime));

        transform.LookAt(transform.position + directionInput);

        transform.position += directionInput;
    }
}

/*
public class Look : MonoBehaviour
{

    public float lookSpeed = 10;
    private Vector3 curLoc;
    private Vector3 prevLoc;
    public float angle;

    void Update()
    {
        //transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
        
       transform.RotateAround(transform.position, transform.up, Time.deltaTime * angle);

        //InputListen();
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - prevLoc), Time.deltaTime * lookSpeed);
    }

    private void InputListen()
    {
        prevLoc = curLoc;
        curLoc = transform.position;

        if (Input.GetKey(KeyCode.A))
            curLoc.x -= 1 * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            curLoc.x += 1 * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
            curLoc.z += 1 * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            curLoc.z -= 1 * Time.deltaTime;

        transform.position = curLoc;
    }
}
*/
