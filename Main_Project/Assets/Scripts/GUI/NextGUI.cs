using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NextGUI : BaseButtonGUI {

    public PlanetGUI selectedPlanet;


    public override void OnCancel(BaseEventData eventData) {
        selectedPlanet.OnCancel(eventData);
    }
    public override void OnPointerClick(PointerEventData eventData) {
        GuiManager.GUIInstance.CheckSelectedLevel();
    }
    public override void OnSubmit(BaseEventData eventData) {
        GuiManager.GUIInstance.CheckSelectedLevel();
    }

    public override void OnPointerEnter(PointerEventData eventData) {
        
    }
    public override void OnSelect(BaseEventData eventData) {
     

    }
    public override void OnPointerExit(PointerEventData eventData) {
 
    }
    public override void OnDeselect(BaseEventData eventData) {

    }
}
