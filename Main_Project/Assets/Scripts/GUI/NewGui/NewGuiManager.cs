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

    [Header("Level Details")]
    public GameObject planetInfoPanel;
    public TextMeshProUGUI levelName;
    public TextMeshProUGUI epoch;
    public TextMeshProUGUI season;

    [Header("Challanges")]
    public List<TextMeshProUGUI> challanges;

    [Space]
    public NewPlayGUI PlayButton;

    private void Awake() {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        thisCanvas = GetComponent<Canvas>();

        planetInfoPanel.SetActive(false);
    }

    public void ShowLevelInfo() {
        planetInfoPanel.SetActive(true);
        levelName.text = GameManager.Instance.levelLoaded.LevelName;
        epoch.text = GameManager.Instance.levelLoaded.epoch.ToString();
        season.text = GameManager.Instance.levelLoaded.levelSeason.ToString();
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

    public void SwitchCharANDLevel(bool charSelection = false) {
        thisCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        planetInfoPanel.SetActive(!planetInfoPanel.activeInHierarchy);
        onScreenAnchor.SetActive(!onScreenAnchor.activeInHierarchy);

        charManager.gameObject.SetActive(!charManager.gameObject.activeInHierarchy);

        if (!charSelection) {
            spaceShip.charSelection = false;
            spaceShip.shipModel.SetActive(true);
            GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            charManager.HideModels();
        }
        else {
            charManager.ShowModels();
        }
    }

    private void LateUpdate() {
        if (!GameManager.Instance.levelLoaded && GameManager.Instance.TeamList == null && Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
        if (GameManager.Instance.levelLoaded && PlayButton.teamGUI.teamList.Count == 0 && Input.GetKeyDown(KeyCode.Escape))
            SwitchCharANDLevel();
    }

    public void CheckSelectedTeam() {
        if (GameManager.Instance.TeamList != null && GameManager.Instance.TeamList.Count >= 2 && GameManager.Instance.levelLoaded)
            GameManager.Instance.LoadGame();
        else {
            PlayButton.thisButton.interactable = false;
            // PlayButton.transform.GetChild(0).gameObject.SetActive(PlayButton.interactable);
            PlayButton.StopHalo();
        }
    }
}