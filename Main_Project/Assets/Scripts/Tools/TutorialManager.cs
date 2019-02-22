using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public TutorialStep[] spawnOrder;

    private GameObject previous, actual, next;

    private int index;
    private float originalDeltaTime;
    private bool isActive;

	void Start ()
    {
        index = 0;
        originalDeltaTime = Time.fixedDeltaTime;
        isActive = true;
        StopTime();
        GameManager.Instance.digCount = 1; // Gives a re-chance to the player if he fails a dig
	}
	
	void Update ()
    {
        if (isActive && Time.timeScale != 0f) // Avoid that the player pauses to restart time
            StopTime();

        if (isActive && Input.GetKeyDown(KeyCode.Tab))
            if (!spawnOrder[index].Next())
            { 
                isActive = false;
                spawnOrder[index].AddMenuEntry();
                RestartTime();
                if (index == spawnOrder.Length)
                    gameObject.SetActive(false);
            }
	}

    public void NewStep()
    {
        spawnOrder[index].gameObject.SetActive(false);
        index++;
        isActive = true;
        StopTime();
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
