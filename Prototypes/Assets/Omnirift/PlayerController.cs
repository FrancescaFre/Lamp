using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed = 18f;
    public float turnSpeed = 60f;

    private Rigidbody _rig;
	// Use this for initialization
	void Start () {
        _rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        //to move the player
        float horiz_axis = Input.GetAxis("Horizontal");
        float vert_axis = Input.GetAxis("Vertical");

       
       

        Vector3 movement = transform.TransformDirection(new Vector3(horiz_axis,0,vert_axis)*speed*Time.deltaTime);

        _rig.MovePosition(transform.position+movement);

        //to rotate the player
        float rStickX = Input.GetAxis("PS4_RStick_X");
        transform.Rotate(new Vector3(0, rStickX, 0) * turnSpeed * Time.deltaTime);


        float dPadX = Input.GetAxis("PS4_DPad_X");
        float dPadY = Input.GetAxis("PS4_DPad_Y");

        //Front Left
        if (dPadX > 0)
            Debug.Log("DPad: Right");
        if (dPadX < 0)
            Debug.Log("DPad: Left");
        if (dPadY > 0)
            Debug.Log("DPad: Up");
        if (dPadY < 0)
            Debug.Log("DPad: Down");
        if (Input.GetButtonDown("PS4_Button_LStickClick"))
            Debug.Log("Input: LStickClick");

        //Front Right
        if (Input.GetButtonDown("PS4_Button_Square"))
            Debug.Log("Input: Square");
        if (Input.GetButtonDown("PS4_Button_X"))
            Debug.Log("Input: X");
        if (Input.GetButtonDown("PS4_Button_O"))
            Debug.Log("Input: O");
        if (Input.GetButtonDown("PS4_Button_Triangle"))
            Debug.Log("Input: Triangle");
        if (Input.GetButtonDown("PS4_Button_RStickClick"))
            Debug.Log("Input: RStickClick");

        //Back
        if (Input.GetButtonDown("PS4_L1"))
            Debug.Log("Input: L1");
        if (Input.GetButtonDown("PS4_R1"))
            Debug.Log("Input: R1");
        if (Input.GetButtonDown("PS4_L2"))
            Debug.Log("Input: L2");
        if (Input.GetButtonDown("PS4_R2"))
            Debug.Log("Input: R2");

        //Options
        if (Input.GetButtonDown("PS4_Button_SHARE"))
            Debug.Log("Input: SHARE");
        if (Input.GetButtonDown("PS4_Button_OPTIONS"))
            Debug.Log("Input: OPTIONS");


    }

}
