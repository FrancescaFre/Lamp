using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    public GameManager gm;
    private void Start() {
        gm = GameManager.Instance;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.F12)) {
             if (gm.LastAllyLamp)

                GameManager.CharactersDict[gm.TeamList[gm.currentCharacter]].transform.position = gm.LastAllyLamp.transform.position + GameManager.CharactersDict[gm.TeamList[gm.currentCharacter]].transform.forward;
             else
                 GameManager.CharactersDict[gm.TeamList[gm.currentCharacter]].transform.position = gm.levelLoaded.entryPoint;

           
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            FindObjectOfType<SpinFree>().spinParent = !FindObjectOfType<SpinFree>().spinParent;

        }
    }
}
