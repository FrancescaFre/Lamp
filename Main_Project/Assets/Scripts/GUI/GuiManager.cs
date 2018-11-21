using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {
    public GameObject Characters;
    public GameObject Galaxies;

    public GameObject firstSelectedGalaxy;
    public GameObject firstSelectedCharacter;


    public Button nextButton;
    public Button StartButton;

    private EventSystem eSystem;


    private void Start() {
        Galaxies.SetActive(true);
        Characters.SetActive(false);
        eSystem = FindObjectOfType<EventSystem>();
        eSystem.SetSelectedGameObject(firstSelectedGalaxy, null);
       
    }

    public void CheckSelectedGalaxy() {
        if (GameManager.Instance.levelsQueue == null) {
            nextButton.interactable = false;
            return;
        }
        
        
        Galaxies.SetActive(false);
        Characters.SetActive(true);
        eSystem.SetSelectedGameObject(firstSelectedCharacter, null);
    }


    public void CheckSelectedTeam() {
        //if(GameManager.Instance.)
        GameManager.Instance.StartGame();

    }

    //to turn backonce reached the team selection
    public void BackToGalaxySelect() {
        
        Characters.SetActive(false);
        Galaxies.SetActive(true);
        eSystem.SetSelectedGameObject(firstSelectedGalaxy, null);
    }
}
