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
    public Skill skill;

    public int digCount = 100;
    
    public bool usingSkill = false;
    public bool isSneaking = false;

    [SerializeField]
    public Status CurseStatus { get; set; }
    public Visibility Visible { get; set; }
    public CameraManager MainCamera { get; set; }
    public VerticalDig VDig { get; set; }
    public ZoneDig ZDig { get; set; }

    public bool IsMimicOrDash { get; set; }
    public bool IsSafe { get;  set; }
    public bool IsZoneDigging { get; set; } 
    public bool IsCasting { get; set; }
    public bool runningAnimation = false;

    private Rigidbody _rb;
    private int _missingParts = 0;
    public int keys = 0;

    public ParticleSystem halfCurseEffect;
    public ParticleSystem fullCurseEffect;
    public ParticleSystem digEffect;

    //TEST
    public GameObject otherplayer;
    public Lantern Lantern { get; set; }
    //TEST

    //#####################################################################

    void Awake() {
        IsSafe = false;
        CurseStatus = Status.NORMAL;
        Visible = Visibility.INVISIBLE;
        IsZoneDigging = false;      
    }

    // Use this for initialization
    void Start() {

        digCount = 100;
        _rb = GetComponent<Rigidbody>();
        VDig = GetComponentInChildren<VerticalDig>(includeInactive:true);
        ZDig = GetComponentInChildren<ZoneDig>(includeInactive: true);
        MainCamera = FindObjectOfType<CameraManager>();

        skill = GetComponentInChildren<Skill>();

        var particles =GetComponentsInChildren<CFX_AutoDestructShuriken>(); 
        foreach(var part in particles) {

            if (part.CompareTag("Half_Curse"))
                halfCurseEffect = part.GetComponent<ParticleSystem>();
            else if(part.CompareTag("Full_Curse"))
                fullCurseEffect = part.GetComponent<ParticleSystem>();
            else if(part.CompareTag("Dig_Effect"))
                digEffect = part.GetComponent<ParticleSystem>();
            part.gameObject.SetActive(false);
        }
        
       
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
        return !(IsZoneDigging || IsCasting || runningAnimation);
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
            _missingParts++;
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

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Lamp_Base")) {//if the character has entered the light of a lamp that is switched on
            IsSafe = true;
        }
        //if the character touches an enemy trigger
        //READ AS: if an enemy curse the character
        else if (other.CompareTag("EnemyBody") && !IsSafe) {

            Enemy touchedEnemy = other.GetComponentInParent<Enemy>();
            touchedEnemy.PlayerTouched(this);
            Debug.Log("before " + CurseStatus);
            if (CurseStatus == Status.NORMAL && !touchedEnemy.data_enemy.instant_curse) {
                CurseStatus = Status.HALF_CURSED;
                if (halfCurseEffect) {
                    halfCurseEffect.gameObject.SetActive(true);
                    halfCurseEffect.Play();
                }

                TeamHUD.Instance.HalfCurse();
                Debug.Log("after " + CurseStatus);
                return;
            }

            if (fullCurseEffect) {
                fullCurseEffect.gameObject.SetActive(true);
                fullCurseEffect.Play();
            }
           
            GameManager.Instance.SpawnNewEnemy(touchedEnemy.data_enemy.level - 1, _rb.position, touchedEnemy.path);
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
            if (lamp.isEnemyLamp  ) { //if it is an enemy lamp AND it is turned on
                if(lamp.isTurnedOn)
                    lamp.SwitchOffEnemyLamp();
                return;
            }
            
            if (lamp.hasMissingPart && _missingParts > 0) {
                lamp.hasMissingPart = false;
                _missingParts--;
            }
           

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
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetButtonDown("PS4_Button_Triangle")) // [VDIG]
                if (digCount > 0)
                {
                    VDig.CheckInput();
                }

            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetButtonDown("PS4_Button_Square")) // [ZDIG]
                if (digCount > 0)
                {
                    ZDig.CheckInput();
                    AnimationManager.Anim_StarDigging(transform);
                }
        }
    }
}


