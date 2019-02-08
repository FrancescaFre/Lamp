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


}
