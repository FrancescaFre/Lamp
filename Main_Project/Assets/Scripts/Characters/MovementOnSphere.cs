using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementOnSphere : MonoBehaviour {

    [Range(5, 10)]
    public float walkSpeed = 8f;
    [Range(10, 15)]
    public float runSpeed = 10f;
    [Range(1, 5)]
    public float stealthSpeed = 4f;

    private Vector3 _orientation;
    private Vector3 _moveDir = Vector3.zero;
    private float _horiz_axis;
    private float _vert_axis;
    private PlayerController _player;
    private Rigidbody _rig;
   
    
    // Use this for initialization
    void Start () {
        _horiz_axis = 0f;
        _vert_axis = 0f;
        _player = GetComponent<PlayerController>();
        _rig = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {
        this.SetDirection();
    }

    private void FixedUpdate() {
        this.CheckMovement();
    }

    /// <summary>
    /// Sets the player movement direction
    /// </summary>
    private void SetDirection() {

        this._horiz_axis = Input.GetAxis("Horizontal");
        this._vert_axis = Input.GetAxis("Vertical");

        _moveDir.Set(_horiz_axis, 0f, _vert_axis);
        _moveDir.Normalize(); 
    }

    /// <summary>
    /// Moves the player if an input is detected
    /// </summary>
    private void CheckMovement() {

        //to move the player
        Vector3 movement = Vector3.zero;


        if ((Input.GetButton("PS4_R2") || Input.GetKey(KeyCode.R)) && (_horiz_axis != 0 || _vert_axis != 0)) {
            //if is holding down a button and moving use the running animation and speed
            movement = transform.TransformDirection(_moveDir) * runSpeed * Time.deltaTime;


            Debug.Log("RUN");
        }
        else if ((Input.GetButton("PS4_L2") || Input.GetKey(KeyCode.T)) && (_horiz_axis != 0 || _vert_axis != 0)) {
            //if is holding down a button and moving use the stealth animation and speed

            movement = transform.TransformDirection(_moveDir) * stealthSpeed * Time.deltaTime;
            Debug.Log("STEALTH");
        }
        else if (_horiz_axis != 0 || _vert_axis != 0) {
            //if only moving use walk animation and speed

            movement = transform.TransformDirection(_moveDir) * walkSpeed * Time.deltaTime;
            //movement = movement * walkSpeed * Time.deltaTime;
            Debug.Log("WALK");
        }



        _rig.MovePosition(_rig.position + movement);
        _rig.MoveRotation(new Quaternion(_horiz_axis, 0, _vert_axis, 1).normalized);

    }
}
