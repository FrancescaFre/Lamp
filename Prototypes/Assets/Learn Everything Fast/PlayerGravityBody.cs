using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityBody : MonoBehaviour {
    public PlanetGravity AttractorPlanet;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().constraints=RigidbodyConstraints.FreezeRotation;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (AttractorPlanet)
            this.AttractorPlanet.Attract(transform);
	}
}
