
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayGUI : BaseButtonGUI {

    [Header("play section")]
    public TeamFormationGUI teamGUI;


    private void Start() {
        GuiManager.GUIInstance.PlayButton = this;
    }

    public override void OnPointerClick(PointerEventData eventData) {
        GuiManager.GUIInstance.CheckSelectedTeam();
    }
    public override void OnSubmit(BaseEventData eventData) {
        GuiManager.GUIInstance.CheckSelectedTeam();
    }

    public override void OnCancel(BaseEventData eventData) {
        if (teamGUI.teamList.Count == 1)
            teamGUI.SetCharacter(teamGUI.teamList[0]);
        else if (teamGUI.teamList.Count == 2)
            teamGUI.SetCharacter(teamGUI.teamList[1]);
        else if (teamGUI.teamList.Count == 3)
            teamGUI.SetCharacter(teamGUI.teamList[2]);
    }
}
