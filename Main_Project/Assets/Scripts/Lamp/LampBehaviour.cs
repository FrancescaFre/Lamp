using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampBehaviour : MonoBehaviour {

    public Light lightBulb;
    public SphereCollider baseCollider;
    public CapsuleCollider lampCollider;
    public bool IsEnemyLamp;
   
    /// <summary>
    /// True if the lamp is missing a part.
    /// </summary>
    public bool IsMissingPart { get; private set; }   

	// Use this for initialization
	void Start () {
        lightBulb=GetComponentInChildren<Light>();                  //the light source of the child GO
        baseCollider = GetComponentInChildren<SphereCollider>();    //the collider of the base around the lamp (is a trigger)
        lampCollider = GetComponent<CapsuleCollider>();             //the collider around the lamp model (is a trigger)

        lightBulb.enabled = false;
        baseCollider.enabled = false;
        lampCollider.enabled = true;
        IsMissingPart = false;

         

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SwitchOnAllyLamp() {
        if (IsMissingPart) return;

        lightBulb.enabled = true;
        baseCollider.enabled = true;
        lampCollider.enabled = false;

        GameManager.Instance.LastAllyLamp = this;   //if the character dies, the next one will be spawned here
    }

    public void SwitchOffEnemyLamp() {
        lampCollider.enabled = false;
        lightBulb.enabled = false;
    }


}
