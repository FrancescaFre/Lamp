using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerGroupGUI : MonoBehaviour {
    public List<Character_SO> CharList;


    public Dictionary<CharPeriod,Character_SO> characters;
    public static Dictionary<CharPeriod, Character_SO> SharedCharacterInfo; // to access every information of the characters everywhere




    private void Start() {

        characters = new Dictionary<CharPeriod, Character_SO>();
        for (int i = 0; i < CharList.Count; i++) {
            characters[CharList[i].timePeriod] = CharList[i];
        }
        SharedCharacterInfo = characters;
        
    }


}
