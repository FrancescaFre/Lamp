using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

    private bool _isPaused;
    public GameObject PausePanel;
    private float _originalFixedTime;   //this way it is possible to restore the previous value

    void Awake() {
        this._originalFixedTime = Time.fixedDeltaTime;
    }

    void Start() {
        _isPaused = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("PS4_Button_OPTIONS")) {//once the button has JUST been released
                                                                                 //switches on and off the pause menu

            this.ChangePauseStatus();

        }
    }

    private void _UpdateGamePause(){
        if (_isPaused) {//stops time
            Time.timeScale = 0f;
            Time.fixedDeltaTime = .2f * Time.timeScale;
        }
        else {//restores time
            Time.timeScale = 1f;
            Time.fixedDeltaTime = _originalFixedTime;
        }

        PausePanel.SetActive(_isPaused);    //the activation of the panel depends on whether it is paused or not
    }

    /// <summary>
    /// Call this function to change the pause status (even from a button in-game)
    /// </summary>
    public void ChangePauseStatus() {// this allows to press a button and call this function to resume
        _isPaused = !_isPaused;
        this._UpdateGamePause();
    }
}
