using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {

    public static GuiManager Instance;
    public Transform CharInfoPanel;
    [Header("Description panel")]
    public TMP_Text CharName;
    public TMP_Text CharDesc;
    public TMP_Text CharPeriod;
    public Image CharSkill;
    public TMP_Text CharSkillDescription;

    private void Start() {
        if (!Instance)
            Instance = this;

        
    }

    private void FixedUpdate() {
        if (!PlayerGroup.pointedChar) return;

        CharName.SetText(PlayerGroup.pointedChar.charName);
        CharDesc.SetText(PlayerGroup.pointedChar.description);
        CharPeriod.SetText(PlayerGroup.pointedChar.timePeriod.ToString());
        CharSkillDescription.SetText(PlayerGroup.pointedChar.SkillDescription);
        CharSkill.sprite = PlayerGroup.pointedChar.Skill;
    }
}
