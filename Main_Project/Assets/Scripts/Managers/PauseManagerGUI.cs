
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

    public LampGUI lampGUIInfo;
    public TextMeshProUGUI allyText;
    public TextMeshProUGUI enemyText;


    void Awake() {
        this._originalFixedTime = Time.fixedDeltaTime;
    }

    void Start() {
        _isPaused = false;
       // lampGUIInfo = GetComponentInChildren<LampGUI>();
        PausePanel.SetActive(_isPaused);
    }

    // Update is called once per frame
    void Update() {

        allyText.text = string.Format("{0}/{1}",GameManager.Instance.allyLamps.ToString("00"),lampGUIInfo.allyLamp.ToString("00"));
        enemyText.text = string.Format("{0}/{1}",GameManager.Instance.enemyLamps.ToString("00"), lampGUIInfo.enemyLamp.ToString("00"));

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("PS4_Button_OPTIONS")) {//once the button has JUST been released
                                                                                 //switches on and off the pause menu

            this.ChangePauseStatus();

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

        GameManager.Instance.EndGame();
    }
}
