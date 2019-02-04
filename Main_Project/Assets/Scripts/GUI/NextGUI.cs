using UnityEngine.EventSystems;

public class NextGUI : BaseButtonGUI {

    public PlanetGUI selectedPlanet;
    public GalaxyButtonGUI selectedGalaxy;

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
