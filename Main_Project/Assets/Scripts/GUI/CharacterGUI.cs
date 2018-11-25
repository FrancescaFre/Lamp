using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, ICancelHandler, ISubmitHandler, IPointerClickHandler {

    public GameObject displayModel;
    public CharPeriod timePeriod;

    private DescriptionGUI descriptionGUI;
    private TeamFormationGUI teamGUI;

    // Use this for initialization
    void Start() {
        descriptionGUI = GetComponentInParent<DescriptionGUI>();
        teamGUI = descriptionGUI.GetComponentInChildren<TeamFormationGUI>();
    }


    #region Input interaction

    public void OnPointerEnter(PointerEventData eventData) {
        ShowInfo();

    }
    public void OnPointerExit(PointerEventData eventData) {
        HideInfo();
    }

    public void OnSelect(BaseEventData eventData) {
        ShowInfo();
    }

    public void OnDeselect(BaseEventData eventData) {
        HideInfo();
    }
    private void HideInfo() {
        displayModel.SetActive(false);
        descriptionGUI.DescriptionPanel.SetActive(false);
    }

    public void OnCancel(BaseEventData eventData) {
        
        if (teamGUI.teamList.Count > 0) {// remove one by one the team members
            teamGUI.SetCharacter(teamGUI.teamList[teamGUI.teamList.Count - 1]);
            //TODO clear team in GAMEMANAGER
            Debug.Log("CANCEL TEAM");
        }
        else {  //if the list is empty goes back to galaxy selection
            Debug.Log("BACK TO GALAXY");
            GUIManager.GUIInstance.BackToGalaxySelect();
            
        }

    }

    public void OnSubmit(BaseEventData eventData) {
        teamGUI.SetCharacter(timePeriod);
    }

    public void OnPointerClick(PointerEventData eventData) {
        teamGUI.SetCharacter(timePeriod);
    }

    #endregion


    private void ShowInfo() {
       
        descriptionGUI.pointedChar = PlayerGroupGUI.SharedCharacterInfo[timePeriod];

        descriptionGUI.DescriptionPanel.SetActive(true);
        descriptionGUI.ShowCharacterInfo();
        displayModel.SetActive(true);
    }




}
