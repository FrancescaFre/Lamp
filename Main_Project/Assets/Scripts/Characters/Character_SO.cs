using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

[CreateAssetMenu]
public class Character_SO : ScriptableObject {
    [Header("Main Menu Informations")]
    public string charName;
    public string description;
    public CharPeriod timePeriod;
    public Sprite Skill;
    public string SkillDescription;
    public Sprite CharSprite;

    [Header("In Game Information")]
    public List<AudioClip> hurtVoiceList;    


}
