using UnityEngine;

public class DrillSpawner : TutorialTrigger {

    // Easier keyspawner without sounds et cetera
    
    public GameObject drill;
    public int numberOfLampsToTurn;

    private void Update()
    {
        if (GameManager.Instance.allyLamps == numberOfLampsToTurn)
        {
            drill.SetActive(true);

            tm.NewStep();
            if (nextToSpawn)
                nextToSpawn.gameObject.SetActive(true);

            Destroy(this);
        }
    }
}
