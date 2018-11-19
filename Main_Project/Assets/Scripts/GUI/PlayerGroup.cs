using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerGroup : MonoBehaviour {
    public List<Character_SO> CharList;
    public static Character_SO pointedChar;
    public Dictionary<CharPeriod,Character_SO> characters;

    private void Start() {
        characters = new Dictionary<CharPeriod, Character_SO>();
        for (int i = 0; i < CharList.Count; i++) {
            characters[CharList[i].timePeriod] = CharList[i];
        }
    }


    public void GetPrimitive() {

        pointedChar = characters[CharPeriod.PREHISTORY];
    }
    public void GetOriental() {

        pointedChar = characters[CharPeriod.ORIENTAL];
    }
    public void GetVictorian() {

        pointedChar = characters[CharPeriod.VICTORIAN];
    }
    public void GetFuture() {

        pointedChar = characters[CharPeriod.FUTURE];
    }

    
}
