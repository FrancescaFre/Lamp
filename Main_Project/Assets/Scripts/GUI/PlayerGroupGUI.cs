
using System.Collections.Generic;
using UnityEngine;


public class PlayerGroupGUI : MonoBehaviour {
    public List<Character_SO> CharList;


    public Dictionary<CharPeriod,Character_SO> characters;
    public static Dictionary<CharPeriod, Character_SO> SharedCharacterInfo; // to access every information of the characters everywhere

    public Dictionary<CharPeriod, CharacterGUI> charactersGUI;
    public static Dictionary<CharPeriod, CharacterGUI> SharedCharactersGUI;


    private void Start() {

        characters = new Dictionary<CharPeriod, Character_SO>(4);
        for (int i = 0; i < CharList.Count; i++) {
            characters[CharList[i].timePeriod] = CharList[i];
        }
        SharedCharacterInfo = characters;

        charactersGUI = new Dictionary<CharPeriod, CharacterGUI>(4);
        foreach (Transform child in transform) {
            if (child.CompareTag(Tags.GUICharacter)) {
                CharacterGUI temp = child.GetComponent<CharacterGUI>();
                charactersGUI[temp.timePeriod] = temp;
            }

        }

        SharedCharactersGUI = charactersGUI;
    }


}
