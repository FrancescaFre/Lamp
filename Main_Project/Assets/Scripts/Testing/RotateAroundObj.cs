using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundObj : MonoBehaviour {

    public Transform center;
    public float speed = 30f; 
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(center.position, transform.up, speed * Time.deltaTime);
	}
}
