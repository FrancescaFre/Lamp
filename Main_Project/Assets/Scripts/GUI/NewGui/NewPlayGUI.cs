using UnityEngine;
using UnityEngine.EventSystems;

public class NewPlayGUI : BaseButtonGUI {

    [Header("play section")]
    public NewTeamFormationGUI teamGUI;

    private void Start() {
        NewGuiManager.instance.PlayButton = this;
        teamGUI = NewGuiManager.instance.GetComponentInChildren<NewTeamFormationGUI>();
    }

    public override void OnPointerClick(PointerEventData eventData) {
        NewGuiManager.instance.CheckSelectedTeam();
    }

    public override void OnSubmit(BaseEventData eventData) {
        NewGuiManager.instance.CheckSelectedTeam();
    }

    public override void OnCancel(BaseEventData eventData) {
        if (teamGUI.teamList.Count == 1)
            teamGUI.SetCharacter(teamGUI.teamList[0]);
        else if (teamGUI.teamList.Count == 2)
            teamGUI.SetCharacter(teamGUI.teamList[1]);
        else if (teamGUI.teamList.Count == 3)
            teamGUI.SetCharacter(teamGUI.teamList[2]);
        else
            NewGuiManager.instance.SwitchCharANDLevel();
    }

    public override void OnPointerEnter(PointerEventData eventData) {
        if (teamGUI.teamList.Count < teamGUI.MIN_TEAM_NUMBER) return;
        base.OnPointerEnter(eventData);
    }

    public override void OnSelect(BaseEventData eventData) {
        if (teamGUI.teamList.Count < teamGUI.MIN_TEAM_NUMBER) return;
        base.OnSelect(eventData);
    }
}