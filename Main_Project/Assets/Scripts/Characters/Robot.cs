using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour {

    [Range(5, 10)]
    public float walkSpeed = 8f;
    public PlayerController player;
    public Camera cam;
    public bool paused;

    private Rigidbody _rb;
    private Vector3 _moveDir;
    private Vector3 _movement;
    private float _horiz_axis;
    private float _vert_axis;

    public void ActivateSkill()
    {
        //disable the control to the player
        //enables robot at the player position
        //if player press again the button of the skill, DEACTIVATE ROBOT
        //else wait x seconds and DEACIVATE ROBOT
    }

    void DeactivateSkill()
    {
        //return the control to the player
        //becomes pickable (update a flag in the robot), and if picked, becomes disabled (the go of the robot)
    }

    // Use this for initialization
    void Start () {
        _movement = Vector3.zero;
        _moveDir = Vector3.zero;
        _horiz_axis = 0f;
        _vert_axis = 0f;
        _rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        MoveRobot();
        if (Input.GetKeyDown(KeyCode.P))
            ShutDown();
    }

    /// <summary>
    /// Spawns the robot in front of the player
    /// </summary>
    public void Spawn(Vector3 playerPosition)
    {
        cam.gameObject.SetActive(true);
        transform.position = playerPosition; // TODO
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Gives back the control to the sleeping robot
    /// </summary>
    public void Restart()
    {
        cam.gameObject.SetActive(true);
        enabled = true;
    }

    /// <summary>
    /// Same movement code as the character
    /// </summary>
    private void MoveRobot()
    {
        this._horiz_axis = Input.GetAxis("Horizontal");
        this._vert_axis = Input.GetAxis("Vertical");
        _moveDir.Set(_horiz_axis, 0f, _vert_axis);
        _moveDir.Normalize();
        _movement = transform.TransformDirection(_moveDir) * walkSpeed * Time.deltaTime;
        _rb.MovePosition(_rb.position + _movement);
    }
    
    /// <summary>
    /// Shuts down the robot and returns control to the character
    /// </summary>
    private void ShutDown()
    {
        cam.gameObject.SetActive(false);
        enabled = false;
        paused = true;

        player.playerCamera.gameObject.SetActive(true);
        player.IsCasting = false;
        player.enabled = true;
    }
}
