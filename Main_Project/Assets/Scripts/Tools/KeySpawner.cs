using UnityEngine;

public class KeySpawner : TutorialTrigger {

    // Because of the trigger-step-manager structure of the tutorial, i needed
    // to be more flexible with some scripts, so they extend TutorialTrigger
    
    public GameObject key;
    public int numberOfLampsToTurn;

    private void Update()
    {
        if (GameManager.Instance.allyLamps == numberOfLampsToTurn)
        {
            GetComponent<AudioSource>().Play();
            key.SetActive(true);

            tm.NewStep();
            if (nextToSpawn)
                nextToSpawn.gameObject.SetActive(true);

            // I really need to destroy the other one, or it's update will be fired too
            foreach (KeySpawner ks in FindObjectsOfType<KeySpawner>())
                Destroy(ks); 
        }
    }
}
