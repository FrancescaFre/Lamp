using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TeamFormationGUI : MonoBehaviour, ICancelHandler {
    public List<CharPeriod> teamList;
    public int MAX_TEAM_NUMBER = 3;


    public Image First;
    public Image Second;
    public Image Third;

    public Light PrimitiveLight;
    public Light OrientalLight;
    public Light VictorianLight;
    public Light FutureLight;
    public Dictionary<CharPeriod, Light> LightsDict;

    private Button startButton;

    private void Start() {
        teamList = new List<CharPeriod>(3);
        LightsDict = new Dictionary<CharPeriod, Light>();
        startButton = GetComponentInParent<GUIManager>().StartButton;

        LightsDict[CharPeriod.PREHISTORY] = PrimitiveLight;
        LightsDict[CharPeriod.ORIENTAL] = OrientalLight;
        LightsDict[CharPeriod.VICTORIAN] = VictorianLight;
        LightsDict[CharPeriod.FUTURE] = FutureLight;

    }

    private void FixedUpdate() {
        Debug.Log("THERE ARE "+teamList.Count);
        if (teamList.Count>0 && teamList[0] >= 0) {
            First.gameObject.SetActive(true);
            First.sprite = PlayerGroupGUI.SharedCharacterInfo[teamList[0]].CharSprite;
        }
        else {
            First.sprite = null;
            First.gameObject.SetActive(false);
        }

        if (teamList.Count > 1 && teamList[1] >= 0) {
            Second.gameObject.SetActive(true);
            Second.sprite = PlayerGroupGUI.SharedCharacterInfo[teamList[1]].CharSprite;
        }
        else {
            Second.sprite = null;
            Second.gameObject.SetActive(false);
        }

        if (teamList.Count > 2 && teamList[2] >= 0) {
            Third.gameObject.SetActive(true);
            Third.sprite = PlayerGroupGUI.SharedCharacterInfo[teamList[2]].CharSprite;
            startButton.interactable = true;
        }
        else {
            Third.sprite = null;
            Third.gameObject.SetActive(false);
            startButton.interactable = false;
        }
       
    }

    public void OnCancel(BaseEventData eventData) {
        Debug.Log("CANCEL");
        if (teamList.Count > 0) {
            teamList.Clear();
            //TODO clear team in GAMEMANAGER
            Debug.Log("CANCEL TEAM");
        }

    }


    public void AddCharacter(CharPeriod temp) {
        if (teamList.Contains(temp)) {
            teamList.RemoveAll(x => x.Equals(temp));
            LightsDict[temp].gameObject.SetActive(false);
        }
        else {
            if (teamList.Count < MAX_TEAM_NUMBER) {
                teamList.Add(temp);
               
                LightsDict[temp].gameObject.SetActive(true);
            }


        }

    }

}

