using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public TutorialStep[] spawnOrder;

    private GameObject previous, actual, next;

    private int index;
    private float originalDeltaTime;
    //private bool isZoneActive;

	void Start ()
    {
        index = 0;
        originalDeltaTime = Time.fixedDeltaTime;
        //isZoneActive = true;
        //GameManager.Instance.currentPC.isZoneDigging = true;
        InGameHUD.Instance.tabTutorial.gameObject.SetActive(true);
        //StopTime();
        GameManager.Instance.digCount = 1; // Gives a re-chance to the player if he fails a dig
	}
	
	void Update ()
    {
        //if (isZoneActive && Time.timeScale != 0f) // Avoid that the player pauses to restart time
        //if (GameManager.Instance.currentPC.IsZoneDigging && Time.timeScale != 0f) // Avoid that the player pauses to restart time
            //StopTime();
        //if (isZoneActive && Input.GetKeyDown(KeyCode.Tab))
       // if (GameManager.Instance.currentPC.isZoneDigging && Input.GetKeyDown(KeyCode.Tab))
            if (!spawnOrder[index].Next())
            {
                //isZoneActive = false;
              //  GameManager.Instance.currentPC.isZoneDigging = false;
                InGameHUD.Instance.tabTutorial.gameObject.SetActive(false);
                spawnOrder[index].AddMenuEntry();
                //RestartTime();
                if (index == spawnOrder.Length)
                    gameObject.SetActive(false);
            }
	}

    public void NewStep()
    {
        spawnOrder[index].gameObject.SetActive(false);
        index++;
        //isZoneActive = true;
        //GameManager.Instance.currentPC.isZoneDigging = true;
        InGameHUD.Instance.tabTutorial.gameObject.SetActive(true);
        //StopTime();
        spawnOrder[index].gameObject.SetActive(true);
    }

    private void StopTime()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = .2f * Time.timeScale;
    }

    private void RestartTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalDeltaTime;
    }
}
