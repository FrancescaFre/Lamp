
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



/*
 * ESC/Start(or options)  Pause the game
 
     */
public class PauseManagerGUI : MonoBehaviour
{
    [SerializeField]
    public bool IsPaused { get; set; }
    public GameObject PausePanel;
    public GameObject firstButtonSelected;
    private float _originalFixedTime;   //this way it is possible to restore the previous value
    public GameObject firstOptionSelected;
    public GameObject firstTutorialSelected;

    private TutorialGUI _tutorialGUI;
    private OptionsGUI _optionsGUI;

    void Awake()
    {
        this._originalFixedTime = Time.fixedDeltaTime;
    }

    void Start()
    {
        IsPaused = false;
        _optionsGUI = GetComponentInChildren<OptionsGUI>(includeInactive: true);
        _tutorialGUI = GetComponentInChildren<TutorialGUI>(includeInactive: true);

        if (!PausePanel)
            foreach (Transform child in transform)
                if (child.CompareTag(Tags.HUDPauseMenu))
                    PausePanel = child.gameObject;

        PausePanel.SetActive(IsPaused);
        firstButtonSelected = PausePanel.transform.GetChild(0).gameObject;
        firstOptionSelected = _optionsGUI.transform.GetChild(0).gameObject;
       // firstTutorialSelected = _tutorialGUI.firstSelected;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp(Controllers.PS4_Button_OPTIONS))
        {//once the button has JUST been released
         //switches on and off the pause menu
            if (_optionsGUI.gameObject.activeInHierarchy)
            {
                OptionsToPause();

                return;
            }
            if (_tutorialGUI.gameObject.activeInHierarchy)
            {
                TutorialToPause();
                return;
            }
            this.ChangePauseStatus();

        }
    }


    private void _UpdateGamePause()
    {
        GetComponent<InGameHUD>().InGameHUDPanel.SetActive(!IsPaused);
        PausePanel.SetActive(IsPaused);    //the activation of the panel depends on whether it is paused or not
        Cursor.visible = IsPaused;


        if (IsPaused)
        {//stops time
            Time.timeScale = 0f;
            Time.fixedDeltaTime = .2f * Time.timeScale;
            EventSystem.current.SetSelectedGameObject(firstButtonSelected, null);
        }
        else
        {//restores time
            Time.timeScale = 1f;
            Time.fixedDeltaTime = _originalFixedTime;
        }

    }

    /// <summary>
    /// Call this function to change the pause status (even from a button in-game)
    /// </summary>
    public void ChangePauseStatus()
    {// this allows to press a button and call this function to resume
        IsPaused = !IsPaused;

        this._UpdateGamePause();
    }

    public void QuitGame()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }

    public void LevelSelection()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = _originalFixedTime;

        GameManager.Instance.EndGame();
    }

    public void PauseToOptions()
    {
        PausePanel.SetActive(false);
        _optionsGUI.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstOptionSelected, null);
    }
    public void OptionsToPause()
    {
        _optionsGUI.gameObject.SetActive(false);
        PausePanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(firstButtonSelected, null);
    }

    public void PauseToTutorial()
    {
        PausePanel.SetActive(false);
        _tutorialGUI.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstTutorialSelected, null);
    }
    public void TutorialToPause()
    {
        _tutorialGUI.Escape();
        PausePanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(firstButtonSelected, null);
    }

}
