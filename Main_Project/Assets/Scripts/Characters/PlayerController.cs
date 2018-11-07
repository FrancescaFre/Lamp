using System.Collections.Generic;
using System;
using UnityEngine;

public enum Status { NORMAL = 0, HALF_CURSED, CURSED }
public enum Visibility { INVISIBLE = 0, WARNING, SPOTTED }

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(ItemWheel))]
[RequireComponent(typeof(DigWheel))]
public class PlayerController : MonoBehaviour {

    public bool IsSafe { get; private set; }
    public Status CurseStatus { get; private set; }
    public Visibility Visible { get; private set; }
    

   

    public bool IsZoneDigging { get; private set; }

    private Rigidbody _rig;
    public CameraManager cameraManager;

    public DigStarter _digStarter;  // Digging circle under the player (used for both dig)
    public DigTarget _digTarget;  // Digging circle that moves around (used for the zone dig)

    private Dig _digType;  // Actual digging state
    private Vector3 _targetStartingPosition;  // Saves the zone digging target's position

    void Awake() {
        IsSafe = false;
        CurseStatus = Status.NORMAL;
        Visible = Visibility.INVISIBLE;
        
        IsZoneDigging = false;
        cameraManager = GetComponentInChildren<CameraManager>();
    }

    // Use this for initialization
    void Start() {
        _rig = GetComponent<Rigidbody>();
        
        //_digStarter = GetComponentInChildren<DigStarter>();
        //_digTarget = GetComponentInChildren<DigTarget>();
        _digType = Dig.NONE;
        
    }
    
    // Update is called once per frame
    void Update() {
        // With the dig active, check the circle color on the ground
        if (_digType != Dig.NONE)
            _digStarter.CheckDig(_digType);


        this.CheckSkillInteraction();
        this.CheckCamera();
        this.DiggingTest();
        Debug.Log("PLAYER IS: "+IsSafe);
    }
    




    /// <summary>
    /// The skill is used if an input is detected
    /// </summary>
    private void CheckSkillInteraction() {
        if (Input.GetButtonDown("PS4_Button_O") || Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log("SKILL used");
        }
    }
    /// <summary>
    /// If a button is clicked, enables/disables the control of the camera around the player
    /// </summary>
    private void CheckCamera(){
        if (Input.GetButtonDown("PS4_Button_RStickClick") || Input.GetKeyDown(KeyCode.Tab)) {
            Debug.Log("before " + cameraManager.IsFollowingPlayer);
            cameraManager.SetCamera();
            
            Debug.Log("after "+cameraManager.IsFollowingPlayer);
        }
        
        float rStickX = Input.GetAxis("PS4_RStick_X");
        float rStickY = Input.GetAxis("PS4_RStick_Y");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (cameraManager.IsFollowingPlayer) {// forbid the camera to go above or below the player
            rStickY = mouseY = 0f;
        }

        Debug.Log("move camera");
        if ((rStickX != 0 || rStickY != 0) && (mouseX == 0 && mouseY == 0)) {// if only the controller is used

            cameraManager.LookAtTarget(rStickX, rStickY);
        }
        else {
            cameraManager.LookAtTarget(mouseX, mouseY);
        }
        

    }

    /// <summary>
    /// Change the state of the curse of the character
    /// </summary>
    /// <param name="stat">New state of the curse (ENUM)</param>
    public void ChangeStatus(Status stat) {
        this.CurseStatus = stat;
    }

    /// <summary>
    /// Change the visibility of the character
    /// </summary>
    /// <param name="vis">New visibility of the character\</param>
    public void ChangeVisibility(Visibility vis) {
        this.Visible = vis;
    }
    /// <summary>
    /// Sets the opposite of the current value of safety
    /// </summary>
    public void ChangeSafety() {
        IsSafe = !IsSafe;
        Debug.Log("chiamato safety");
    }


    #region Collision Detection

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Lamp_Base")|| other.CompareTag("Lamp_Switch")) {//if the character has entered the light of a lamp that is switched on
            
            if (other.CompareTag("Lamp_Switch")) {
                LampBehaviour lamp = other.GetComponent<LampBehaviour>();
                
                if (lamp.IsEnemyLamp) { //if the player touches an enemy lamp, it will be switched off 
                    lamp.SwitchOffEnemyLamp();
                    //TODO decrease the number of enemy maps turned on
                    return;
                }

                
                if (lamp.IsMissingPart) return;    //if the lamp is missing the light bulb 

                lamp.SwitchOnAllyLamp();
                Debug.Log("lamp_switch: ON");
                
            }
            IsSafe = true;


        }

    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Lamp_Base") ) {//if the character has entered the light of a lamp that is switched on

            IsSafe = false;
            
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Enemy")) {// if the player touches an enemy
            Enemy touchedEnemy = collision.gameObject.GetComponent<Enemy>();
            GameObject enemyGO = GameManager.Instance.enemies[touchedEnemy.data_enemy.level]; 

        }
    }

    #endregion

    #region Digging

    /// <summary>
    /// Checks the conditions for the linear dig (valid terrain both
    /// at start and end) and performs the vertical teleport
    /// </summary>
    public void LinearDig()
    {
        if (_digType == Dig.LINEAR) // If you already pressed shift
            if (_digStarter.CanDig(_digType))
            {
                _digStarter.Dig();
                transform.position = -transform.position;

                // After digging
                _digStarter.StopDig(ref _digType);
            }
            else
                _digStarter.StopDig(ref _digType); // Also resets digType to 0

        else if (_digType == Dig.LINEAR) // If you press shift after ctrl it cancels the digging action
            _digStarter.StopDig(ref _digType);
        else
        {
            _digType = Dig.LINEAR;
            _digStarter.gameObject.SetActive(true);
            _digStarter.CheckDig(_digType); // Type 1 for vertical dig
        }
    }

    /// <summary>
    /// Checks the conditions for the zone dig and performs both
    /// ending point selection and the dig itself
    /// </summary>
    public void ZoneDig()
    {
        if (IsZoneDigging) // If you're searching for a target to dig
            if (_digTarget.CanDig())
            {
                transform.position = _digTarget.Dig();

                // After digging
                IsZoneDigging = false;
                _digTarget.StopTarget(_digStarter.transform.position);
                _digStarter.StopDig(ref _digType);
            }
            else
            {
                IsZoneDigging = false;
                _digTarget.StopTarget(_targetStartingPosition);
            }
        else if (_digType == Dig.ZONE) // If you already pressed ctrl
            if (_digStarter.CanDig(_digType))
            {
                IsZoneDigging = true;
                _digTarget.isDigging = true;
                _digTarget.gameObject.SetActive(true);
                _targetStartingPosition = _digTarget.transform.position; // Saves the position to restart the target
                _digTarget.CheckTarget();
            }
            else
                _digStarter.StopDig(ref _digType);

        else if (_digType == Dig.LINEAR) // If you press ctrl after shift it cancels the digging action
            _digStarter.StopDig(ref _digType);
        else
        {
            _digType = Dig.ZONE;
            _digStarter.gameObject.SetActive(true);
            _digStarter.CheckDig(_digType); // Type 2 for vertical dig
        }
    }

    private void DiggingTest()
    {
        if (Input.GetKeyDown(KeyCode.I) && !IsZoneDigging)
            LinearDig();

        if (Input.GetKeyDown(KeyCode.O))
            ZoneDig();
    }

    #endregion
}


