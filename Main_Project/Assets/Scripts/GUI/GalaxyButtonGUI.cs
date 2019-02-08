using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GalaxyButtonGUI : BaseButtonGUI {


    [Header("Levels of the Galaxy")]
    public List<Level_SO> levels;
    public PlanetGUI firstPlanet;
    private NextGUI nextButton;
    public GameObject planetGroup;
    


    public override void Awake() {
        base.Awake();

        foreach (Transform child in transform) {
            if (child.CompareTag(Tags.GUIGroup))
                planetGroup = child.gameObject;
        }

        FetchPlanets();
    }

    private void Start() {
        ///in the beginning all the planets are active
        ///to allow the loading of the information
        ///and after the information fetching they are disabled again

        nextButton = GuiManager.GUIInstance.nextButton ;
        planetGroup.SetActive(false);
        
    }



    public void SelectGalaxy() {
        if (!nextButton)
            nextButton = GuiManager.GUIInstance.nextButton;
        if (nextButton.selectedGalaxy && nextButton.selectedGalaxy != this) {
            nextButton.selectedGalaxy.StopHalo();
            nextButton.selectedGalaxy.SetHighlight();
            if (nextButton.selectedPlanet) {
                nextButton.selectedPlanet.StopHalo();
                

            }
        }

        nextButton.selectedGalaxy = this;

        if (nextButton.selectedPlanet && nextButton.selectedPlanet.parentGalaxy==this)
            nextButton.selectedPlanet.StartHalo();


        EventSystem.current.SetSelectedGameObject(firstPlanet.gameObject, null);
        this.haloParticleGO.gameObject.SetActive(true);
    }


    /// <summary>
    /// For each planet of the galaxy (in order) associate the corresponding Level_SO
    /// </summary>
    public void FetchPlanets() {
        PlanetGUI[] temp = planetGroup.GetComponentsInChildren<PlanetGUI>();

        for (int i = 0; i < levels.Count; i++) {

            temp[i].planetLevel = levels[i];
        }
        firstPlanet = temp[0];
    }




    #region Event Handlers
    public override void OnPointerEnter(PointerEventData eventData) {
        if (!thisButton.interactable) return;
        StartHalo();
    }
    public override void OnSelect(BaseEventData eventData) {
        if (!thisButton.interactable) return;
        StartHalo();

    }

    public override void OnSubmit(BaseEventData eventData) {
        if (!thisButton.interactable) return;
        base.OnSubmit(eventData);
        SelectGalaxy();
    }

    public override void OnPointerClick(PointerEventData eventData) {
        if (!thisButton.interactable) return;
        base.OnPointerClick(eventData);
        SelectGalaxy();
    }

    public override void OnPointerExit(PointerEventData eventData) {
        if(!nextButton)
            nextButton = GuiManager.GUIInstance.nextButton;
        if (nextButton.selectedGalaxy && nextButton.selectedGalaxy == this) return;
        if (!thisButton.interactable) return;
        StopHalo();
    }



    public override void OnDeselect(BaseEventData eventData) {
        if (!nextButton)
            nextButton = GuiManager.GUIInstance.nextButton;
        if (nextButton.selectedGalaxy && nextButton.selectedGalaxy == this) return;
        if (!thisButton.interactable) return;
        StopHalo();
    }
    public override void OnCancel(BaseEventData eventData) {
        base.OnCancel(eventData);
        if (!GameManager.Instance.levelLoaded) {
            SceneManager.LoadScene(0);
        }
        GameManager.Instance.levelLoaded = null;
        GuiManager.GUIInstance.nextButton.thisButton.interactable = false;
        GuiManager.GUIInstance.nextButton.StopHalo();
        planetGroup.SetActive(false);

    }


    #endregion
}