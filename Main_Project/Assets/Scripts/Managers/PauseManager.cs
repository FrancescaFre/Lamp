
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour {

    private bool _isPaused;
    public GameObject PausePanel;
    public Button firstButtonSelected;
    private float _originalFixedTime;   //this way it is possible to restore the previous value
    private EventSystem eSystem;


    void Awake() {
        this._originalFixedTime = Time.fixedDeltaTime;
    }

    void Start() {
        _isPaused = false;
        eSystem = FindObjectOfType<EventSystem>();
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
            eSystem.SetSelectedGameObject(firstButtonSelected.gameObject, null);
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

    public void QuitGame() {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }
}
