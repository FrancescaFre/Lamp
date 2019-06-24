using UnityEngine;

public class DigBehaviour : MonoBehaviour {
    public static DigBehaviour instance;

    [Header("Particle Ring")]
    public ParticleSystem digRing;
    private ParticleSystemRenderer pRenderer;
    public Material dig;
    public Material no_dig;

    
    [Header("Dig Information")]
    public bool canDig = true;
    public  bool isZoneActive;
    public  bool isVerticalActive;
    [Range(1f,10f)]
    public float distanceRadius = 8f;
    private PlayerController pc;
    private PlayerMovement _pm;
    private Vector3 _movement;
    private Rigidbody rb;
    private float _speed;
    private float _horizInput, _vertInput;
    
    //caster
    private float castingTime = 0f; 
    private float _progress; // Actual casting progress
    [Space]
    public DiggableWallLighter wallLighter;

    private void Awake() {
        if (!instance)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        isZoneActive = false;
        isVerticalActive = false;
    }

    private void Start() {
        _movement = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        _horizInput = 0f;
        _vertInput = 0f;

        digRing = GetComponentInChildren<ParticleSystem>();

        pRenderer = digRing.GetComponent<ParticleSystemRenderer>();
        pRenderer.sharedMaterial = dig;
        digRing.Stop();
        
        pc = GameManager.Instance.currentPC;
        rb.MovePosition(Vector3.zero);
    }

    protected void Update() {
        _horizInput = Input.GetAxis(Controllers.Horizontal);
        _vertInput = Input.GetAxis(Controllers.Vertical);
        CheckInput();
    }
 
    private void FixedUpdate() {
        if (!pc) return;
        if (!pc.caster) return;

        pc.caster.transform.position = BasicCamera.instance.GetComponent<Camera>().WorldToScreenPoint(transform.position);

        if (pc.caster.isActiveAndEnabled)
            CastDig();

        
        if(isVerticalActive)//the circle stays under the player
            rb.MovePosition(-pc.transform.position);

        if (!pc.isCasting && isZoneActive) {//the circle moves around the player

            Vector3 forward, right;
            forward = Vector3.ProjectOnPlane((transform.position - BasicCamera.instance.transform.position), transform.up).normalized;
            right = Vector3.Cross(transform.up, forward).normalized;

            _movement = (_horizInput * right + _vertInput * forward).normalized * Time.deltaTime * _speed;

            if (Vector3.Distance(rb.position + _movement, pc.transform.position) < distanceRadius && isZoneActive) {
                rb.MovePosition(rb.position + _movement);
            }
        }
    }

    private void LateUpdate() {
        if (isZoneActive || isVerticalActive) {
            
            if (CanZoneDig() ||CanVerticalDig()) {
                if (!pRenderer.sharedMaterial.Equals(dig))
                    pRenderer.sharedMaterial = dig;
            }
            else {
                if (!pRenderer.sharedMaterial.Equals(no_dig))
                    pRenderer.sharedMaterial = no_dig;
            }
        }


    }

    private void CheckInput() {
        if (!pc) return;
        if (GameManager.Instance.digCount <= 0) return;
        if (pc.isCasting || InGameHUD.Instance.pauseManager.IsPaused) return;
        
        //vertical
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetButtonDown(Controllers.PS4_Button_Triangle)) { // [VDIG]
            if (isZoneActive) {
                ReleaseZoneDigger();
                return;
            }
            if (!isVerticalActive) {
                ActivateVerticalDigger();
                return;
            }
            VerticalDig();
        }

        //zone
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown(Controllers.PS4_Button_Square)) { // [ZDIG]
            if (isVerticalActive) {
                ReleaseVerticalDigger();
                return;
            }
               
            if (!isZoneActive) {
                ActivateZoneDigger();
                return;
            }
                
            ZoneDig();
        }   
    }

    #region ZONE DIG HANDLER

    public void ActivateZoneDigger() {
        pc = GameManager.Instance.currentPC;
        _pm = pc.GetComponent<PlayerMovement>();
        _speed = _pm.walkSpeed;

      
        wallLighter.EnlightWalls();

        BasicCamera.instance.ChangeTarget(transform);

        transform.position = pc.transform.position;

        
        if (Physics.Raycast(pc.transform.position, pc.transform.forward, 1f, ~(1 << 9))) // If it hits something in front of the player (except the player, that's layer 9)
            transform.position -= Vector3.forward; // Spawns the circle in rear of the player
        else
            transform.localPosition += Vector3.forward; // Spawns the circle in front of the player

        pRenderer = digRing.GetComponent<ParticleSystemRenderer>();
        digRing.Play();
        isZoneActive = true;
    }

    public void ReleaseZoneDigger() {

        BasicCamera.instance.ChangeTarget(GameManager.Instance.currentPC.transform);

        wallLighter.TurnOffWallLights();
  
        transform.position = Vector3.zero;
        
        //[CHECK]
        transform.rotation = Quaternion.Euler(Vector3.zero);
        digRing.Stop();
        isZoneActive = false;
    }

    private void ZoneDig() {
        
        if (CanZoneDig()) {
            pc.drillGO.SetActive(true);
            AnimationManager.Anim_StarDigging(pc.characterAnimator);
            if (pc.CharacterPeriod == CharPeriod.ORIENTAL)

                Invoke("HideDrillGO", AnimationManager.Anim_LenghtAnim(pc.characterAnimator, "orientalDIG"));
            else
                Invoke("HideDrillGO", AnimationManager.Anim_LenghtAnim(pc.characterAnimator, "Dig And Plant Seeds"));
            StartCasting();
            pc.isCasting = true;
        }
        else {
            
            ReleaseZoneDigger();

        }
        
    }
  
    #endregion

    #region VERTICAL DIG HANDLER
    public void ActivateVerticalDigger() {
        pc = GameManager.Instance.currentPC;
        _pm = pc.GetComponent<PlayerMovement>();


        rb.MovePosition(-pc.transform.position);
        pc.digRing.gameObject.SetActive(true);
        pRenderer = pc.digRing.GetComponent<ParticleSystemRenderer>();
        pc.digRing.Play();

        isVerticalActive = true;
    }

    public void ReleaseVerticalDigger() {
        pc.isCasting = false;

        transform.position = Vector3.zero;

        //[CHECK]
        transform.rotation = Quaternion.Euler(Vector3.zero);
        pc.digRing.Stop();
        isVerticalActive = false;
    }

    private void VerticalDig() {

        if (CanVerticalDig()) {
            pc.drillGO.SetActive(true);
            AnimationManager.Anim_StarDigging(pc.characterAnimator);
            if (pc.CharacterPeriod == CharPeriod.ORIENTAL)

                Invoke("HideDrillGO", AnimationManager.Anim_LenghtAnim(pc.characterAnimator, "orientalDIG"));
            else
                Invoke("HideDrillGO", AnimationManager.Anim_LenghtAnim(pc.characterAnimator, "Dig And Plant Seeds"));
            StartCasting();
            pc.isCasting = true;
        }
        else 
            ReleaseVerticalDigger();
    }

    #endregion
  protected void HideDrillGO() {
        pc.drillGO.SetActive(false);
    }
    /// <summary>
    /// Pops up and starts the casting bar
    /// </summary>
    protected void StartCasting() {
        _progress = 0;
        pc.caster.gameObject.SetActive(true);
    }

    /// <summary>
    /// Fills the caster while performing the animation
    /// </summary>
    protected void CastDig() {
        if (castingTime <= 0)
            SetCastingTime();
        _progress += Time.deltaTime;
        pc.caster.value = _progress;

        if (_progress >= castingTime) {
            Dig();
            StopCasting();
           
        }
    }
    private void SetCastingTime() {
        if (pc.CharacterPeriod == CharPeriod.VICTORIAN || pc.CharacterPeriod == CharPeriod.PREHISTORY)
            castingTime = AnimationManager.Anim_LenghtAnim(pc.characterAnimator, "Dig And Plant Seeds");
        if (pc.CharacterPeriod == CharPeriod.ORIENTAL)
            castingTime = AnimationManager.Anim_LenghtAnim(pc.characterAnimator, "orientalDIG");

        pc.caster.maxValue = castingTime;
    }

    public bool CanZoneDig() {
        
        return canDig;

    }

    public bool CanVerticalDig() {
      
        return !_pm.OnWater &&  !_pm.OnIce && !_pm.OnSolidFloor && canDig;

    }
    public void Dig() {
        if (pc.digEffect) {
            pc.digEffect.gameObject.SetActive(true);
            pc.digEffect.Play();
            pc.GetComponent<PlayerSFXEmitter>().DigEffect();//plays dig audio
        }

        if (isZoneActive) { 
            pc.GetComponent<Rigidbody>().MovePosition(transform.position);
            ReleaseZoneDigger();
            wallLighter.TurnOffWallLights();
        }

        if (isVerticalActive) {
            pc.GetComponent<Rigidbody>().MovePosition(-(pc.transform.position)); // Moves the player on the other side of the world
            ReleaseVerticalDigger();
        }
        canDig = false;
        pc.isCasting = false;
        GameManager.Instance.digCount--;

       
    }

    /// <summary>
    /// Hides and resets the casting bar
    /// </summary>
    protected void StopCasting() {
        _progress = 0;
        pc.caster.value = 0;
        pc.caster.gameObject.SetActive(false);
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, .1f);
    }



    #region Trigger Handling
    protected void OnTriggerEnter(Collider terrain) {
        
        if (terrain.gameObject.CompareTag(Tags.Solid) ||
            terrain.gameObject.CompareTag(Tags.Water) ||
            terrain.gameObject.CompareTag(Tags.Ice) ) {
            Debug.Log("enter in "+terrain.name);
            canDig = false;
        }
    }

    protected void OnTriggerStay(Collider terrain) {
        if (terrain.gameObject.CompareTag(Tags.Solid) ||
            terrain.gameObject.CompareTag(Tags.Water) ||
            terrain.gameObject.CompareTag(Tags.Ice) ) {

            canDig = false;

        }
    }

    protected void OnTriggerExit(Collider terrain) {
        if (terrain.gameObject.CompareTag(Tags.Solid) ||
            terrain.gameObject.CompareTag(Tags.Water) ||
            terrain.gameObject.CompareTag(Tags.Ice) ) {

            canDig = true;
           
        }
    }


    #endregion
}