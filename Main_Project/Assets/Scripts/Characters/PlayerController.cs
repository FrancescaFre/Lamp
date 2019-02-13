using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

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
    [Header("Charackter Information")]
    public CharPeriod CharacterPeriod;
    public Skill skill;

    public int digCount = 100;
    
    public bool usingSkill = false;
    public bool isSneaking = false;
    public bool isRunning = false;


    public Status CurseStatus;
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
    private Transform _modelTransform;
    [Header("Item informations")]
    public int _missingParts = 0;
    public int keys = 0;

    [Header("Character's FX")]
    public ParticleSystem halfCurseEffect;
    public ParticleSystem fullCurseEffect;
    public ParticleSystem digEffect;

    [Header("In-HUD References")]
    public GameObject questionMarkPrefab;  
    public Image questionMark;

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

        _rb = GetComponent<Rigidbody>();
        _modelTransform = GetComponent<DifferenceOfTerrain>().modelTransform;

        VDig = GetComponentInChildren<VerticalDig>(includeInactive:true);
        ZDig = GetComponentInChildren<ZoneDig>(includeInactive: true);
        MainCamera = FindObjectOfType<CameraManager>();

        skill = GetComponentInChildren<Skill>();

        var particles =GetComponentsInChildren<CFX_AutoDestructShuriken>(); 
        foreach(var part in particles) {

            if (part.CompareTag(Tags.Half_Curse))
                halfCurseEffect = part.GetComponent<ParticleSystem>();
            else if(part.CompareTag(Tags.Full_Curse))
                fullCurseEffect = part.GetComponent<ParticleSystem>();
            else if(part.CompareTag(Tags.Dig_Effect))
                digEffect = part.GetComponent<ParticleSystem>();
            part.gameObject.SetActive(false);
        }


        questionMark = Instantiate(questionMarkPrefab, InGameHUD.Instance.InGameHUDPanel.transform).GetComponent<Image>();
        questionMark.gameObject.SetActive(false);
    }
    
    // Update is called once per frame
    void Update() {
        this.CheckSkillInteraction();
        this.CheckItemInteraction();
        this.CheckDig();
    }

    private void LateUpdate() {
        questionMark.transform.position = MainCamera.GetComponent<Camera>().WorldToScreenPoint(_modelTransform.position);
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
 
        if (Input.GetButtonDown(Controllers.PS4_Button_O) || Input.GetKeyDown(KeyCode.Q)) {
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
        if (other.CompareTag(Tags.MissingPart))
        {
            _missingParts++;
            other.gameObject.SetActive(false);
            AudioManager.Instance.SFXSource.PlayOneShot(GameManager.Instance.levelLoaded.missingSFX);
        }
        else if (other.CompareTag(Tags.Key))
        {
            keys++;
            AudioManager.Instance.SFXSource.PlayOneShot(GameManager.Instance.levelLoaded.keySFX);
            other.gameObject.SetActive(false);
            //------ WARNING: todo NON LO SO
        }
        else if (other.CompareTag(Tags.Drill)) {
            digCount++;
            other.gameObject.SetActive(false);
            AudioManager.Instance.SFXSource.PlayOneShot(GameManager.Instance.levelLoaded.drillSFX);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.Lamp_Base)) {//if the character has entered the light of a lamp that is switched on
            IsSafe = true;
        }
        //if the character touches an enemy trigger
        //READ AS: if an enemy curse the character
        else if (other.CompareTag(Tags.EnemyBody) && !IsSafe) {
            
            Enemy touchedEnemy = other.GetComponentInParent<Enemy>();

            



            if (CurseStatus == Status.NORMAL && !touchedEnemy.data_enemy.instant_curse) {
                CurseStatus = Status.HALF_CURSED;
                if (halfCurseEffect) {
                    halfCurseEffect.gameObject.SetActive(true);
                    halfCurseEffect.Play();
                }
                touchedEnemy.PlayerTouched();
                TeamHUD.Instance.HalfCurse();
                _rb.MovePosition(transform.position + Vector3.one);

                ChangeVignetteSmoothness(GameManager.Instance.curseVignetteSmoothness);
                
                return;
            }

            CurseStatus = Status.CURSED;
            touchedEnemy.PlayerTouched();
            if (fullCurseEffect) {
                fullCurseEffect.gameObject.SetActive(true);
                fullCurseEffect.Play();
            }

            ChangeVignetteSmoothness(GameManager.Instance.normalVignetteSmoothness);
            GameManager.Instance.SpawnNewEnemy(touchedEnemy.data_enemy.level - 1, _rb.position, touchedEnemy.path);
            
        }
 
    }
    private void ChangeVignetteSmoothness(float value) {
        VignetteModel.Settings cameraVignette = MainCamera.GetComponent<PostProcessingBehaviour>().profile.vignette.settings;
        cameraVignette.smoothness = value;
        MainCamera.GetComponent<PostProcessingBehaviour>().profile.vignette.settings = cameraVignette;

    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(Tags.Lamp_Base) ) {//if the character has entered the light of a lamp that is switched on
            IsSafe = false;
        }
    }


    private void OnCollisionStay(Collision collision) {
        Collider other = collision.collider;
        if (other.CompareTag(Tags.Lamp_Switch)) {

            LampBehaviour lamp = other.GetComponentInParent<LampBehaviour>();

            if (lamp.isEnemyLamp) { //if it is an enemy lamp AND it is turned on
                if (lamp.isTurnedOn)
                    lamp.SwitchOffEnemyLamp();
                return;
            }

            if (lamp.hasMissingPart && _missingParts > 0) {
                lamp.hasMissingPart = false;
                lamp.canBeSwitchedOn = true;
                _missingParts--;
            }
            else if (lamp.hasMissingPart && _missingParts <= 0) {
                questionMark.gameObject.SetActive(true);
               
            }


            if (IsMimicOrDash) return;
            if (lamp.isTurnedOn) {
                questionMark.gameObject.SetActive(false);
                return;
            }    //if the lamp is already turned on, exit
            lamp.SwitchOnAllyLamp();


        }
    }

    private void OnCollisionExit(Collision collision) {
        Collider other = collision.collider;
        if (other.CompareTag(Tags.Lamp_Switch)) {
            LampBehaviour lamp = other.GetComponentInParent<LampBehaviour>();
            if(!lamp.isTurnedOn && lamp.hasMissingPart) {
                
                questionMark.gameObject.SetActive(false);
            }
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
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetButtonDown(Controllers.PS4_Button_Triangle)) // [VDIG]
                if (digCount > 0)
                {
                    VDig.CheckInput();
                }

            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetButtonDown(Controllers.PS4_Button_Square)) // [ZDIG]
                if (digCount > 0)
                {
                    ZDig.CheckInput();
                    AnimationManager.Anim_StarDigging(transform);
                }
        }
    }
}


