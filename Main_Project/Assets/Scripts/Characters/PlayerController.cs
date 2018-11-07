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
    

   

    public bool IsZoneDigging { get; private set; } // If the player is blocked to zone dig (searching for destination)
    public bool IsCasting { get; private set; } // If the player is blocked while casting the dig

    private Rigidbody _rig;
    public CameraManager cameraManager;

    public DigStarter digStarter; // Digging circle under the player (used for both dig)
    public DigTarget digTarget; // Digging circle that moves around (used for the zone dig)
    public Caster caster; // Caster waiting bar that appears before a dig

    private Dig _digType; // Actual digging state (None, Linear, Zone)
    private Vector3 _targetStartingPosition; // Saves the zone digging target's position

    //-----------------------------------------------------------------------//

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
            digStarter.CheckDig(_digType);

        this.CheckSkillInteraction();
        this.CheckCamera();
        this.DiggingTest();
        Debug.Log("PLAYER IS: "+IsSafe);
    }

    //-----------------------------------------------------------------------//

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

        Debug.Log("chiamato");
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
    /// at start and end) and eventually awakes the Casting Circle
    /// </summary>
    public void LinearCheck()
    {
        if (_digType == Dig.LINEAR) // If you already pressed [LDIG]
            if (digStarter.CanDig(_digType))
            {
                caster.StartCircle(_digType);
                IsCasting = true;
            }
            else
                digStarter.StopDig(ref _digType);

        else if (_digType == Dig.ZONE) // If you press [LDIG] after [ZDIG] it cancels the digging action
            digStarter.StopDig(ref _digType);

        else // First time the player presses [LDIG]
        {
            _digType = Dig.LINEAR;
            digStarter.gameObject.SetActive(true);
            digStarter.CheckDig(_digType);
        }
    }

    /// <summary>
    /// Performs the linear dig. This is called by the
    /// Caster script, after the casting is ready.
    /// </summary>
    public void LinearDig()
    {
        transform.position = -transform.position; // Actual linear dig action

        // After digging
        digStarter.StopDig(ref _digType);
        IsCasting = false;
    }

    /// <summary>
    /// Checks the conditions for the zone dig and performs both
    /// ending point selection and the dig itself
    /// </summary>
    public void ZoneCheck()
    {
        if (IsZoneDigging) // If you already pressed [ZDIG] 2 times (activate -> valid start -> now)
            if (digTarget.CanDig())
            {
                caster.StartCircle(_digType);
                digTarget.isDigging = false;
                IsCasting = true;
            }
            else
            {
                IsZoneDigging = false;
                digTarget.StopTarget(_targetStartingPosition);
            }

        else if (_digType == Dig.ZONE) // If you already pressed [ZDIG] (activate -> now)
            if (digStarter.CanDig(_digType))
            {
                IsZoneDigging = true;
                digTarget.isDigging = true;
                digTarget.gameObject.SetActive(true);
                _targetStartingPosition = digTarget.transform.position; // Saves the position to restart the target
                digTarget.CheckTarget();
            }
            else
                digStarter.StopDig(ref _digType);

        else if (_digType == Dig.LINEAR) // If you press [LDIG] after [ZDIG] it cancels the digging action
            digStarter.StopDig(ref _digType);

        else // First time the player presses [ZDIG]
        {
            _digType = Dig.ZONE;
            digStarter.gameObject.SetActive(true);
            digStarter.CheckDig(_digType); // Type 2 for vertical dig
        }
    }

    /// <summary>
    /// Performs the zone dig. This is called by the
    /// Caster script, after the casting is ready.
    /// </summary>
    public void ZoneDig()
    {
        transform.position = digTarget.Dig();

        // After digging
        IsZoneDigging = false;
        IsCasting = false;
        digTarget.StopTarget(digStarter.transform.position);
        digStarter.StopDig(ref _digType);
    }

    /// <summary>
    /// Stub to playtest digging. Press [I] for linear dig
    /// and [O] for zone dig
    /// </summary>
    private void DiggingTest()
    {
        if (IsCasting)
            return;

        if (Input.GetKeyDown(KeyCode.I) && !IsZoneDigging)
            LinearCheck();

        if (Input.GetKeyDown(KeyCode.O))
            ZoneCheck();
    }

    #endregion
}


