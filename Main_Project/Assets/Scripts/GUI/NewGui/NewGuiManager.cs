using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NewGuiManager : MonoBehaviour {
    public static NewGuiManager instance = null;
    private Canvas thisCanvas;

    public CharacterManagerGUI charManager;
    public MovingSpaceShip spaceShip;

    [Header("Overlay UI")]
    public GameObject onScreenAnchor;
    public GameObject continueLabelPrefab;

    public List<Image> Frames_Full;

    [Header("Level Details")]
    public GameObject planetInfoPanel;
    public TextMeshProUGUI levelName;
    public TextMeshProUGUI epoch;
    public TextMeshProUGUI season;

    [Header("Challanges")]
    public List<TextMeshProUGUI> challanges;

    [Space]
    public NewPlayGUI PlayButton;

    public NewTeamFormationGUI teamGUI;

    private void Awake() {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        thisCanvas = GetComponent<Canvas>();
        teamGUI = GetComponentInChildren<NewTeamFormationGUI>(includeInactive: true);
        planetInfoPanel.SetActive(false);
    }
    private void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown(Controllers.PS4_Button_O)) {
            if (teamGUI.teamList.Count > 0) {
                teamGUI.SetCharacter(teamGUI.teamList[teamGUI.teamList.Count - 1]);
                return;

            }

            if (!GameManager.Instance.levelLoaded && GameManager.Instance.TeamList == null) {
                SceneManager.LoadScene(0);
                return;
            }

            if (spaceShip.toCharSelection
             && GameManager.Instance.levelLoaded
             && (teamGUI.teamList == null || teamGUI.teamList.Count == 0)) {
                SwitchCharANDLevel();
            }
        }

    }

    public void ShowLevelInfo(){
        Sprite frame_full = GameManager.Instance.levelLoaded.levelFrameFull;

        if(Frames_Full!=null && Frames_Full.Count>0)
            Frames_Full.ForEach(img=> img.sprite=frame_full);

        levelName.text = GameManager.Instance.levelLoaded.LevelName;
        epoch.text = GameManager.Instance.levelLoaded.epoch.ToString();
        season.text = GameManager.Instance.levelLoaded.levelSeason.ToString();

        planetInfoPanel.SetActive(true);
        for (int i = 0; i < 3; i++) {
            challanges[i].text = GameManager.Instance.levelLoaded.subQuests[i];
            challanges[i].GetComponentInChildren<Toggle>().isOn = GameManager.Instance.levelLoaded.questCompletion[i];
        }
    }

    public void HideLevelInfo() {
        planetInfoPanel.SetActive(false);
    }

    public void CreateHUDReference(ref GameObject confirmLabel) {
        confirmLabel = Instantiate(continueLabelPrefab, onScreenAnchor.transform);
        confirmLabel.SetActive(false);
    }

    public void SwitchCharANDLevel(bool toCharSelection = false) {
        
        planetInfoPanel.SetActive(!planetInfoPanel.activeInHierarchy);
        onScreenAnchor.SetActive(!onScreenAnchor.activeInHierarchy);

        charManager.gameObject.SetActive(!charManager.gameObject.activeInHierarchy);

        if (toCharSelection){
            thisCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            charManager.ShowModels();
            return;
        }

    
        spaceShip.toCharSelection = false;
        spaceShip.shipModel.SetActive(true);
        thisCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        charManager.HideModels();
       
    }



    public void CheckSelectedTeam() {
        if (GameManager.Instance.TeamList != null && GameManager.Instance.TeamList.Count >= 2 && GameManager.Instance.levelLoaded)
            GameManager.Instance.LoadGame();
        else {
            PlayButton.thisButton.interactable = false;
            
            PlayButton.StopHalo();
        }
    }
}