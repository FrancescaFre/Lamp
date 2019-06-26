using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
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

    [Header("Terrain check")]
    public bool OnWater;  // True if player is walking on the water / mud
    public bool OnLeaves; // True if player is walking on the leaves / noisy terrain
    public bool OnIce; // True if player is walking on ice
    public bool OnSolidFloor; // True if player is walking on a solid floor

    private PlayerController _player; // The PlayerController attached to the player

    private Rigidbody _rb; // Player's rigidbody
    private Vector3 _movement; // Direction and magnitude of the movement in a frame
    private Vector3 _movementOnIce; // Saves the last movement direction to use it on ice
    private float _horizInput, _vertInput; // Horizontal and Vertical inputs from analog sticks
    private float _stepSpeed; // How fast is the player. Is always equal to walkSpeed or stealthSpeed
    private float _currentSpeed;// this allows to change movement speed in water based on the actual movement of the player
    private bool _wallOnIce; // True if player is sliding on ice and hits a wall

    private Transform playerModel;
   
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
    public void TutorialZeroMov(bool stop) {
        _player.runningAnimation = stop;
    }

    private void Start() {
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

        playerModel =GetComponent<DifferenceOfTerrain>().modelTransform;
 
        
        //Stamina
        staminaValue = maxStamina;
        staminaHUD = InGameHUD.Instance.InGameHUDPanel.GetComponentInChildren<StaminaHUD>();

        staminaHUD.staminaSlider.value = staminaValue;
        staminaHUD.staminaSlider.maxValue = maxStamina;
    }

    private void FixedUpdate() {
        CheckMovement();
        if (_vertInput == 0 && _horizInput == 0 && !OnIce)
            return;
        else if (_player.CanMove()) {
            CheckMovement();
            if (OnWater || OnLeaves)
                CheckTerrain();

            MovePlayer();
        }
    }

    private void Update() {
        _horizInput = Input.GetAxis(Controllers.Horizontal);
        _vertInput = Input.GetAxis(Controllers.Vertical);

        if ((_horizInput != 0 || _vertInput != 0) && (!OnIce || _wallOnIce))
            _player.isMoving = true;
        else
            _player.isMoving = false;

        AnimationUpdate();
    }
    #region Collision Handling
    private void OnTriggerEnter(Collider terrain) {
        if (terrain.CompareTag(Tags.Water)) // If player entered in a water / mud pond
            OnWater = true;

        if (terrain.CompareTag(Tags.Leaves)) // If player is walking on a noisy terrain
            OnLeaves = true;

        if (terrain.CompareTag(Tags.Ice)) // If player is walking on ice
            OnIce = true;

        if (terrain.CompareTag(Tags.Solid)) // If player is walking on a solid floor
            OnSolidFloor = true;

        if (OnIce && terrain.gameObject.layer == 11) // If hits an obstacle while on ice
            _wallOnIce = true;
    }

    private void OnTriggerStay(Collider terrain) {
        if (terrain.CompareTag(Tags.Water)) // If player entered in a water / mud pond
            OnWater = true;

        if (terrain.CompareTag(Tags.Leaves)) // If player is walking on a noisy terrain
            OnLeaves = true;

        if (terrain.CompareTag(Tags.Ice)) // If player is walking on ice
            OnIce = true;

        if (terrain.CompareTag(Tags.Solid)) // If player is walking on a solid floor
            OnSolidFloor = true;

        if (OnIce && terrain.gameObject.layer == 11) // If hits an obstacle while on ice
            _wallOnIce = true;
    }

    private void OnTriggerExit(Collider terrain) {
        if (terrain.CompareTag(Tags.Water)) // If player entered in a water / mud pond
            OnWater = false;

        if (terrain.CompareTag(Tags.Leaves)) // If player is walking on a noisy terrain
            OnLeaves = false;

        if (terrain.CompareTag(Tags.Ice)) // If player is walking on ice
            OnIce = false;

        if (terrain.CompareTag(Tags.Solid)) // If player is walking on a solid floor
            OnSolidFloor = false;

        if (OnIce && terrain.gameObject.layer == 11) // If hits an obstacle while on ice
            _wallOnIce = false;
    }

    private void OnCollisionEnter(Collision collision) {
        if (OnIce && collision.gameObject.layer == 11) // If hits an obstacle while on ice
            _wallOnIce = true;
    }

    private void OnCollisionStay(Collision collision) {
        if (OnIce && collision.gameObject.layer == 11) // If hits an obstacle while on ice
            _wallOnIce = true;
    }

    private void OnCollisionExit(Collision collision) {
        if (OnIce && collision.gameObject.layer == 11) // If hits an obstacle while on ice
            _wallOnIce = false;
    }
    #endregion

    /// <summary>
    /// Checks if the player is stealthing or not
    /// </summary>
    private void CheckMovement() {
        //STEALTH
        if (Input.GetButton(Controllers.PS4_L2) || Input.GetKey(KeyCode.Space)) // Slows the movement if the player is pressing the stealth button
        {
            _stepSpeed = stealthSpeed;
            _player.isSneaking = true;
            _player.isRunning = false;
        }

        //RUN - Stamina
        else if ((Input.GetButton(Controllers.PS4_R2) || Input.GetKey(KeyCode.LeftShift)) && (_vertInput != 0 || _horizInput != 0) && staminaValue > 0) {
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
        if (staminaValue < maxStamina && !(Input.GetButton(Controllers.PS4_R2) || Input.GetKey(KeyCode.LeftShift))) {
            staminaValue += Time.deltaTime / staminaRegainRate * staminaRegainMult;
        }

        if (staminaValue <= 0) {
            staminaValue = 0;
            _player.isRunning = false;
            _stepSpeed = walkSpeed;
        }
        _currentSpeed = _stepSpeed;
        staminaHUD.staminaSlider.value = staminaValue - maxStamina > 0 ? maxStamina : staminaValue;
    }

    /// <summary>
    /// Checks if the player is walking on a particular terrain
    /// </summary>
    private void CheckTerrain() {
        if (OnWater)
            _stepSpeed = _currentSpeed / 2f; // On the water, the player is slowed
    }


    /// <summary>
    /// Moves the player always towards the input direction
    /// </summary>
    private void MovePlayer() {
       
        if (OnIce && !_wallOnIce) {

            if (_movementOnIce != Vector3.zero)
                _movement = _movementOnIce;
            
            playerModel.LookAt(_rb.position + _movement.normalized, transform.up);
            playerModel.localEulerAngles = Vector3.up * playerModel.localEulerAngles.y;

        }
        else {

            Vector3 forward, right;
            forward = Vector3.ProjectOnPlane((transform.position - BasicCamera.instance.transform.position), transform.up).normalized;
            right = Vector3.Cross(transform.up, forward).normalized;

            _movement = (_horizInput * right + _vertInput * forward).normalized * Time.deltaTime * _stepSpeed;
            
            playerModel.LookAt(_rb.position+_movement.normalized, transform.up);
        }
        
        _rb.MovePosition(_rb.position + _movement); // Moves the player's rigidbody through physics

        _movementOnIce = _movement; // Stores the movement direction to be used on ice
    }
    //-------------------------------------------------------------------------

    //----------- ANIMATION MANAGER -------------
    private void AnimationUpdate() {
        //se  mi muovo e non scavo
        if (_player.isMoving && !DigBehaviour.instance.isZoneActive) {
            //se sono sneaky ma non è attiva l'animazione, allora sneaky
            if (!(AnimationManager.Anim_CheckBool(_player.characterAnimator, "IsMovingSneaky")) && _player.isSneaking) {
                AnimationManager.Anim_StopMovingStanding(_player.characterAnimator);
                AnimationManager.Anim_StartMovingSneaky(_player.characterAnimator);
            }

            //se non sono sneaky, ma l'animazione è attiva, allora ferma l'animazione
            if (AnimationManager.Anim_CheckBool(_player.characterAnimator, "IsMovingSneaky") && !_player.isSneaking)
                AnimationManager.Anim_StopMovingSneaky(_player.characterAnimator);

            //se sto correndo ma l'animazione della corsa non è attiva, attivala
            if (!(AnimationManager.Anim_CheckBool(_player.characterAnimator, "IsMovingRunning")) && _player.isRunning) {
                AnimationManager.Anim_StartMovingRunning(_player.characterAnimator);
            }

            //se l'animazione della corsa è attiva, ma non sto correndo, allora fermala
            if (AnimationManager.Anim_CheckBool(_player.characterAnimator, "IsMovingRunning") && !_player.isRunning)
                AnimationManager.Anim_StopMovingRunning(_player.characterAnimator);

            //sono in piedi e mi muovo (quindi cammino)
            if (!_player.isRunning && !_player.isSneaking) {
                AnimationManager.Anim_StopMovingSneaky(_player.characterAnimator);
                AnimationManager.Anim_StopMovingRunning(_player.characterAnimator);
                AnimationManager.Anim_StartMovingStanding(_player.characterAnimator);
            }
        }
        else {
            if (AnimationManager.Anim_CheckBool(_player.characterAnimator, "IsMovingRunning") || (AnimationManager.Anim_CheckBool(_player.characterAnimator, "IsMovingSneaky"))) {
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