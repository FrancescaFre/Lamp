using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {
    public static GUIManager GUIInstance;
    [Header("Menu list")]
    public GameObject StartMenu;
    public GameObject Galaxies;
    public GameObject Characters;
    
    [Header("First selected object in each menu")]
    public GameObject firstSelectedPlayButton;
    public GameObject firstSelectedGalaxy;
    public GameObject firstSelectedCharacter;

    [Header("Buttons to the next menu")]
    public Button nextButton;
    public Button PlayButton;

    public EventSystem eSystem;

    private void Awake() {
        if (!GUIInstance)
            GUIInstance = this;
        else
            Destroy(gameObject);
    }
    private void Start() {
        StartMenu.SetActive(true);
        Galaxies.SetActive(false);
        Characters.SetActive(false);
        eSystem = FindObjectOfType<EventSystem>();
        eSystem.SetSelectedGameObject(firstSelectedPlayButton, null);
       
    }

    public void OnStartPressed() {
        Invoke("DelayStartButton", 1f);//allows the SFX to end
    } 
    private void DelayStartButton() {
        StartMenu.SetActive(false);
        Galaxies.SetActive(true);

        Debug.Log("StartGame");
        eSystem.SetSelectedGameObject(firstSelectedGalaxy, null);
    }
    public void CheckSelectedGalaxy() {
        if (GameManager.Instance.levelsQueue == null) {
            nextButton.interactable = false;
            return;
        }

        Invoke("DelayNextButton", 1f);//allows the SFX to end
    }

    private void DelayNextButton() {
        Galaxies.SetActive(false);
        Characters.SetActive(true);
        eSystem.SetSelectedGameObject(firstSelectedCharacter, null);
    }

    public void CheckSelectedTeam() {
        if (GameManager.Instance.TeamList != null && GameManager.Instance.TeamList.Count == 3)
            GameManager.Instance.StartGame();
        else
            PlayButton.interactable = false;

    }

    //to turn backonce reached the team selection
    public void BackToGalaxySelect() {
        
        Characters.SetActive(false);
        Galaxies.SetActive(true);
        eSystem.SetSelectedGameObject(firstSelectedGalaxy, null);
    }
}
