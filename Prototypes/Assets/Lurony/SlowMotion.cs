using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour {
    private float _originalFixedTime;   //this way it is possible to restore the previous value

    void Awake() {
        this._originalFixedTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetButton("PS4_L1") || Input.GetKey(KeyCode.I)) { //https://bit.ly/2yQF6uM  Documentation link
            Time.timeScale = .5f;   //halves the scale at which the time is passing as long as the button is held down
            Time.fixedDeltaTime = .2f * Time.timeScale;
        }
        else {// restores the time scales
            Time.timeScale = 1f;
            Time.fixedDeltaTime = _originalFixedTime;
        }
    }
}
