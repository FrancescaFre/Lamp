
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



/*
 * ESC/Start(or options)  Pause the game
 
     */
public class PauseManagerGUI : MonoBehaviour {
    [SerializeField]
    private bool _isPaused;
    public GameObject PausePanel;
    public GameObject firstButtonSelected;
    private float _originalFixedTime;   //this way it is possible to restore the previous value
    public GameObject firstOptionSelected;



    private OptionsGUI optionsGUI;

    void Awake() {
        this._originalFixedTime = Time.fixedDeltaTime;
    }

    void Start() {
        _isPaused = false;
        optionsGUI = GetComponentInChildren<OptionsGUI>(includeInactive:true);
        PausePanel.SetActive(_isPaused);
        firstButtonSelected = PausePanel.transform.GetChild(0).gameObject;
        firstOptionSelected = optionsGUI.transform.GetChild(0).gameObject;

    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("PS4_Button_OPTIONS")) {//once the button has JUST been released
                                                                                        //switches on and off the pause menu
            if (optionsGUI.gameObject.activeInHierarchy) {
                OptionsToPause();
                
                return;
            }
            this.ChangePauseStatus();

        }
    }


    private void _UpdateGamePause(){
        GetComponent<InGameHUD>().InGameHUDPanel.SetActive(!_isPaused);
        PausePanel.SetActive(_isPaused);    //the activation of the panel depends on whether it is paused or not
        Cursor.visible = _isPaused;
        
        
        if (_isPaused) {//stops time
            Time.timeScale = 0f;
            Time.fixedDeltaTime = .2f * Time.timeScale;
            EventSystem.current.SetSelectedGameObject(firstButtonSelected, null);
        }
        else {//restores time
            Time.timeScale = 1f;
            Time.fixedDeltaTime = _originalFixedTime;
        }
        
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

    public void LevelSelection() {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = _originalFixedTime;

        GameManager.Instance.EndGame();
    }

    public void PauseToOptions() {
        PausePanel.SetActive(false);
        optionsGUI.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstOptionSelected,null);
    }
    public void OptionsToPause() {
        optionsGUI.gameObject.SetActive(false);
        PausePanel.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(firstButtonSelected, null);
    }

    
}
