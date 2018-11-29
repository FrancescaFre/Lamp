using System.Collections.Generic;

using UnityEngine;

public enum Status { NORMAL = 0, HALF_CURSED, CURSED }
public enum Visibility { INVISIBLE = 0, WARNING, SPOTTED }
public enum CharPeriod { PREHISTORY = 0, ORIENTAL, VICTORIAN, FUTURE }

/*
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(ItemWheel))]

    
Q /CIRCLE   using skill
2/TRIANGLE 	LINEAR DIGGING
3/SQUARE	ZONE DIGGING
Q/CIRCLE	ACTIVATE/DEACTIVATE SKILL
SPACE/L2 (HOLD) SNEAKY
TAB/L1	 (HOLD) NAVIGATE RADIAL MENU
ESC/PAUSE	PAUSE MENU
 
     */

public class PlayerController : MonoBehaviour {

    public CharPeriod CharacterPeriod;

    public CamManager MainCamera { get; set; }
 
    private Digging _dig;
    
    public bool usingSkill=false;
    public bool isSneaking = false;
    public bool IsMimicOrDash { get; set; }
    public bool IsSafe { get;  set; }
    public bool IsZoneDigging { get; set; } 
    public bool IsCasting { get; set; } 
    [SerializeField]
    public Status CurseStatus { get;  set; }
    public Visibility Visible { get;  set; }

    public Skill skill;

    private Rigidbody _rb;
    private int missingParts=0;
    private int keys=0;

    //#####################################################################

    void Awake() {
        IsSafe = false;
        CurseStatus = Status.NORMAL;
        Visible = Visibility.INVISIBLE;
        IsZoneDigging = false;      
    }

    // Use this for initialization
    void Start() {
        _rb = GetComponent<Rigidbody>();
        _dig = GetComponentInChildren<Digging>(includeInactive:true);
        MainCamera = FindObjectOfType<CamManager>();

        skill = GetComponentInChildren<Skill>();
    }
    
    // Update is called once per frame
    void Update() {
        this.CheckSkillInteraction();
        this.CheckItemInteraction();
        this.CheckDig();
    }

    //#####################################################################

    /// <summary>
    /// Returns true if the player can move freely
    /// </summary>
    public bool CanMove()
    {
        return !(IsZoneDigging || IsCasting);
    }

    //#####################################################################
    #region Player's Interaction/action
    /// <summary>
    /// The skill is used if an input is detected
    /// </summary>
    private void CheckSkillInteraction() {
 
        if (Input.GetButtonDown("PS4_Button_O") || Input.GetKeyDown(KeyCode.Q)) {
            if (usingSkill)
            {
                skill.DeactivateSkill();
            }
            else
            {
                skill.ActivateSkill();
                usingSkill = true; 
                Debug.Log("SKILL used");
            }
        }
    }

    private void CheckItemInteraction() {//TODO button to pick an item and to select an item to use
        if (IsMimicOrDash) return;
    }

    #endregion

    /// <summary>
    /// Sets the opposite of the current value of safety
    /// </summary>
    public void ChangeSafety() {
        IsSafe = !IsSafe;
    }

    #region Collision Detection
    //far apparire il pulsante per interagirci, per ora basta avvicinarsi per raccoglierlo
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MissingPart"))
        {
            missingParts++;
            other.gameObject.SetActive(false);
            //------ WARNING: todo
        }
        else if (other.CompareTag("Key"))
        {
            keys++;
            other.gameObject.SetActive(false);
            //------ WARNING: todo NON LO SO
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lamp_Base"))
        {//if the character has entered the light of a lamp that is switched on
            IsSafe = true;
        }

        else if (other.CompareTag("Enemy"))
        {   //if the character touches an enemy trigger
            //READ AS: if an enemy curse the character
            Debug.Log("Touched");
            Enemy touchedEnemy = other.GetComponentInParent<Enemy>();
            Debug.Log("before " + CurseStatus);
            if (CurseStatus == Status.NORMAL && !touchedEnemy.data_enemy.instant_curse)
            {
                CurseStatus = Status.HALF_CURSED;
                Debug.Log("after " + CurseStatus);
                return;
            }
            Debug.Log("create the enemy ");
            //if the enemy can curse the character instantly
            GameObject enemyGO = GameManager.Instance.enemies[touchedEnemy.data_enemy.level - 1]; //the levels are [1,3]
            enemyGO.GetComponent<Rigidbody>().position = _rb.position;

            enemyGO.GetComponent<Enemy>().path = touchedEnemy.path;

            GameManager.Instance.SpawnNewPlayer(); //destroys the character
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
            if (lamp.isEnemyLamp && lamp.isTurnedOn) { //if it is an enemy lamp AND it is turned on
                lamp.SwitchOffEnemyLamp();
                return;
            }
            
            if (lamp.hasMissingPart && missingParts > 0) {
                lamp.hasMissingPart = false;
                missingParts--;
            }
            else if (lamp.hasMissingPart) return;   //if the lamp is missing the light bulb 

            if (IsMimicOrDash) return;
            if (lamp.isTurnedOn) return;    //if the lamp is already turned on, exit
            lamp.SwitchOnAllyLamp();
        }
    }

    #endregion


    /// <summary>
    /// Stub to playtest digging. Press [I] for linear dig
    /// and [O] for zone dig
    /// </summary>
    private void CheckDig()
    {
        if (!IsCasting)
        {
            if ((Input.GetKeyDown(KeyCode.Alpha2) || Input.GetButtonDown("PS4_Button_Triangle")) && !IsZoneDigging) // [LDIG]
                _dig.LinearDig();

            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetButtonDown("PS4_Button_Square")) // [ZDIG]
                _dig.ZoneDig();
        }
    }
}


