
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterGUI : BaseButtonGUI {
    [Header("Character Information")]
    public GameObject displayModel;
    public CharPeriod timePeriod;

    private DescriptionGUI descriptionGUIPanel;
    private TeamFormationGUI teamGUI;

    // Use this for initialization
    public override void  Awake() {
        base.Awake();
        descriptionGUIPanel = GetComponentInParent<DescriptionGUI>();
        teamGUI = descriptionGUIPanel.GetComponentInChildren<TeamFormationGUI>();
        displayModel.SetActive(false);
    }

    private void HideInfo() {
        displayModel.SetActive(false);
        descriptionGUIPanel.DescriptionPanel.SetActive(false);
    }
    private void ShowInfo() {

        descriptionGUIPanel.pointedChar = PlayerGroupGUI.SharedCharacterInfo[timePeriod];

        descriptionGUIPanel.DescriptionPanel.SetActive(true);
        descriptionGUIPanel.ShowCharacterInfo();
        displayModel.SetActive(true);
    }



    #region Event Handlers

    public override void OnPointerEnter(PointerEventData eventData) {
        base.OnPointerEnter(eventData);
        ShowInfo();

    }
    public override void OnPointerExit(PointerEventData eventData) {
        base.OnPointerExit(eventData);
        HideInfo();
    }

    public override void OnSelect(BaseEventData eventData) {
        base.OnSelect(eventData);
        ShowInfo();
    }

    public override void OnDeselect(BaseEventData eventData) {
        base.OnDeselect(eventData);
        HideInfo();
    }


    public override void OnCancel(BaseEventData eventData) {
        base.OnCancel(eventData);
        if (teamGUI.teamList.Count > 0) {// remove one by one the team members
            teamGUI.SetCharacter(teamGUI.teamList[teamGUI.teamList.Count - 1]);
            //TODO clear team in GAMEMANAGER
            

        }
        else {  //if the list is empty goes back to galaxy selection
            
            GuiManager.GUIInstance.BackToGalaxySelect();

        }

    }

    public override void OnSubmit(BaseEventData eventData) {
        base.OnSubmit(eventData);
        teamGUI.SetCharacter(timePeriod);
    }

    public override void OnPointerClick(PointerEventData eventData) {
        base.OnPointerClick(eventData);
        teamGUI.SetCharacter(timePeriod);
    }

    #endregion


}
