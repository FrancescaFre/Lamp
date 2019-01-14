
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    public TutorialTrigger nextToSpawn;
    protected TutorialManager tm;

    private void Start()
    {
        tm = FindObjectOfType<TutorialManager>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            tm.NewStep();
            if (nextToSpawn)
                nextToSpawn.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
