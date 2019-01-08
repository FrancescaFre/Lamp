using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TeamFormationGUI : MonoBehaviour {
    [Header("Team Information")]
    public List<CharPeriod> teamList;
    public int MAX_TEAM_NUMBER = 3;

    [Header("Team profile picture")]
    public Image First;
    public Image Second;
    public Image Third;

    [Header("Team stage light")]
    public Light PrimitiveLight;
    public Light OrientalLight;
    public Light VictorianLight;
    public Light FutureLight;
    [Header("Stage Light Colours")]
    public Color FirstLight;
    public Color secondLight;
    public Color thirdLight;

    public Dictionary<CharPeriod, Light> LightsDict;



    private void Start() {
        teamList = new List<CharPeriod>(3);
        LightsDict = new Dictionary<CharPeriod, Light>();
       

        LightsDict[CharPeriod.PREHISTORY] = PrimitiveLight;
        LightsDict[CharPeriod.ORIENTAL] = OrientalLight;
        LightsDict[CharPeriod.VICTORIAN] = VictorianLight;
        LightsDict[CharPeriod.FUTURE] = FutureLight;

    }

    private void FixedUpdate() {

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
            GuiManager.GUIInstance.PlayButton.interactable = true;

        }
        else {
            Third.sprite = null;
            Third.gameObject.SetActive(false);
            GuiManager.GUIInstance.PlayButton.interactable = false;
            

        }
       
    }

    /// <summary>
    /// Adds the character to the team(if already present it will be removed)
    /// </summary>
    /// <param name="period">the time period of the character to be added/removed</param>
    public void SetCharacter(CharPeriod period) {
        if (teamList.Contains(period)) {
            teamList.RemoveAll(x => x.Equals(period));
            LightsDict[period].gameObject.SetActive(false);
        }
        else {
            if (teamList.Count < MAX_TEAM_NUMBER) {
                teamList.Add(period);
               
                LightsDict[period].gameObject.SetActive(true);
                if (teamList.Count == 1)
                    LightsDict[period].color = FirstLight;
                else if (teamList.Count == 2)
                    LightsDict[period].color = secondLight;
                else if (teamList.Count == 3)
                    LightsDict[period].color = thirdLight;
            }


        }

        if (teamList.Count == 3)
            GameManager.Instance.TeamList = teamList;
        else
            GameManager.Instance.TeamList = null;

    }

}

