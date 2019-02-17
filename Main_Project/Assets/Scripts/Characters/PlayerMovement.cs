using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //-------------------------------------------------------------------------

    [Range(0, 10)]
    [Tooltip("The player's walking speed")]
    public float walkSpeed = 3f;

    [Range(1, 5)]
    [Tooltip("The player's stealth speed")]
    public float stealthSpeed = 3f;

    [Range(1, 5)]
    [Tooltip("The player's running speed")]
    public float runSpeed = 5f;

    //[Tooltip("The dummy player's transform")]
    public Transform DummyPlayer { get; set; }

    //[Tooltip("The dummy player's camera transform (must be dummy's child")]
    private Transform dummyCam;

    //-------------------------------------------------------------------------

    public bool OnWater { get; set; }  // True if player is walking on the water / mud
    public bool OnLeaves { get; set; } // True if player is walking on the leaves / noisy terrain
    public bool OnIce { get; set; } // True if player is walking on ice
    public bool OnSolidFloor { get; set; } // True if player is walking on a solid floor
    public Vector3 DummyOffset { get; set; } // The distance between the player's and the dummy's positions  

    private PlayerController _player; // The PlayerController attached to the player 

    private Rigidbody _rb; // Player's rigidbody
    private Vector3 _movement; // Direction and magnitude of the movement in a frame
    private Vector3 _movementOnIce; // Saves the last movement direction to use it on ice
    private float _horizInput, _vertInput; // Horizontal and Vertical inputs from analog sticks
    private float _stepSpeed; // How fast is the player. Is always equal to walkSpeed or stealthSpeed 
    private bool _wallOnIce; // True if player is sliding on ice and hits a wall

    private Vector3 _screenForward, _screenRight, _screenUp; // Screen's global axes. They can have different impact based on the camera angle on the player
    

    /*
    private BoxCollider _verticalCollider; // The long vertical collider to check triggers on the other side of the world
    private SphereCollider _zoneCollider; // The spherical collider in ZoneDig to check lights on walls
    private CapsuleCollider _playerCollider; // The player's collider
    private List<Collider> _collisions; // When colliding with something, memorizes all the colliders involved
    */

    //STAMINA
    public StaminaHUD staminaHUD;
    public float maxStamina = 5f;
    [Tooltip("The higher the value, the slower it is consumed.")]
    public float staminaFallRate = 2f;
    public float staminaFallMult = 1f;
    [Tooltip("The higher the value, the slower it returns back to normal.")]
    public float staminaRegainRate = 3f;
    public float staminaRegainMult = 1f;
    public float staminaValue;


    //-------------------------------------------------------------------------

    void Start()
    {
        DummyPlayer = GameObject.FindGameObjectWithTag(Tags.DummyPlayer).transform;
        dummyCam = GameObject.FindGameObjectWithTag(Tags.DummyCam).transform;

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
        DummyOffset = DummyPlayer.transform.position - transform.position;

        StartCoroutine(CorrectPlayerPositions());
        StartCoroutine(CorrectPlayerRotation());

        /*
        _verticalCollider = GetComponentInChildren<VerticalDig>().GetComponent<BoxCollider>();
        _zoneCollider = GetComponentInChildren<ZoneDig>().GetComponent<SphereCollider>();
        _playerCollider = GetComponent<CapsuleCollider>();
        _collisions = new List<Collider>();
        */

        //Stamina
        staminaValue = maxStamina;
        staminaHUD = InGameHUD.Instance.InGameHUDPanel.GetComponentInChildren<StaminaHUD>();
        
        staminaHUD.staminaSlider.value = staminaValue;
        staminaHUD.staminaSlider.maxValue = maxStamina;
    }


    private void FixedUpdate()
    {
        CheckMovement();
        if (_vertInput == 0 && _horizInput == 0 && !OnIce)
            return;
        else if (_player.CanMove())
        {
            CheckMovement();
            if (OnWater || OnLeaves)
                CheckTerrain();
            ComputeCamAxes();
            MovePlayer();
        }
    }

    private void Update()
    {
        _horizInput = Input.GetAxis(Controllers.Horizontal);
        _vertInput = Input.GetAxis(Controllers.Vertical);

        AnimationUpdate();
    }


    private void OnTriggerEnter(Collider terrain)
    {
        if (terrain.CompareTag(Tags.Water)) // If player entered in a water / mud pond
            OnWater = true;

        if (terrain.CompareTag(Tags.Leaves)) // If player is walking on a noisy terrain
            OnLeaves = true;

        if (terrain.CompareTag(Tags.Ice)) // If player is walking on ice
            OnIce = true;

        if (terrain.CompareTag(Tags.Solid)) // If player is walking on a solid floor
            OnSolidFloor = true;
    }

    private void OnTriggerStay(Collider terrain)
    {
        if (terrain.CompareTag(Tags.Water)) // If player entered in a water / mud pond
            OnWater = true;

        if (terrain.CompareTag(Tags.Leaves)) // If player is walking on a noisy terrain
            OnLeaves = true;

        if (terrain.CompareTag(Tags.Ice)) // If player is walking on ice
            OnIce = true;

        if (terrain.CompareTag(Tags.Solid)) // If player is walking on a solid floor
            OnSolidFloor = true;
    }

    private void OnTriggerExit(Collider terrain)
    {
        if (terrain.CompareTag(Tags.Water)) // If player entered in a water / mud pond
            OnWater = false;

        if (terrain.CompareTag(Tags.Leaves)) // If player is walking on a noisy terrain
            OnLeaves = false;

        if (terrain.CompareTag(Tags.Ice)) // If player is walking on ice
            OnIce = false;

        if (terrain.CompareTag(Tags.Solid)) // If player is walking on a solid floor
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
    /// 

    private void CheckMovement()
    {
        //STEALTH
        if (Input.GetButton(Controllers.PS4_L2) || Input.GetKey(KeyCode.Space)) // Slows the movement if the player is pressing the stealth button
        {
            _stepSpeed = stealthSpeed;
            _player.isSneaking = true;
            _player.isRunning = false; 
        }

        //RUN - Stamina
        else if ((Input.GetButton(Controllers.PS4_R2) || Input.GetKey(KeyCode.LeftShift)) && (_vertInput != 0 || _horizInput != 0) && staminaValue > 0)
        {
            staminaValue -= Time.deltaTime / staminaFallRate * staminaFallMult;
            _player.isRunning = true;
            _player.isSneaking = false; 
            _stepSpeed = runSpeed;
            
        }

        //WALK
        else // Otherwise, it's normal speed
        {
            _stepSpeed = walkSpeed;
            _player.isSneaking = false;
            _player.isRunning = false;
        }

        //Stamina Controll
        if (staminaValue < maxStamina && !(Input.GetButton(Controllers.PS4_R2) || Input.GetKey(KeyCode.LeftShift)))
        {
            staminaValue += Time.deltaTime / staminaRegainRate * staminaRegainMult;
        }

        if (staminaValue <= 0)
        {
            staminaValue = 0;
            _player.isRunning = false; 
            _stepSpeed = walkSpeed;
        }

        staminaHUD.staminaSlider.value = staminaValue - maxStamina > 0 ? maxStamina : staminaValue;
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
        //Debug.Log(OnIce + " " + _wallOnIce);
        if (OnIce && !_wallOnIce) // When walking on ice, there's no need to compute input's direction
        {
            if (_movementOnIce != Vector3.zero) // Sometimes a zero input vector can happen, and this blocks the player from moving anywhere. Solved ↓
                _movement = _movementOnIce;
            else
            {
                // Normal Movement even if it's on ice. This solves the ice glitch
                if (_vertInput > 0)
                    _movement = (_screenForward * _vertInput + _screenRight * _horizInput).normalized * _stepSpeed * Time.deltaTime;
                else
                    _movement = (_screenUp * _vertInput + _screenRight * _horizInput).normalized * _stepSpeed * Time.deltaTime;
            }
        }
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
        DummyPlayer.position = other.gameObject.transform.position + DummyOffset;
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
            if (DummyPlayer.transform.position - transform.position != DummyOffset)
            {
                //transform.position = dummyPlayer.transform.position - _dummyOffset; // Less adjustments, but harder to force teleports and transform movements outside
                DummyPlayer.transform.position = transform.position + DummyOffset; // More adjustments, but the player can now do the fuck it wants with its transform
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
        //se non mi muovo e non scavo
        if ((_horizInput != 0 || _vertInput != 0) && !_player.IsZoneDigging)
        {
            //se sono sneaky ma non è attiva l'animazione, allora sneaky
            if (!(AnimationManager.Anim_CheckBool(_player.characterAnimator, "IsMovingSneaky")) && _player.isSneaking)
            {
                AnimationManager.Anim_StopMovingStanding(_player.characterAnimator);
                AnimationManager.Anim_StartMovingSneaky(_player.characterAnimator);
            }

            //se non sono sneaky, ma l'animazione è attiva, allora ferma l'animazione
            if (AnimationManager.Anim_CheckBool(_player.characterAnimator, "IsMovingSneaky") && !_player.isSneaking)
                AnimationManager.Anim_StopMovingSneaky(_player.characterAnimator);

            //se sto correndo ma l'animazione della corsa non è attiva, attivala
            if (!(AnimationManager.Anim_CheckBool(_player.characterAnimator, "IsMovingRunning")) && _player.isRunning)
            {
                AnimationManager.Anim_StartMovingRunning(_player.characterAnimator);
            }

            //se l'animazione della corsa è attiva, ma non sto correndo, allora fermala
            if (AnimationManager.Anim_CheckBool(_player.characterAnimator, "IsMovingRunning") && !_player.isRunning)
                AnimationManager.Anim_StopMovingRunning(_player.characterAnimator);

            //sono in piedi e mi muovo (quindi cammino) 
            if (!_player.isRunning && !_player.isSneaking)
            {
                AnimationManager.Anim_StopMovingSneaky(_player.characterAnimator);
                AnimationManager.Anim_StopMovingRunning(_player.characterAnimator);
                AnimationManager.Anim_StartMovingStanding(_player.characterAnimator);
            }
        }

        else
        {
            if (AnimationManager.Anim_CheckBool(_player.characterAnimator, "IsMovingRunning") || (AnimationManager.Anim_CheckBool(_player.characterAnimator, "IsMovingSneaky")))
            {
                AnimationManager.Anim_StopMovingSneaky(_player.characterAnimator);
                AnimationManager.Anim_StopMovingRunning(_player.characterAnimator);
            }

            //se NON sto camminando, ma sono sneaky
            if (_player.isSneaking && !AnimationManager.Anim_CheckBool(_player.characterAnimator, "IsMovingStanding"))
                AnimationManager.Anim_StopMovingSneaky(_player.characterAnimator);

            AnimationManager.Anim_StopMovingStanding(_player.characterAnimator);
        }
    }
}

   