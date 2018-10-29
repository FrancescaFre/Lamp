using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampBehaviour : MonoBehaviour {

    public Light lightBulb;
    public SphereCollider baseCollider;
    public CapsuleCollider lampCollider;

	// Use this for initialization
	void Start () {
        lightBulb=GetComponentInChildren<Light>();                  //the light source of the child GO
        baseCollider = GetComponentInChildren<SphereCollider>();    //the collider of the base around the lamp (is a trigger)
        lampCollider = GetComponent<CapsuleCollider>();             //the collider around the lamp model (is a trigger)

        lightBulb.enabled = false;
        baseCollider.enabled = false;
        lampCollider.enabled = true;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchOnLamp() {
        lightBulb.enabled = true;
        baseCollider.enabled = true;
        lampCollider.enabled = false;

    }

}
