using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GalaxyButtonGUI : BaseButtonGUI {


    [Header("Levels of the Galaxy")]
    public List<Level_SO> levels;
    public PlanetGUI firstPlanet;
    //public GameObject haloParticle;
    public override void Awake() {
        base.Awake();
        FetchPlanets();
    }

    private void Start() {
        ///in the beginning all the planets are active
        ///to allow the loading of the information
        ///and after the information fetching they are disabled again
        transform.GetChild(0).gameObject.SetActive(false);

    }



    public void SelectLevel() {

        EventSystem.current.SetSelectedGameObject(firstPlanet.gameObject, null);
        this.haloParticle.gameObject.SetActive(true);
    }


    /// <summary>
    /// For each planet of the galaxy (in order) associate the corresponding Level_SO
    /// </summary>
    public void FetchPlanets() {
        PlanetGUI[] temp = GetComponentsInChildren<PlanetGUI>();

        for (int i = 0; i < levels.Count; i++) {
          
            temp[i].planetLevel = levels[i];
        }
        firstPlanet = temp[0];
    }




    #region Event Handlers
    public override void OnSubmit(BaseEventData eventData) {
        SelectLevel();
    }

    public override void OnPointerClick(PointerEventData eventData) {
        SelectLevel();
    }


    public override void OnCancel(BaseEventData eventData) {
        if(!GameManager.Instance.levelLoaded) {
            SceneManager.LoadScene(0);
        }
        GameManager.Instance.levelLoaded = null;
        GuiManager.GUIInstance.nextButton.interactable = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }


    #endregion
}
