
using UnityEngine;
using UnityEngine.EventSystems;

public class PlanetGUI : BaseButtonGUI {

    public Level_SO planetLevel;
    private DescriptionGUI descriptionGUIPanel;
    public GalaxyButtonGUI parentGalaxy;
    private NextGUI nextButton;

    public override void Awake() {
        base.Awake();
        descriptionGUIPanel = GetComponentInParent<DescriptionGUI>();
        parentGalaxy = GetComponentInParent<GalaxyButtonGUI>();

    }

    private void Start() {
        nextButton = GuiManager.GUIInstance.nextButton.GetComponent<NextGUI>();

    }
    public void ShowPlanetInfo() {
        if (!planetLevel) return;
        descriptionGUIPanel.DescriptionPanel.SetActive(true);
        descriptionGUIPanel.pointedLevel = planetLevel;
        descriptionGUIPanel.ShowPlanetInfo();
    }






    private void LevelSelected() {

   
        GameManager.Instance.levelLoaded = planetLevel;
        GuiManager.GUIInstance.nextButton.interactable = true;

        
        if (nextButton.selectedPlanet && nextButton.selectedPlanet != this) 
            nextButton.selectedPlanet.StopHalo();
            
        
        nextButton.selectedPlanet = this;
        nextButton.haloParticle.gameObject.SetActive(GuiManager.GUIInstance.nextButton.interactable);

        EventSystem.current.SetSelectedGameObject(nextButton.gameObject, null);
        this.haloParticle.SetActive(true);
    }


    #region Event Handlers
    public override void OnPointerClick(PointerEventData eventData) {
        base.OnPointerClick(eventData);
        LevelSelected();

    }

    public override void OnSubmit(BaseEventData eventData) {
        base.OnSubmit(eventData);
        LevelSelected();

    }
    public override void OnPointerEnter(PointerEventData eventData) {    //allows to decide what happens when hovered 
        base.OnPointerEnter(eventData);
        ShowPlanetInfo();

    }

    public override void OnSelect(BaseEventData eventData) {
        base.OnSelect(eventData);
        ShowPlanetInfo();
    }

    public override void OnPointerExit(PointerEventData eventData) {
        if (nextButton.selectedPlanet && nextButton.selectedPlanet == this) return;
        StopHalo();
    }



    public override void OnDeselect(BaseEventData eventData) {
        if (nextButton.selectedPlanet && nextButton.selectedPlanet == this) return;
        StopHalo();
    }

    public override void OnCancel(BaseEventData eventData) {
        base.OnCancel(eventData);

        GameManager.Instance.levelLoaded = null;
        GuiManager.GUIInstance.nextButton.interactable = false;
        GuiManager.GUIInstance.nextButton.transform.GetChild(0).gameObject.SetActive(GuiManager.GUIInstance.nextButton.interactable);
        descriptionGUIPanel.DescriptionPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(transform.parent.parent.gameObject, null);

        ///switch off all the halos of the planets
        for (int i = 0; i < this.haloParticle.transform.parent.childCount; i++) {
            this.haloParticle.transform.parent.GetChild(i).gameObject.SetActive(false);
        }


        transform.parent.gameObject.SetActive(false);
    }
    #endregion
}
