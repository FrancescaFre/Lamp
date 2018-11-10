using UnityEngine;
using AuraAPI;

public class LampBehaviour : MonoBehaviour {

    public Light[] lightBulb;
    public SphereCollider baseCollider;
    public CapsuleCollider lampCollider;
    public bool IsEnemyLamp;
   
    /// <summary>
    /// True if the lamp is missing a part.
    /// </summary>
    public bool IsMissingPart { get; private set; }   

	// Use this for initialization
	void Start () {
        lightBulb=GetComponentsInChildren<Light>();                 //the light sources of the child GO
        baseCollider = GetComponentInChildren<SphereCollider>();    //the collider of the base around the lamp (is a trigger)
        lampCollider = GetComponent<CapsuleCollider>();             //the collider around the lamp model (is a trigger)

        
        for (int i =0;i<lightBulb.Length; i++)
            lightBulb[i].enabled = false;
        baseCollider.enabled = false;
        lampCollider.enabled = true;
        IsMissingPart = false;

         

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SwitchOnAllyLamp() {
        if (IsMissingPart) return;

        for (int i = 0; i < lightBulb.Length; i++) {
            lightBulb[i].enabled = true;
            lightBulb[i].GetComponent<AuraLight>().enabled=true;
        }
        baseCollider.enabled = true;
        lampCollider.enabled = false;

        GameManager.Instance.LastAllyLamp = this;   //if the character dies, the next one will be spawned here
    }

    public void SwitchOffEnemyLamp() {
        lampCollider.enabled = false;
        for (int i = 0; i < lightBulb.Length; i++)
            lightBulb[i].enabled = false;
    }


}
