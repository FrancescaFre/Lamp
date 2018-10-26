using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityBody : MonoBehaviour {

    public FauxGravityAttract fauxAttractor;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //fauxAttractor.Attract(gameObject.transform);
        fauxAttractor.Attract(rb);
	}
}
