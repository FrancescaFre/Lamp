using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlanetGUI : MonoBehaviour, IPointerEnterHandler ,ISelectHandler,ICancelHandler{

    public Level_SO planetLevel;
    private DescriptionGUI descriptionPanel;
    private void OnEnable() {
        GetComponentInParent<GalaxyButtonGUI>().FetchPlanets();
        descriptionPanel = GetComponentInParent<DescriptionGUI>();
    }

    public void ShowPlanetInfo() {
        if (!planetLevel) return;
        descriptionPanel.DescriptionPanel.SetActive(true);
        descriptionPanel.pointedLevel = planetLevel;
        descriptionPanel.ShowPlanetInfo();
    }

    public void OnPointerEnter(PointerEventData eventData) {    //allows to decide what happens when hovered 
        ShowPlanetInfo();
        
    }

    public void OnSelect(BaseEventData eventData) {
        ShowPlanetInfo();
    }

    public void OnCancel(BaseEventData eventData) {
        GameManager.Instance.levelsQueue=null;
        GUIManager.GUIInstance.nextButton.interactable = false;
        descriptionPanel.DescriptionPanel.SetActive(false);
        GUIManager.GUIInstance.eSystem.SetSelectedGameObject(transform.parent.parent.gameObject,null);
        transform.parent.gameObject.SetActive(false);
    }
}
