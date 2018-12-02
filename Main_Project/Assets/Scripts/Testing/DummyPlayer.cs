using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    [Tooltip("The dummy player's camera transform (must be dummy's child")]
    public Transform dummyCam;

    [Tooltip("Attach the real player to this")]
    public PlayerMovement player;

    private Rigidbody _rb; // Player's rigidbody
    private Vector3 _movement; // Direction and magnitude of the movement in a frame
    private float _horizInput, _vertInput; // Horizontal and Vertical inputs from analog sticks
    private float _walkSpeed, _stealthSpeed; // Takes both speed values from the real player
    private float _stepSpeed; // How fast is the player. Is always equal to _walkSpeed or _stealthSpeed

    private Vector3 _screenForward, _screenRight, _screenUp; // Screen's global axes. They can have different impact based on the camera angle on the player

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _movement = Vector3.zero;
        _stealthSpeed = player.stealthSpeed;
        _walkSpeed = 8f;
    }

    private void Update()
    {
        _horizInput = Input.GetAxis("Horizontal");
        _vertInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (_vertInput == 0 && _horizInput == 0)
            return;
        else if (CanMove())
            MovePlayer();
    }

    /// <summary>
    /// Checks if the player can move and how fast
    /// </summary>
    private bool CanMove()
    {
        if (Input.GetButton("PS4_L2") || Input.GetKey(KeyCode.T)) // Slows the movement if the player is pressing the stealth button
            _stepSpeed = _stealthSpeed;
        else // Otherwise, it's normal speed
            _stepSpeed = _walkSpeed;

        return true;
    }

    /// <summary>
    /// Sets up the global screen axes with those of the DUMMY CAMERA
    /// </summary>
    private void ComputeCamAxes()
    {
        _screenForward = dummyCam.forward;
        _screenRight = dummyCam.right;
        _screenUp = dummyCam.up;
    }

    /// <summary>
    /// Moves the player always towards the input direction
    /// </summary>
    private void MovePlayer()
    {
        ComputeCamAxes();

        if (_vertInput > 0) // Pressing ↓ made the player literally jump backwards (so it's needed the _screenUp variable)
            _movement = (_screenForward * _vertInput + _screenRight * _horizInput).normalized * _stepSpeed * Time.deltaTime; // Takes camera axes to correct the direction
        else
            _movement = (_screenUp * _vertInput + _screenRight * _horizInput).normalized * _stepSpeed * Time.deltaTime; // Same, but using _screenUp to go backwards correctly

        _rb.MovePosition(_rb.position + _movement); // Moves the player's rigidbody through physics
    }
}

