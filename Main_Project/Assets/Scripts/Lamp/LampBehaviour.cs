using System.Collections.Generic;
using UnityEngine;


public class LampBehaviour : MonoBehaviour {
    [Header("Light Sources")]
    public Light[] lightBulb;

    [Header("All Paricles")]
    public ParticleSystem[] particleSystems;
    [Header("All the Colliders")]
    public Collider[] allColliders;
    [Header("Lamp Properties")]
    public bool isEnemyLamp = false;
    [Tooltip("DO NOT MODIFY! \nDepends on wether the lamp is an enemy or not")]
    public bool isTurnedOn = false;


    /// <summary>
    /// True if the lamp is missing a part.
    /// </summary>
    public bool hasMissingPart = false;

    //----- evolution of enemylamp
    private Collider[] nearByLampColliders;
    public List<LampBehaviour> lampsInDomain = null;
    [Tooltip("DO NOT MODIFY! \nDepends on wether the lamp has missing parts or not")]
    public bool canBeSwitchedOn = false;
    public ParticleSystem goodSpirits;
    public ParticleSystem badSpirits;
    public int radiusDomain;

    private SFXEmitter _emitter;

    void Awake() {
        
        canBeSwitchedOn = !hasMissingPart;
        isTurnedOn = isEnemyLamp;
        if (isEnemyLamp)
            lampsInDomain = new List<LampBehaviour>();
    }

    // Use this for initialization
    void Start() {
        _emitter = GetComponent<SFXEmitter>();

        lightBulb = GetComponentsInChildren<Light>();                 //the light sources of the child GO
        
        particleSystems = GetComponentsInChildren<ParticleSystem>();

        allColliders = GetComponentsInChildren<Collider>();
        
        
        for (int i = 0; i < particleSystems.Length; i++) {


                ParticleSystem.MainModule main = particleSystems[i].main;

                if (isEnemyLamp)
                    main.startColor = GameManager.Instance.levelLoaded.enemyColor;
                else
                    main.startColor = GameManager.Instance.levelLoaded.allyColor;
           
            if (particleSystems[i].CompareTag(Tags.GoodSpirits) && !isEnemyLamp)
                goodSpirits = particleSystems[i];
            else if (particleSystems[i].CompareTag(Tags.BadSpirits) && isEnemyLamp)
                badSpirits = particleSystems[i];
        }

        for (int i = 0; i < lightBulb.Length; i++) {
            if (isEnemyLamp) {
                lightBulb[i].color = GameManager.Instance.levelLoaded.enemyColor;
                lightBulb[i].range = 1f;
            }
            else
                lightBulb[i].color = GameManager.Instance.levelLoaded.allyColor;

            lightBulb[i].gameObject.SetActive(isTurnedOn);
            
        }



        for (int i = 0; i < allColliders.Length; i++) {
            if (allColliders[i].CompareTag(Tags.Lamp_Switch)) {
                allColliders[i].enabled = true;
                allColliders[i].isTrigger = false;
            }
            if (allColliders[i].CompareTag(Tags.Lamp_Base)) {
                allColliders[i].enabled = false;
                allColliders[i].isTrigger = true;
            }
        }

        
        _emitter.source.Stop();
        // ------------- Evolution of enemy lamp
        if (isEnemyLamp)
        {
            badSpirits.Stop(withChildren: true);
            _emitter.source.Play();
            nearByLampColliders = Physics.OverlapSphere(this.transform.position, radiusDomain);

            foreach (Collider l in nearByLampColliders) {
                LampBehaviour lamp = l.GetComponentInParent<LampBehaviour>();
                if (lamp && !lamp.isEnemyLamp) {
                    lampsInDomain.Add(lamp);
                    lamp.IsSwitchable(false);
                    
                }
            }
        }else
            IsSwitchable(canBeSwitchedOn);


    }


    public void SwitchOnAllyLamp() { //you cant turn on a lamp if there are any enemy lamp turned on
       // if (hasMissingPart || GameManager.Instance.enemyLamps > 0 ) return;
        if (hasMissingPart || !canBeSwitchedOn) return;

        for (int i = 0; i < lightBulb.Length; i++) {
            lightBulb[i].gameObject.SetActive(true);
           
        }

        for (int i = 0; i < allColliders.Length; i++) {

            if (allColliders[i].CompareTag(Tags.Lamp_Base))
                allColliders[i].enabled = true;
        }

        AudioManager.Instance.SFXSource.PlayOneShot(GameManager.Instance.levelLoaded.lampSwitchSFX);
        var temp = goodSpirits.main;
        temp.loop = false;
        
        
        isTurnedOn = true;

        gameObject.layer = 11; //obstacle layer
        GameManager.Instance.allyLamps++;
        GameManager.Instance.worldLight.Enlight();

       
        if (GameManager.Instance.allyLamps == GameManager.Instance.levelLoaded.allyLamps)
            GameManager.Instance.GoodEndGame();
        GameManager.Instance.LastAllyLamp = this;   //if the character dies, the next one will be spawned here

        _emitter.source.Play();

    }

    public void SwitchOffEnemyLamp() {
        isTurnedOn = false;
        for (int i = 0; i < lightBulb.Length; i++) {
            lightBulb[i].gameObject.SetActive(false);
          
            Debug.Log("off: " + lightBulb[i].gameObject.name);
        }

        badSpirits.Play(withChildren:true);

        GameManager.Instance.enemyLamps++;
       
        AudioManager.Instance.SFXSource.PlayOneShot(GameManager.Instance.levelLoaded.lampSwitchSFX);
        _emitter.source.Stop();
        //----------- Evolution of enemy lamp
        foreach (var lamp in lampsInDomain) {
                lamp.IsSwitchable(true);
        }

    }
    public void IsSwitchable(bool condition) {
        canBeSwitchedOn = condition;
        if (!goodSpirits ) {
            if (particleSystems.Length == 0)
                particleSystems = GetComponentsInChildren<ParticleSystem>();
            foreach (var part in particleSystems) {
                if (part.CompareTag(Tags.GoodSpirits))
                    goodSpirits = part;
                if (part.CompareTag(Tags.BadSpirits))
                    badSpirits = part;
            }
        }

        if (canBeSwitchedOn ) {
            //TODO: switch on particleFX
            goodSpirits.gameObject.SetActive(true);
            goodSpirits.Play();
        }
        else {
            //TODO: switch off particleFX

            goodSpirits.Stop();
            goodSpirits.gameObject.SetActive(false);
        }

    }
}
