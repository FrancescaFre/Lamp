using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountingSomething : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach (AudioSource audio in FindObjectsOfType<AudioSource>())
            Debug.Log(audio.transform.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
