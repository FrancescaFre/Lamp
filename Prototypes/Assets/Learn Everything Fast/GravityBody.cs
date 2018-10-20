using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]  //this script requires a rigidbody to work
public class GravityBody : MonoBehaviour {
    
    public PlanetGravity AttractorPlanet;

	
	void Awake () {
       // AttractorPlanet = GameObject.FindObjectOfType<PlanetGravity>();   //in case the search happens at the beginning of the level
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().constraints=RigidbodyConstraints.FreezeRotation;  //the rotation is done manually
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (AttractorPlanet)
            this.AttractorPlanet.Attract(transform);
	}
}
