using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("PS4_L1") || Input.GetKeyDown(KeyCode.I)) { //https://bit.ly/2yQF6uM  Documentation link
            Time.timeScale = .5f;   //halves the scale at which the time is passing
        }

        if (Input.GetButtonUp("PS4_L1") || Input.GetKeyUp(KeyCode.I)) {// restores the time scales
            Time.timeScale = 1f;
        }
    }
}
