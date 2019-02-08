using UnityEngine.EventSystems;

public class NextGUI : BaseButtonGUI {

    public PlanetGUI selectedPlanet=null;
    public GalaxyButtonGUI selectedGalaxy=null;

    public  void Start() {
        
        if(!GuiManager.GUIInstance.nextButton)
            GuiManager.GUIInstance.nextButton = this;
    }

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
        if (!selectedPlanet) return;
        base.OnPointerEnter(eventData);
    }
    public override void OnSelect(BaseEventData eventData) {
        if (!selectedPlanet) return;
        base.OnSelect(eventData);
    }

}
