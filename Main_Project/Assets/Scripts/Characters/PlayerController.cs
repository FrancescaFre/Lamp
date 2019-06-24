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

    [Header("Character Information")]
    public CharPeriod CharacterPeriod;
    public Skill skill;
    public Character_SO profile;

    [Space]
    public bool usingSkill = false;
    public bool isSneaking = false;
    public bool isRunning = false;
    public bool isMoving = false;

    public Status CurseStatus;
    public Visibility Visible;

    //public VerticalDig VDig { get; set; }
   // public ZoneDig ZDig { get; set; }

    public bool IsMimicOrDash;
    public bool IsSafe;
    //public bool isZoneDigging;
    public bool isCasting;
    public bool runningAnimation = false;

    public Animator characterAnimator;

    private Rigidbody _rb;
    private Transform _modelTransform;

    [Header("Item informations")]
    public GameObject drillGO;

    [Header("Character's FX")]
    public ParticleSystem halfCurseEffect;
    public ParticleSystem fullCurseEffect;
    public ParticleSystem digEffect;
    public PlayerSFXEmitter emitter;

    [Header("In-HUD References")]
    public Image questionMark;
    public Slider caster;

    [Space]
    //TEST
    public GameObject otherplayer;
    public Lantern Lantern { get; set; }
    //TEST

    //#####################################################################

    private void Awake() {
        IsSafe = false;
        CurseStatus = Status.NORMAL;
        Visible = Visibility.INVISIBLE;
        
    }

    // Use this for initialization
    private void Start() {
        _rb = GetComponent<Rigidbody>();
        _modelTransform = GetComponent<DifferenceOfTerrain>().modelTransform;

      //  VDig = GetComponentInChildren<VerticalDig>(includeInactive: true);
       // ZDig = GetComponentInChildren<ZoneDig>(includeInactive: true);

        skill = GetComponentInChildren<Skill>();

        emitter = GetComponent<PlayerSFXEmitter>();

        var particles = GetComponentsInChildren<CFX_AutoDestructShuriken>();
        foreach (var part in particles) {
            if (part.CompareTag(Tags.Half_Curse))
                halfCurseEffect = part.GetComponent<ParticleSystem>();
            else if (part.CompareTag(Tags.Full_Curse))
                fullCurseEffect = part.GetComponent<ParticleSystem>();
            else if (part.CompareTag(Tags.Dig_Effect))
                digEffect = part.GetComponent<ParticleSystem>();
            part.gameObject.SetActive(false);
        }

        InGameHUD.Instance.CreateHUDReferences(ref questionMark, ref caster);
        characterAnimator = GetComponentInChildren<Animator>();

        var allChilds = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChilds) {
            if (child.CompareTag(Tags.Drill)) {
                drillGO = child.gameObject;
                break;
            }
        }

        drillGO.SetActive(false);
    }

    // Update is called once per frame
    private void Update() {
        this.CheckSkillInteraction();
        this.CheckItemInteraction();
        
    }

    private void LateUpdate() {
        questionMark.transform.position = BasicCamera.instance.GetComponent<Camera>().WorldToScreenPoint(_modelTransform.position);
        if (!DigBehaviour.instance.isZoneActive)
            caster.transform.position = BasicCamera.instance.GetComponent<Camera>().WorldToScreenPoint(_modelTransform.position);
    }

    //#####################################################################

    /// <summary>
    /// Returns true if the player can move freely
    /// </summary>
    public bool CanMove() {
        return !(DigBehaviour.instance.isZoneActive || isCasting || runningAnimation);
    }

    //#####################################################################

    #region Player's Interaction/action

    /// <summary>
    /// The skill is used if an input is detected
    /// </summary>
    private void CheckSkillInteraction() {
        if (Input.GetButtonDown(Controllers.PS4_Button_O) || Input.GetKeyDown(KeyCode.Q)) {
            if (usingSkill) {
                skill.DeactivateSkill();
                GameManager.Instance.subquest_skill++;
            }
            else {
                skill.ActivateSkill();
                usingSkill = true;
                Debug.Log("SKILL used");
            }
        }
    }

    private void CheckItemInteraction() {//TODO button to pick an item and to select an item to use
        if (IsMimicOrDash) return;
        // GameManager.Instance.subquest_item++;
    }

    #endregion Player's Interaction/action

    /// <summary>
    /// Sets the opposite of the current value of safety
    /// </summary>
    public void ChangeSafety() {
        IsSafe = !IsSafe;
    }

    #region Collision Detection

    //far apparire il pulsante per interagirci, per ora basta avvicinarsi per raccoglierlo
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag(Tags.MissingPart)) {
            GameManager.Instance.missingParts++;
            other.gameObject.SetActive(false);
            AudioManager.Instance.SFXSource.PlayOneShot(GameManager.Instance.levelLoaded.missingSFX);
        }
        else if (other.CompareTag(Tags.Key)) {
            GameManager.Instance.keys++;
            AudioManager.Instance.SFXSource.PlayOneShot(GameManager.Instance.levelLoaded.keySFX);
            other.gameObject.SetActive(false);
            //------ WARNING: todo NON LO SO
        }
        else if (other.CompareTag(Tags.Drill)) {
            GameManager.Instance.digCount++;
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

            emitter.HurtEffect();

            if (CurseStatus == Status.NORMAL && !touchedEnemy.data_enemy.instant_curse) {
                CurseStatus = Status.HALF_CURSED;
                if (halfCurseEffect) {
                    halfCurseEffect.gameObject.SetActive(true);
                    halfCurseEffect.Play();
                }
                touchedEnemy.PlayerTouched();
                TeamHUD.Instance.HalfCurse();
                _rb.MovePosition(_rb.position + Vector3.one);

                BasicCamera.instance.ChangeVignetteSmoothness(halfCurse: true);

                return;
            }

            CurseStatus = Status.CURSED;
            touchedEnemy.PlayerTouched();
            if (fullCurseEffect) {
                fullCurseEffect.gameObject.SetActive(true);
                fullCurseEffect.Play();
            }

            GameManager.Instance.subquest_curse++;

            BasicCamera.instance.ChangeVignetteSmoothness();
            GameManager.Instance.SpawnNewEnemy(touchedEnemy.data_enemy.level - 1, _rb.position, touchedEnemy.path);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(Tags.Lamp_Base)) {//if the character has entered the light of a lamp that is switched on
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

            if (lamp.hasMissingPart && GameManager.Instance.missingParts > 0) {
                lamp.hasMissingPart = false;
                lamp.canBeSwitchedOn = true;
                GameManager.Instance.missingParts--;
            }
            else if (lamp.hasMissingPart && GameManager.Instance.missingParts <= 0) {
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
            if (!lamp.isTurnedOn && lamp.hasMissingPart) {
                questionMark.gameObject.SetActive(false);
            }
        }
    }

    #endregion Collision Detection

    /// <summary>
    /// Stub to playtest digging. Press [I] for linear dig
    /// and [O] for zone dig
    /// </summary>

}