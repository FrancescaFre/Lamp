using UnityEngine;

public class ItemSpawner : TutorialTrigger {

    // Spawns the item at touch
    
    public GameObject item;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        item.SetActive(true);
    }
}
