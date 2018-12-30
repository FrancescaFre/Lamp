using System.Collections.Generic;
using UnityEngine;
using AuraAPI;

public class LampBehaviour : MonoBehaviour {
    [Header("Light Sources")]
    public Light[] lightBulb;
    public AuraLight[] auraLight;
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
    public int radius;
   

    void Awake() {

        canBeSwitchedOn = !hasMissingPart;
        isTurnedOn = isEnemyLamp;
        if (isEnemyLamp)
            lampsInDomain = new List<LampBehaviour>();
    }

    // Use this for initialization
    void Start() {
        lightBulb = GetComponentsInChildren<Light>();                 //the light sources of the child GO
        auraLight = GetComponentsInChildren<AuraLight>();                 //the aura sources of the child GO
        particleSystems = GetComponentsInChildren<ParticleSystem>();

        allColliders = GetComponentsInChildren<Collider>();
        
        
        for (int i = 0; i < particleSystems.Length; i++) {
            if (particleSystems[i].rotationOverLifetime.enabled) {

                ParticleSystem.MainModule main = particleSystems[i].main;

                if (isEnemyLamp)
                    main.startColor = GameManager.Instance.levelLoaded.enemyColor;
                else
                    main.startColor = GameManager.Instance.levelLoaded.allyColor;
            }
            else if (particleSystems[i].noise.enabled && !isEnemyLamp)
                goodSpirits = particleSystems[i];
        }

        for (int i = 0; i < lightBulb.Length; i++) {
            if (isEnemyLamp) {
                lightBulb[i].color = GameManager.Instance.levelLoaded.enemyColor;
                lightBulb[i].range = 1f;
            }
            else
                lightBulb[i].color = GameManager.Instance.levelLoaded.allyColor;

            lightBulb[i].gameObject.SetActive(isTurnedOn);
            auraLight[i].enabled = true;
        }



        for (int i = 0; i < allColliders.Length; i++) {
            if (allColliders[i].CompareTag("Lamp_Switch")) {
                allColliders[i].enabled = true;
                allColliders[i].isTrigger = false;
            }
            if (allColliders[i].CompareTag("Lamp_Base")) {
                allColliders[i].enabled = false;
                allColliders[i].isTrigger = true;
            }
        }
        
        
// ------------- Evolution of enemy lamp
        if (isEnemyLamp)
        {
            nearByLampColliders = Physics.OverlapSphere(this.transform.position, radius);

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
            auraLight[i].enabled = true;
        }

        for (int i = 0; i < allColliders.Length; i++) {

            if (allColliders[i].CompareTag("Lamp_Base"))
                allColliders[i].enabled = true;
        }

        AudioManager.Instance.SFXSource.PlayOneShot(GameManager.Instance.levelLoaded.lampSwitchSFX);
        var temp = goodSpirits.main;
        temp.loop = false;
        
        isTurnedOn = true;
        Debug.Log("lamp_switch: ON ");
        gameObject.layer = 11; //obstacle layer
        GameManager.Instance.allyLamps++;
        GameManager.Instance.lampHUD.DequeueAlly();
        if (GameManager.Instance.allyLamps == GameManager.Instance.levelLoaded.allyLamps)
            GameManager.Instance.GoodEndGame();
        GameManager.Instance.LastAllyLamp = this;   //if the character dies, the next one will be spawned here

  
    }

    public void SwitchOffEnemyLamp() {
        isTurnedOn = false;
        for (int i = 0; i < lightBulb.Length; i++) {
            lightBulb[i].gameObject.SetActive(false);
            auraLight[i].enabled = false;
            Debug.Log("off: " + lightBulb[i].gameObject.name);
        }

        GameManager.Instance.enemyLamps++;
        GameManager.Instance.lampHUD.DequeueEnemy();
        AudioManager.Instance.SFXSource.PlayOneShot(GameManager.Instance.levelLoaded.lampSwitchSFX);

        //----------- Evolution of enemy lamp
        foreach (var lamp in lampsInDomain) {
                lamp.IsSwitchable(true);
        }

    }
    public void IsSwitchable(bool condition) {
        canBeSwitchedOn = condition;
        if (!goodSpirits) {
            if (particleSystems.Length == 0)
                particleSystems = GetComponentsInChildren<ParticleSystem>();
            foreach (var part in particleSystems) {
                if (part.noise.enabled)
                    goodSpirits = part;
            }
        }

        if (canBeSwitchedOn) {
            //TODO: switch on FX
            goodSpirits.gameObject.SetActive(true);
            goodSpirits.Play();
        }
        else {
            //TODO: switch off FX

            goodSpirits.Stop();
            goodSpirits.gameObject.SetActive(false);
        }

    }
}
