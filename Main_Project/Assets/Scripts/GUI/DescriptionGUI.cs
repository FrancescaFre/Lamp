using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionGUI : MonoBehaviour {


    [Header("Description panel")]
    public GameObject DescriptionPanel;

    [Header("Basic information")]
    public TMP_Text Name;
    public TMP_Text Description;
    public TMP_Text Period;
    [Header("For the characters")]
    public Character_SO pointedChar;
    public Image CharSkill;
    public TMP_Text CharSkillDescription;
    [Header("For the levels")]
    public Level_SO pointedLevel;
    public TMP_Text[] subQuests;

    private void Start() {
        DescriptionPanel.SetActive(false);
    }

    /// <summary>
    /// Shows the information of the selected character
    /// </summary>
    public  void ShowCharacterInfo() {
  

        Name.SetText(pointedChar.charName);
        Description.SetText(pointedChar.description);
        Period.SetText(pointedChar.timePeriod.ToString());

        CharSkillDescription.SetText(pointedChar.SkillDescription);

        CharSkill.sprite = pointedChar.Skill;

    }

    /// <summary>
    /// Shows the information of the selcted planet
    /// </summary>
    public void ShowPlanetInfo() {
        Name.SetText(pointedLevel.LevelName);
        Period.SetText(pointedLevel.levelSeason.ToString());
        Description.SetText(pointedLevel.Description);

        /*SubQuest1.SetText(pointedLevel.subQuest_1);
        SubQuest2.SetText(pointedLevel.subQuest_2);
        SubQuest3.SetText(pointedLevel.subQuest_3);*/

        for (int i = 0; i < 3; i++) {
            subQuests[i].SetText(pointedLevel.subQuests[i]);
        }
    }



}
