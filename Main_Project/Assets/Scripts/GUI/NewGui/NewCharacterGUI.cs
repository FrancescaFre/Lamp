using UnityEngine;
using UnityEngine.EventSystems;

public class NewCharacterGUI : BaseButtonGUI {

    [Header("Character Information")]
    public CharPeriod timePeriod;

    private CharacterManagerGUI chManager;

    private NewTeamFormationGUI teamGUI;

    // Use this for initialization
    public override void Awake() {
        base.Awake();

        chManager = GetComponentInParent<CharacterManagerGUI>();
        teamGUI = chManager.GetComponentInChildren<NewTeamFormationGUI>();
    }

    private void HideInfo() {
        chManager.descriptionPanel.SetActive(false);
    }

    private void ShowInfo() {
        chManager.pointedChar = CharacterManagerGUI.SharedCharacterInfo[timePeriod];

        chManager.ShowCharacterInfo();
    }

    #region Event Handlers

    public override void OnPointerEnter(PointerEventData eventData) {
        base.OnPointerEnter(eventData);
        ShowInfo();
    }

    public override void OnPointerExit(PointerEventData eventData) {
        HideInfo();
        // if (teamGUI.teamList.Contains(timePeriod)) return;
        StopHalo();
    }

    public override void OnSelect(BaseEventData eventData) {
        base.OnSelect(eventData);
        ShowInfo();
    }

    public override void OnDeselect(BaseEventData eventData) {
        HideInfo();
        // if (teamGUI.teamList.Contains(timePeriod)) return;
        StopHalo();
    }

    public override void OnCancel(BaseEventData eventData) {
        base.OnCancel(eventData);
        if (teamGUI.teamList.Count > 0) {// remove one by one the team members
            teamGUI.SetCharacter(teamGUI.teamList[teamGUI.teamList.Count - 1]);
        }
        else {  //if the list is empty goes back to level selection
            NewGuiManager.instance.SwitchCharANDLevel();
        }
    }

    public override void OnSubmit(BaseEventData eventData) {
        if (!thisButton.interactable) return;
        base.OnSubmit(eventData);
        teamGUI.SetCharacter(timePeriod);
    }

    public override void OnPointerClick(PointerEventData eventData) {
        if (!thisButton.interactable) return;
        base.OnPointerClick(eventData);
        teamGUI.SetCharacter(timePeriod);
    }

    #endregion Event Handlers
}