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
    public bool isTurnedOn = false;

    /// <summary>
    /// True if the lamp is missing a part.
    /// </summary>
    public bool hasMissingPart = false;

    // Use this for initialization
    void Start() {
        lightBulb = GetComponentsInChildren<Light>();                 //the light sources of the child GO
        auraLight = GetComponentsInChildren<AuraLight>();                 //the aura sources of the child GO
        particleSystems = GetComponentsInChildren<ParticleSystem>();

        allColliders = GetComponentsInChildren<Collider>();
        isTurnedOn = isEnemyLamp;

        for (int i = 0; i < particleSystems.Length; i++) {
            if (particleSystems[i].rotationOverLifetime.enabled) {

                ParticleSystem.MainModule main = particleSystems[i].main;

                if (isEnemyLamp)
                    main.startColor = GameManager.Instance.levelLoaded.enemyColor;
                else
                    main.startColor = GameManager.Instance.levelLoaded.allyColor;
            }

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



    }


    public void SwitchOnAllyLamp() { //you cant turn on a lamp if there are any enemy lamp turned on
        if (hasMissingPart || GameManager.Instance.enemyLamps > 0) return;

        for (int i = 0; i < lightBulb.Length; i++) {
            lightBulb[i].gameObject.SetActive(true);
            auraLight[i].enabled = true;
        }

        for (int i = 0; i < allColliders.Length; i++) {

            if (allColliders[i].CompareTag("Lamp_Base"))
                allColliders[i].enabled = true;
        }

        isTurnedOn = true;
        Debug.Log("lamp_switch: ON ");
        gameObject.layer = 11; //obstacle layer
        GameManager.Instance.allyLamps--;
        GameManager.Instance.lampHUD.DequeueAlly();
        if (GameManager.Instance.levelLoaded.allyLamps == 0) {

            //TODO crate a canvas as win condition
            Invoke("GameManager.Instance.EndGame", 10);
        }
        GameManager.Instance.LastAllyLamp = this;   //if the character dies, the next one will be spawned here
    }

    public void SwitchOffEnemyLamp() {
        isTurnedOn = false;
        for (int i = 0; i < lightBulb.Length; i++) {
            lightBulb[i].gameObject.SetActive(false);
            auraLight[i].enabled = false;
            Debug.Log("off: " + lightBulb[i].gameObject.name);

        }
        GameManager.Instance.enemyLamps--;
        GameManager.Instance.lampHUD.DequeueEnemy();

    }
}
