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

    private AudioSource _source;
    /// <summary>
    /// True if the lamp is missing a part.
    /// </summary>
    public bool hasMissingPart = false;

    //----- evolution of enemylamp
    private Collider[] nearByLampColliders;
    [Tooltip("DO NOT MODIFY! \nDepends on wether the lamp has missing parts or not")]
    public bool canBeSwtichedOn = false;
    public ParticleSystem goodSpirits;
    public int radius;
   

    private void Awake() {
        _source=gameObject.AddComponent<AudioSource>();
        _source.playOnAwake=false;
        _source.volume = .3f;
    }

    // Use this for initialization
    void Start() {
        lightBulb = GetComponentsInChildren<Light>();                 //the light sources of the child GO
        auraLight = GetComponentsInChildren<AuraLight>();                 //the aura sources of the child GO
        particleSystems = GetComponentsInChildren<ParticleSystem>();

        allColliders = GetComponentsInChildren<Collider>();
        isTurnedOn = isEnemyLamp;
        canBeSwtichedOn = !hasMissingPart;
        for (int i = 0; i < particleSystems.Length; i++) {
            if (particleSystems[i].rotationOverLifetime.enabled) {

                ParticleSystem.MainModule main = particleSystems[i].main;

                if (isEnemyLamp)
                    main.startColor = GameManager.Instance.levelLoaded.enemyColor;
                else
                    main.startColor = GameManager.Instance.levelLoaded.allyColor;
            }
            else if (particleSystems[i].noise.enabled)
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
            nearByLampColliders = Physics.OverlapSphere(this.transform.position, radius, LayerMask.GetMask("Lamp_Base"));

            foreach (Collider lamp in nearByLampColliders)
                if (!lamp.GetComponent<LampBehaviour>().isEnemyLamp)
                    lamp.GetComponent<LampBehaviour>().IsSwitchable (false); 
        }

        IsSwitchable(canBeSwtichedOn);
    }


    public void SwitchOnAllyLamp() { //you cant turn on a lamp if there are any enemy lamp turned on
       // if (hasMissingPart || GameManager.Instance.enemyLamps > 0 ) return;
        if (hasMissingPart || !canBeSwtichedOn) return;

        for (int i = 0; i < lightBulb.Length; i++) {
            lightBulb[i].gameObject.SetActive(true);
            auraLight[i].enabled = true;
        }

        for (int i = 0; i < allColliders.Length; i++) {

            if (allColliders[i].CompareTag("Lamp_Base"))
                allColliders[i].enabled = true;
        }

        _source.PlayOneShot(GameManager.Instance.levelLoaded.switchSFX);
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
        _source.PlayOneShot(GameManager.Instance.levelLoaded.switchSFX);

//----------- Evolution of enemy lamp
        nearByLampColliders = Physics.OverlapSphere(this.transform.position, radius, LayerMask.GetMask("Lamp_Base"));
        foreach (Collider lamp in nearByLampColliders)
            lamp.GetComponent<LampBehaviour>().IsSwitchable (true);

    }

    public void IsSwitchable(bool condition) {
        canBeSwtichedOn = condition;
        if (canBeSwtichedOn) {
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
