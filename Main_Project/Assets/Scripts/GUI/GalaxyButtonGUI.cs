using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

        nextButton = GuiManager.GUIInstance.nextButton.GetComponent<NextGUI>();
        planetGroup.SetActive(false);
    }



    public void SelectLevel() {
        if (nextButton.selectedGalaxy && nextButton.selectedGalaxy != this) {
            nextButton.selectedGalaxy.StopHalo();
            nextButton.selectedPlanet.StopHalo();
        }

        nextButton.selectedGalaxy = this;

        if (nextButton.selectedPlanet && nextButton.selectedPlanet.parentGalaxy==this)
            nextButton.selectedPlanet.StartHalo();


        EventSystem.current.SetSelectedGameObject(firstPlanet.gameObject, null);
        this.haloParticle.gameObject.SetActive(true);
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
    public override void OnSubmit(BaseEventData eventData) {
        base.OnSubmit(eventData);
        SelectLevel();
    }

    public override void OnPointerClick(PointerEventData eventData) {
        base.OnPointerClick(eventData);
        SelectLevel();
    }

    public override void OnPointerExit(PointerEventData eventData) {
        if (nextButton.selectedGalaxy && nextButton.selectedGalaxy == this) return;
        StopHalo();
    }



    public override void OnDeselect(BaseEventData eventData) {
        if (nextButton.selectedGalaxy && nextButton.selectedGalaxy == this) return;
        StopHalo();
    }
    public override void OnCancel(BaseEventData eventData) {
        base.OnCancel(eventData);
        if (!GameManager.Instance.levelLoaded) {
            SceneManager.LoadScene(0);
        }
        GameManager.Instance.levelLoaded = null;
        GuiManager.GUIInstance.nextButton.interactable = false;
        GuiManager.GUIInstance.nextButton.transform.GetChild(0).gameObject.SetActive(GuiManager.GUIInstance.nextButton.interactable);
        planetGroup.SetActive(false);

    }


    #endregion
}