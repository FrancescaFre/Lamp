using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //-------------------------------------------------------------------------

    [Range(5, 10)]
    [Tooltip("The player's walking speed")]
    public float walkSpeed = 8f;

    [Range(1, 5)]
    [Tooltip("The player's stealth speed")]
    public float stealthSpeed = 3f;

    //[Tooltip("The dummy player's transform")]
    private Transform dummyPlayer;

    //[Tooltip("The dummy player's camera transform (must be dummy's child")]
    private Transform dummyCam;

    //-------------------------------------------------------------------------

    private bool _onWater; // True if player is walking on the water / mud
    private bool _onLeaves; // True if player is walking on the leaves / noisy terrain
    private bool _onIce; // True if player is walking on ice

    private PlayerController _player; // The PlayerController attached to the player 

    private Rigidbody _rb; // Player's rigidbody
    private Vector3 _movement; // Direction and magnitude of the movement in a frame
    private Vector3 _movementOnIce; // Saves the last movement direction to use it on ice
    private float _horizInput, _vertInput; // Horizontal and Vertical inputs from analog sticks
    private float _stepSpeed; // How fast is the player. Is always equal to walkSpeed or stealthSpeed 

    private Vector3 _screenForward, _screenRight, _screenUp; // Screen's global axes. They can have different impact based on the camera angle on the player
    private Vector3 _dummyOffset; // The distance between the player's and the dummy's positions

    //-------------------------------------------------------------------------

    void Start()
    {
        dummyPlayer = GameObject.FindGameObjectWithTag("DummyPlayer").transform;
        dummyCam = GameObject.FindGameObjectWithTag("DummyCam").transform;

        _player = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody>();
        _movement = Vector3.zero;
        _movementOnIce = Vector3.zero;
        _stepSpeed = walkSpeed;
        _onWater = false;
        _onLeaves = false;
        _onIce = false;
        _dummyOffset = dummyPlayer.transform.position - transform.position;
        StartCoroutine(CorrectPlayerPositions());
        StartCoroutine(CorrectPlayerRotation());
    }

    private void FixedUpdate()
    {
        if (_vertInput == 0 && _horizInput == 0 && !_onIce)
            return;
        else if (_player.CanMove())
        {
            CheckStealth();
            if (_onWater || _onLeaves)
                CheckTerrain();
            ComputeCamAxes();
            MovePlayer();
        }
    }

    private void Update()
    {
        _horizInput = Input.GetAxis("Horizontal");
        _vertInput = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter(Collider terrain)
    {
        if (terrain.CompareTag("Water")) // If player entered in a water / mud pond
            _onWater = true;

        if (terrain.CompareTag("Leaves")) // If player is walking on a noisy terrain
            _onLeaves = true;

        if (terrain.CompareTag("Ice")) // If player is walking on ice
            _onIce = true;
    }

    private void OnTriggerExit(Collider terrain)
    {
        if (terrain.CompareTag("Water")) // If player entered in a water / mud pond
            _onWater = false;

        if (terrain.CompareTag("Leaves")) // If player is walking on a noisy terrain
            _onLeaves = false;

        if (terrain.CompareTag("Ice")) // If player is walking on ice
            _onIce = false;
    }

    // USE THIS OR CorrectPlayerRotation() TO ROTATE THE PLAYER
    /*
    private void LateUpdate()
    {
        if (_vertInput == 0 && _horizInput == 0)
            return;
        //transform.LookAt((_screenUp * _vertInput + _screenRight * _horizInput).normalized, transform.up); // FACCIATA A TERRA
        //transform.LookAt(transform.position + _movement); // SI PIEGA IN AVANTI E DA UN CERTO PUNTO IN POI SI RIBALTA
        //transform.forward = transform.TransformDirection(_horizInput, 0, _vertInput); // BROKEN
    } */

    //-------------------------------------------------------------------------

    /// <summary>
    /// Checks if the player is stealthing or not
    /// </summary>
    private void CheckStealth()
    {
        if (Input.GetButton("PS4_L2") || Input.GetKey(KeyCode.T)) // Slows the movement if the player is pressing the stealth button
        {
            _stepSpeed = stealthSpeed;
            _player.isSneaking = true;
        }
        else // Otherwise, it's normal speed
        {
            _stepSpeed = walkSpeed;
            _player.isSneaking = false;
        }
    }

    /// <summary>
    /// Checks if the player is walking on a particular terrain
    /// </summary>
    private void CheckTerrain()
    {
        if (_onWater)
            _stepSpeed = stealthSpeed / 2f; // On the water, the player is slowed
        else // _onLeaves
            _player.isSneaking = false; // On the leaves, the player is noisy 
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
        if (_onIce) // When walking on ice, there's no need to compute input's direction
            _movement = _movementOnIce;
        else if (_vertInput > 0) // Pressing ↓ made the player literally jump backwards (so it's needed the _screenUp variable)
            _movement = (_screenForward * _vertInput + _screenRight * _horizInput).normalized * _stepSpeed * Time.deltaTime; // Takes camera axes to correct the direction
        else
            _movement = (_screenUp * _vertInput + _screenRight * _horizInput).normalized * _stepSpeed * Time.deltaTime; // Same, but using _screenUp to go backwards correctly

        _rb.MovePosition(_rb.position + _movement); // Moves the player's rigidbody through physics
        dummyPlayer.gameObject.GetComponent<Rigidbody>().MovePosition(dummyPlayer.gameObject.GetComponent<Rigidbody>().position + _movement); // Moves the dummy as well 

        _movementOnIce = _movement; // Stores the movement direction to be used on ice
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// Does its best to adjust position bugs
    /// </summary>
    /// <returns></returns>
    private IEnumerator CorrectPlayerPositions()
    {
        while (true)
        {
            if (dummyPlayer.transform.position - transform.position != _dummyOffset)
            {
                //transform.position = dummyPlayer.transform.position - _dummyOffset; // Less adjustments, but harder to force teleports and transform movements outside
                dummyPlayer.transform.position = transform.position + _dummyOffset; // More adjustments, but the player can now do the fuck it wants with its transform
            }
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Does its best to adjust the transform rotation bug
    /// </summary>
    /// <returns></returns>
    private IEnumerator CorrectPlayerRotation()
    {
        while (true)
        {
            if ((_horizInput != 0 || _vertInput != 0) && _player.CanMove()) // OMFG IT WORKS GOD BLESS COROUTINES
            {
                transform.LookAt((_screenUp * _vertInput + _screenRight * _horizInput).normalized, transform.up);
                transform.Rotate(-88, 0, 0, Space.Self); // DON'T ASK ME WHY 88 WORKS BETTER
            }
            yield return new WaitForFixedUpdate();
        }
    }
}