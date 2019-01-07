using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //-------------------------------------------------------------------------

    [Range(0, 10)]
    [Tooltip("The player's walking speed")]
    public float walkSpeed = 8f;

    [Range(1, 5)]
    [Tooltip("The player's stealth speed")]
    public float stealthSpeed = 3f;

    //[Tooltip("The dummy player's transform")]
    public Transform DummyPlayer { get; set; }

    //[Tooltip("The dummy player's camera transform (must be dummy's child")]
    private Transform dummyCam;

    //-------------------------------------------------------------------------

    public bool OnWater { get; set; }  // True if player is walking on the water / mud
    public bool OnLeaves { get; set; } // True if player is walking on the leaves / noisy terrain
    public bool OnIce { get; set; } // True if player is walking on ice
    public bool OnSolidFloor { get; set; } // True if player is walking on a solid floor
    private bool _wallOnIce; // True if player is sliding on ice and hits a wall

    private PlayerController _player; // The PlayerController attached to the player 

    private Rigidbody _rb; // Player's rigidbody
    private Vector3 _movement; // Direction and magnitude of the movement in a frame
    private Vector3 _movementOnIce; // Saves the last movement direction to use it on ice
    private float _horizInput, _vertInput; // Horizontal and Vertical inputs from analog sticks
    private float _stepSpeed; // How fast is the player. Is always equal to walkSpeed or stealthSpeed 

    private Vector3 _screenForward, _screenRight, _screenUp; // Screen's global axes. They can have different impact based on the camera angle on the player
    private Vector3 _dummyOffset; // The distance between the player's and the dummy's positions

    /*
    private BoxCollider _verticalCollider; // The long vertical collider to check triggers on the other side of the world
    private SphereCollider _zoneCollider; // The spherical collider in ZoneDig to check lights on walls
    private CapsuleCollider _playerCollider; // The player's collider
    private List<Collider> _collisions; // When colliding with something, memorizes all the colliders involved
    */

    //-------------------------------------------------------------------------

    void Start()
    {
        DummyPlayer = GameObject.FindGameObjectWithTag("DummyPlayer").transform;
        dummyCam = GameObject.FindGameObjectWithTag("DummyCam").transform;

        _player = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody>();
        _movement = Vector3.zero;
        _movementOnIce = Vector3.zero;
        _stepSpeed = walkSpeed;
        OnWater = false;
        OnLeaves = false;
        OnIce = false;
        OnSolidFloor = false;
        _wallOnIce = false;
        _dummyOffset = DummyPlayer.transform.position - transform.position;
        

        StartCoroutine(CorrectPlayerPositions());
        StartCoroutine(CorrectPlayerRotation());

        /*
        _verticalCollider = GetComponentInChildren<VerticalDig>().GetComponent<BoxCollider>();
        _zoneCollider = GetComponentInChildren<ZoneDig>().GetComponent<SphereCollider>();
        _playerCollider = GetComponent<CapsuleCollider>();
        _collisions = new List<Collider>();
        */
    }

    private void FixedUpdate()
    {
        CheckStealth();
        if (_vertInput == 0 && _horizInput == 0 && !OnIce)
            return;
        else if (_player.CanMove())
        {
            CheckStealth();
            if (OnWater || OnLeaves)
                CheckTerrain();
            ComputeCamAxes();
            MovePlayer();
        }
    }

    private void Update()
    {
        _horizInput = Input.GetAxis("Horizontal");
        _vertInput = Input.GetAxis("Vertical");

        AnimationUpdate();
    }

    
    private void OnTriggerEnter(Collider terrain)
    {
        if (terrain.CompareTag("Water")) // If player entered in a water / mud pond
            OnWater = true;

        if (terrain.CompareTag("Leaves")) // If player is walking on a noisy terrain
            OnLeaves = true;

        if (terrain.CompareTag("Ice")) // If player is walking on ice
            OnIce = true;

        if (terrain.CompareTag("Solid")) // If player is walking on a solid floor
            OnSolidFloor = true;
    }

    private void OnTriggerStay(Collider terrain)
    {
        if (terrain.CompareTag("Water")) // If player entered in a water / mud pond
            OnWater = true;

        if (terrain.CompareTag("Leaves")) // If player is walking on a noisy terrain
            OnLeaves = true;

        if (terrain.CompareTag("Ice")) // If player is walking on ice
            OnIce = true;

        if (terrain.CompareTag("Solid")) // If player is walking on a solid floor
            OnSolidFloor = true;
    }

    private void OnTriggerExit(Collider terrain)
    {
        if (terrain.CompareTag("Water")) // If player entered in a water / mud pond
            OnWater = false;

        if (terrain.CompareTag("Leaves")) // If player is walking on a noisy terrain
            OnLeaves = false;

        if (terrain.CompareTag("Ice")) // If player is walking on ice
            OnIce = false;

        if (terrain.CompareTag("Solid")) // If player is walking on a solid floor
            OnSolidFloor = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (OnIce && collision.gameObject.layer == 11) // If hits an obstacle while on ice
            _wallOnIce = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (OnIce && collision.gameObject.layer == 11) // If hits an obstacle while on ice
            _wallOnIce = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (OnIce && collision.gameObject.layer == 11) // If hits an obstacle while on ice
            _wallOnIce = false;

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
        if (Input.GetButton("PS4_L2") || Input.GetKey(KeyCode.Space)) // Slows the movement if the player is pressing the stealth button
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
        if (OnWater)
            _stepSpeed = stealthSpeed / 2f; // On the water, the player is slowed

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
        if (OnIce && !_wallOnIce) // When walking on ice, there's no need to compute input's direction
            _movement = _movementOnIce;
        else if (_vertInput > 0) // Pressing ↓ made the player literally jump backwards (so it's needed the _screenUp variable)
            _movement = (_screenForward * _vertInput + _screenRight * _horizInput).normalized * _stepSpeed * Time.deltaTime; // Takes camera axes to correct the direction
        else
            _movement = (_screenUp * _vertInput + _screenRight * _horizInput).normalized * _stepSpeed * Time.deltaTime; // Same, but using _screenUp to go backwards correctly

        _rb.MovePosition(_rb.position + _movement); // Moves the player's rigidbody through physics
        DummyPlayer.gameObject.GetComponent<Rigidbody>().MovePosition(DummyPlayer.gameObject.GetComponent<Rigidbody>().position + _movement); // Moves the dummy as well 

        _movementOnIce = _movement; // Stores the movement direction to be used on ice
    }

    /// <summary>
    /// Changes the dummyplayer's position with the new character's one
    /// </summary>
    /// <param name="other"></param>
    public void BatonPass(PlayerMovement other)
    {
        DummyPlayer.position = other.gameObject.transform.position + _dummyOffset;
        other.DummyPlayer = DummyPlayer;
        DummyPlayer = null;
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
            if (DummyPlayer.transform.position - transform.position != _dummyOffset)
            {
                //transform.position = dummyPlayer.transform.position - _dummyOffset; // Less adjustments, but harder to force teleports and transform movements outside
                DummyPlayer.transform.position = transform.position + _dummyOffset; // More adjustments, but the player can now do the fuck it wants with its transform
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

    //----------- ANIMATION MANAGER -------------
    private void AnimationUpdate()
    {
        if ((_horizInput != 0 || _vertInput != 0) && !_player.IsZoneDigging)
            if (_player.isSneaking)
                AnimationManager.Anim_StartMovingSneaky(this.transform);
            else if (AnimationManager.Anim_CheckBool(this.transform, "IsMovingSneaky") && !_player.isSneaking)
                AnimationManager.Anim_StopMovingSneaky(this.transform);
            else
                AnimationManager.Anim_StartMovingStanding(this.transform);
        else
        {
            if(_player.isSneaking && !AnimationManager.Anim_CheckBool(this.transform, "IsMovingStanding"))
            AnimationManager.Anim_StopMovingSneaky(this.transform);

            AnimationManager.Anim_StopMovingStanding(this.transform);
        }
    }
}