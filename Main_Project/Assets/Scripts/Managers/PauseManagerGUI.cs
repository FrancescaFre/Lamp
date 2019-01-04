﻿
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


/*
 * ESC/Start(or options)  Pause the game
 
     */
public class PauseManagerGUI : MonoBehaviour {
    [SerializeField]
    private bool _isPaused;
    public GameObject PausePanel;
    public Button firstButtonSelected;
    private float _originalFixedTime;   //this way it is possible to restore the previous value

    public LampHUD lampHUDPanel;
    public TextMeshProUGUI allyText;
    public TextMeshProUGUI enemyText;

    private OptionsGUI options;
    void Awake() {
        this._originalFixedTime = Time.fixedDeltaTime;
    }

    void Start() {
        _isPaused = false;
        options = GetComponentInChildren<OptionsGUI>(includeInactive:true);
        PausePanel.SetActive(_isPaused);
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("PS4_Button_OPTIONS")) {//once the button has JUST been released
                                                                                        //switches on and off the pause menu
            if (options.gameObject.activeInHierarchy) {
                options.gameObject.SetActive(false);
                PausePanel.SetActive(true);
                return;
            }
            this.ChangePauseStatus();

        }
    }
    private void LateUpdate() {
        if (GameManager.Instance) {
            allyText.text = string.Format("{0}/{1}", GameManager.Instance.allyLamps.ToString("00"), GameManager.Instance.levelLoaded.allyLamps.ToString("00"));
            enemyText.text = string.Format("{0}/{1}", GameManager.Instance.enemyLamps.ToString("00"), GameManager.Instance.levelLoaded.enemyLamps.ToString("00"));
        }
    }

    private void _UpdateGamePause(){
        if (_isPaused) {//stops time
            Time.timeScale = 0f;
            Time.fixedDeltaTime = .2f * Time.timeScale;
            EventSystem.current.SetSelectedGameObject(firstButtonSelected.gameObject, null);
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
        //Invoke("_UpdateGamePause",.5f);
        this._UpdateGamePause();
    }

    public void QuitGame() {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }

    public void LevelSelection() {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = _originalFixedTime;

        GameManager.Instance.EndGame();
    }
}
