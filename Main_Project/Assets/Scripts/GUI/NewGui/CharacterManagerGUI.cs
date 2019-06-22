using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterManagerGUI : MonoBehaviour {
    public GameObject descriptionPanel;
    public Character_SO pointedChar;

    [Header("Characters")]
    public List<GameObject> models;
    public List<Character_SO> charList;

    [Header("Basic information")]
    public TMP_Text Name;
    public TMP_Text Description;
    public TMP_Text Period;

    public Image CharSkill;
    public TMP_Text CharSkillDescription;

    public static Dictionary<CharPeriod, Character_SO> SharedCharacterInfo;
    public static Dictionary<CharPeriod, NewCharacterGUI> SharedCharactersGUI;

    private void Awake() {
        SharedCharacterInfo = new Dictionary<CharPeriod, Character_SO>(4);
        SharedCharactersGUI = new Dictionary<CharPeriod, NewCharacterGUI>(4);
    }

    private void Start() {
        HideModels();
        charList.ForEach(ch => SharedCharacterInfo[ch.timePeriod] = ch);
        List<NewCharacterGUI> tp = new List<NewCharacterGUI>(GetComponentsInChildren<NewCharacterGUI>());
        tp.ForEach(chGUI => SharedCharactersGUI[chGUI.timePeriod] = chGUI);
        gameObject.SetActive(false);
    }

    public void ShowModels() {
        models.ForEach(go => go.SetActive(true));
    }

    public void HideModels() {
        models.ForEach(go => go.SetActive(false));
    }

    public void ShowCharacterInfo() {
        descriptionPanel.SetActive(true);
        Name.SetText(pointedChar.charName);
        Description.SetText(pointedChar.description);
        Period.SetText(pointedChar.timePeriod.ToString());

        CharSkill.sprite = pointedChar.Skill;
        CharSkillDescription.SetText(pointedChar.SkillDescription);
    }
}