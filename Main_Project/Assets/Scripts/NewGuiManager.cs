using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewGuiManager : MonoBehaviour {
    public static NewGuiManager instance = null;

    [Header("Level Details")]
    public GameObject planetInfoPanel;
    public TextMeshProUGUI levelName;
    public TextMeshProUGUI epoch;
    public TextMeshProUGUI season;
    [Space]
    [Header("Challanges")]
    public List<TextMeshProUGUI> challanges;

    private void Awake() {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
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
}