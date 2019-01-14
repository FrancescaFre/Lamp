using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovOK : MonoBehaviour
{
    [Range(5,10)]
    [Tooltip("The player's walking speed")]
    public float walkSpeed = 8f;

    [Range(1, 5)]
    [Tooltip("The player's stealth speed")]
    public float stealthSpeed = 3f;

    //[Tooltip("The real camera's transform (must NOT be player's child)")]
    //public Transform mainCam;

    [Tooltip("The dummy player's camera transform (must be dummy's child")]
    public Transform dummyCam;

    //[Tooltip("True if attached to the dummy player (must have the same player's speed)")]
    //public bool isDummy;

    //private PlayerController _player; // The PlayerController attached to the player

    public Transform dummyPlayer;
    private Vector3 dummyOffset;

    private Rigidbody _rb; // Player's rigidbody
    private Vector3 _movement; // Direction and magnitude of the movement in a frame
    private float _horizInput, _vertInput; // Horizontal and Vertical inputs from analog sticks
    private float _stepSpeed; // How fast is the player. Is always equal to walkSpeed or stealthSpeed
      
    private Vector3 _screenForward, _screenRight, _screenUp; // Screen's global axes. They can have different impact based on the camera angle on the player

    void Start()
    {
        //_player = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody>();
        _movement = Vector3.zero;
        _stepSpeed = walkSpeed;
        dummyOffset = dummyPlayer.transform.position - transform.position;
        StartCoroutine(CorrectPlayerPositions());
    }

    private void Update()
    {
        _horizInput = Input.GetAxis(Controllers.Horizontal);
        _vertInput = Input.GetAxis(Controllers.Vertical);
    }

    private void FixedUpdate()
    {
        if (_vertInput == 0 && _horizInput == 0)
            return;
        else
            //MovePlayer();
            PleaseMoveIBegYou();
        /*
        else if (CanMove())
        {
            ComputeCamAxes();
            MovePlayer();
        } */
    }

    
    private void LateUpdate()
    {
        if (_vertInput == 0 && _horizInput == 0)
            return;
        transform.LookAt((_screenUp * _vertInput + _screenRight * _horizInput).normalized, transform.up); // FACCIATA A TERRA
        //transform.LookAt(transform.position + _movement); // SI PIEGA IN AVANTI E DA UN CERTO PUNTO IN POI SI RIBALTA
        //transform.forward = transform.TransformDirection(_horizInput, 0, _vertInput); // BROKEN

    }

    /*
    /// <summary>
    /// Checks if the player can move and how fast
    /// </summary>
    private bool CanMove()
    {
        if (_player) // If THIS component is attached to the player
            if (_player.IsZoneDigging || _player.IsCasting) // Stops the character movement when it's zone digging or it's casting the dig
                return false;

        if (Input.GetButton("PS4_L2") || Input.GetKey(KeyCode.T)) // Slows the movement if the player is pressing the stealth button
        {
            _stepSpeed = stealthSpeed;
            if (_player)
                _player.isSneaking = true;
        }
        else // Otherwise, it's normal speed
        {
            _stepSpeed = walkSpeed;
            if (_player)
                _player.isSneaking = false;
        }

        return true;
    }
    */

    /// <summary>
    /// Sets up the global screen axes with those of the DUMMY CAMERA
    /// </summary>
    private void ComputeCamAxes()
    {
        _screenForward = dummyCam.forward;
        _screenRight = dummyCam.right;
        _screenUp = dummyCam.up;
        //_screenForward.Normalize();
        //_screenRight.Normalize();
        //_screenUp.Normalize();
    }

    /*
    /// <summary>
    /// Moves the player always towards the input direction
    /// </summary>
    private void MovePlayer()
    {
        ComputeCamAxes();

        if (_vertInput > 0) // Pressing ↓ made the player literally jump backwards (so it's needed the _screenUp variable)
        {
            _movement = (_screenRight * _horizInput + _screenForward * _vertInput).normalized * walkSpeed * Time.deltaTime; // Takes camera axes to correct the direction
            transform.LookAt((_screenForward * _vertInput + _screenRight * _horizInput).normalized, transform.up); // Rotates the player in the input direction
        }
        else
        {
            _movement = (_screenRight * _horizInput + _screenUp * _vertInput).normalized * walkSpeed * Time.deltaTime; // Same, but using _screenUp to go backwards correctly
            transform.LookAt((_screenUp * _vertInput + _screenRight * _horizInput).normalized, transform.up); // Same, but using _screenUp to rotate correctly backwards
        }

        _rb.MovePosition(_rb.position + _movement); // Moves the player's rigidbody through physics
    } */
    

    /// <summary>
    /// SOMEONE HELP ME I'M CRYING
    /// </summary>
    private void PleaseMoveIBegYou()
    {
        ComputeCamAxes();

        if (_vertInput > 0)
            _movement = (_screenForward * _vertInput + _screenRight * _horizInput).normalized * _stepSpeed * Time.deltaTime;
        else
            _movement = (_screenUp * _vertInput + _screenRight * _horizInput).normalized * _stepSpeed * Time.deltaTime;

        _rb.MovePosition(_rb.position + _movement);
    }

    private IEnumerator CorrectPlayerPositions()
    {
        while (true)
        {
            if (dummyPlayer.transform.position - transform.position != dummyOffset)
            {
                Debug.Log(dummyPlayer.transform.position - transform.position);
                transform.position = dummyPlayer.transform.position - dummyOffset;
            }
            yield return new WaitForFixedUpdate();
        }
        
    }
}