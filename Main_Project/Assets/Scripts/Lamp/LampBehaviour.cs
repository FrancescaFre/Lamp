using UnityEngine;
using AuraAPI;

public class LampBehaviour : MonoBehaviour {

    public Light[] lightBulb;
    public SphereCollider baseCollider;
    public CapsuleCollider lampCollider;
    public bool IsEnemyLamp = false;
   
    /// <summary>
    /// True if the lamp is missing a part.
    /// </summary>
    public bool IsMissingPart { get;  set; }   

	// Use this for initialization
	void Start () {
        lightBulb=GetComponentsInChildren<Light>();                 //the light sources of the child GO
        baseCollider = GetComponent<SphereCollider>();              //the collider of the base around the lamp (is a trigger)
        lampCollider = GetComponentInChildren<CapsuleCollider>();   //the collider around the lamp model 


        for (int i = 0; i < lightBulb.Length; i++) {
            lightBulb[i].gameObject.SetActive(false);
            
        }
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
            lightBulb[i].gameObject.SetActive(true);
            
        }
        baseCollider.enabled = true;
        lampCollider.enabled = false;

        GameManager.Instance.LastAllyLamp = this;   //if the character dies, the next one will be spawned here
    }

    public void SwitchOffEnemyLamp() {
        lampCollider.enabled = false;
        for (int i = 0; i < lightBulb.Length; i++) { 
            lightBulb[i].gameObject.SetActive(false);
            Debug.Log("off: " + lightBulb[i].gameObject.name);
        }
    }


}
