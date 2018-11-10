using System.Collections.Generic;

using UnityEngine;

public enum Status { NORMAL = 0, HALF_CURSED, CURSED }
public enum Visibility { INVISIBLE = 0, WARNING, SPOTTED }
public enum CharPeriod { PREHISTORY = 0, ORIENTAL, VICTORIAN, FUTURE }

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(ItemWheel))]
[RequireComponent(typeof(DigWheel))]
public class PlayerController : MonoBehaviour {

    public Transform playerModel;

    public bool usingSkill=false;
    public bool IsMimicOrDash { get; set; }
    public bool IsSafe { get;  set; }
    public Status CurseStatus { get;  set; }
    public Visibility Visible { get;  set; }
    public CharPeriod CharacterPeriod;
    public Dictionary<string, int> items;

    public Digging dig;
    public Robot robot;

    public bool IsZoneDigging { get;  set; } // If the player is blocked to zone dig (searching for destination)

    public bool IsCasting { get; set; } // If the player is blocked while casting the dig


    private Rigidbody _rig;
    public CameraManager playerCamera;

    public DigStarter digStarter; // Digging circle under the player (used for both dig)
    public DigTarget digTarget; // Digging circle that moves around (used for the zone dig)
    public Caster caster; // Caster waiting bar that appears before a dig

    private Vector3 _targetStartingPosition; // Saves the zone digging target's position

    //-----------------------------------------------------------------------//

    void Awake() {
        IsSafe = false;
        CurseStatus = Status.NORMAL;
        Visible = Visibility.INVISIBLE;
        items = new Dictionary<string, int>(6);
        IsZoneDigging = false;      
    }

    // Use this for initialization
    void Start() {
        _rig = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<CameraManager>();
        //_digStarter = GetComponentInChildren<DigStarter>();
        //_digTarget = GetComponentInChildren<DigTarget>();
        playerModel = transform.Find("Model");
    }
    
    // Update is called once per frame
    void Update() {

        this.CheckSkillInteraction();
        this.CheckItemInteraction();
        this.CheckCamera();
        this.CheckDig();

        // Testing Zone
        this.RobotTest();

        Debug.Log("PLAYER IS: "+IsSafe);
    }

    //-----------------------------------------------------------------------//

    /// <summary>
    /// The skill is used if an input is detected
    /// </summary>
    private void CheckSkillInteraction() {
 
        if (Input.GetButtonDown("PS4_Button_O") || Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log("SKILL used");
            if (usingSkill) {
                //Skill.DeactivateSkill();

            }
            else {
                //Skill.ActivateSkill();
            }
            
        }
    }

    private void CheckItemInteraction() {//TODO button to pick an item and to select an item to use
        if (IsMimicOrDash) return;
        Debug.Log("Item picked/used");

    }
    /// <summary>
    /// If a button is clicked, enables/disables the control of the camera around the player
    /// </summary>
    private void CheckCamera(){
        if (Input.GetButtonDown("PS4_Button_RStickClick") || Input.GetKeyDown(KeyCode.Tab)) {
            Debug.Log("before " + playerCamera.IsFollowingPlayer);
            playerCamera.SetCamera();
            
            Debug.Log("after "+playerCamera.IsFollowingPlayer);
        }
        
        float rStickX = Input.GetAxis("PS4_RStick_X");
        float rStickY = Input.GetAxis("PS4_RStick_Y");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        /*if (_camera.IsFollowingPlayer) {
            rStickY = mouseY = 0f;
        }*/

        Debug.Log("move camera");
        if ((rStickX != 0 || rStickY != 0) && (mouseX == 0 && mouseY == 0)) {// if only the controller is used

            playerCamera.LookAtTarget(rStickX, rStickY);
        }
        else {
            playerCamera.LookAtTarget(mouseX, mouseY);
        }
        

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
        if (other.CompareTag("Lamp_Base")) {//if the character has entered the light of a lamp that is switched on

           
            IsSafe = true;


        }
        else if (other.CompareTag("Enemy")) {   //if the character touches an enemy trigger
                                                //READ AS: if an enemy curse the character
            Enemy touchedEnemy = other.GetComponentInParent<Enemy>();
            Debug.Log("before " + CurseStatus);
            if (CurseStatus == Status.NORMAL && !touchedEnemy.data_enemy.instant_curse) {
                CurseStatus = Status.HALF_CURSED;
                Debug.Log("after " + CurseStatus);
                return;
            }
            Debug.Log("create the enemy ");
            //if the enemy can curse the character instantly
            GameObject enemyGO = GameManager.Instance.enemies[touchedEnemy.data_enemy.level - 1]; //the levels are [1,3]
            enemyGO.GetComponent<Rigidbody>().position = _rig.position;

            enemyGO.GetComponent<Enemy>().path = touchedEnemy.path;

            //Destroy(gameObject);    //destroys the character
            GameManager.Instance.SpawnNewPlayer();
            Instantiate<GameObject>(enemyGO);//creates the enemy instead


        }


    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Lamp_Base") ) {//if the character has entered the light of a lamp that is switched on

            IsSafe = false;
            
        }
    }


    private void OnCollisionEnter(Collision collision) {
        Collider other = collision.collider;
        if (other.CompareTag("Lamp_Switch")) {
            Debug.Log("lamp_switch");
            LampBehaviour lamp = other.GetComponentInParent<LampBehaviour>();
            Debug.Log("lamp_switch: ON "+lamp.transform.position);
            if (lamp.IsEnemyLamp) {
                lamp.SwitchOffEnemyLamp();
                return;
            }

            if (lamp.IsMissingPart) return;    //if the lamp is missing the light bulb 

            if (IsMimicOrDash) return;

            lamp.SwitchOnAllyLamp();
            Debug.Log("lamp_switch: ON");

            

        }
    }

    #endregion

    #region Digging

    /// <summary>
    /// Stub to playtest digging. Press [I] for linear dig
    /// and [O] for zone dig
    /// </summary>
    private void CheckDig()
    {
        if (IsCasting)
            return;

        if (Input.GetKeyDown(KeyCode.I) && !IsZoneDigging) // [LDIG]
            dig.LinearDig();

        if (Input.GetKeyDown(KeyCode.O)) // [ZDIG]
            dig.ZoneDig();
    }

    /*
    /// <summary>
    /// Checks the conditions for the linear dig (valid terrain both
    /// at start and end) and eventually awakes the Casting Circle
    /// </summary>
    public void LinearCheck()
    {
        if (_digType == dig.LINEAR) // If you already pressed [LDIG]
            if (digStarter.CanDig(_digType))
            {
                caster.StartCircle(_digType);
                IsCasting = true;
            }
            else
                digStarter.StopDig(ref _digType);

        else if (_digType == dig.ZONE) // If you press [LDIG] after [ZDIG] it cancels the digging action
            digStarter.StopDig(ref _digType);

        else // First time the player presses [LDIG]
        {
            _digType = dig.LINEAR;
            digStarter.gameObject.SetActive(true);
            digStarter.CheckDig(_digType);
        }
    }
    */

    /*
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
    */
    /*
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

        else if (_digType == dig.ZONE) // If you already pressed [ZDIG] (activate -> now)
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

        else if (_digType == dig.LINEAR) // If you press [LDIG] after [ZDIG] it cancels the digging action
            digStarter.StopDig(ref _digType);

        else // First time the player presses [ZDIG]
        {
            _digType = dig.ZONE;
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
    */

    #endregion

    /// <summary>
    /// Stub to playtest robot. Press [P] to activate and
    /// pause robot
    /// </summary>
    private void RobotTest()
    {
        if (Input.GetKeyDown(KeyCode.P))
            if (robot.paused)
            {
                enabled = false;
                IsCasting = true;
                playerCamera.gameObject.SetActive(false);
                robot.Restart();
            }
            else
            {
                enabled = false;
                IsCasting = true;
                playerCamera.gameObject.SetActive(false);
                robot.Spawn(transform.position);
            }
    }
}


