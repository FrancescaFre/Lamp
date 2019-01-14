using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggerWhenYourSpecialAndYouFallFromTheFirstPlatform : TutorialTrigger {

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            tm.NewStep();
            if (nextToSpawn)
                nextToSpawn.gameObject.SetActive(true);
            Destroy(this);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        ;
    }
}
