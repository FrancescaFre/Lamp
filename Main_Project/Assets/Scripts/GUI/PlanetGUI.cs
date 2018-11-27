
using UnityEngine;
using UnityEngine.EventSystems;

public class PlanetGUI : BaseButtonGUI {

    public Level_SO planetLevel;
    private DescriptionGUI descriptionGUIPanel;

    public override void Awake() {
        base.Awake();
        descriptionGUIPanel = GetComponentInParent<DescriptionGUI>();
    }


    public void ShowPlanetInfo() {
        if (!planetLevel) return;
        descriptionGUIPanel.DescriptionPanel.SetActive(true);
        descriptionGUIPanel.pointedLevel = planetLevel;
        descriptionGUIPanel.ShowPlanetInfo();
    }






    private void LevelSelected() {
        
        GameManager.Instance.levelLoaded = planetLevel;
        GUIManager.GUIInstance.nextButton.interactable = true;
        GUIManager.GUIInstance.eSystem.SetSelectedGameObject(GUIManager.GUIInstance.nextButton.gameObject);
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
    public override void OnCancel(BaseEventData eventData) {
        base.OnCancel(eventData);

        GameManager.Instance.levelLoaded = null;
        GUIManager.GUIInstance.nextButton.interactable = false;
        descriptionGUIPanel.DescriptionPanel.SetActive(false);
        GUIManager.GUIInstance.eSystem.SetSelectedGameObject(transform.parent.parent.gameObject, null);
        transform.parent.gameObject.SetActive(false);
    }
    #endregion
}
