using System.Collections.Generic;
using UnityEngine;

public class FootOnSurface : MonoBehaviour {

    public List<AudioClip> footOnSurfaceList = new List<AudioClip>();


    public  void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerSFXEmitter foot = other.GetComponentInParent<PlayerSFXEmitter>();
            foot.stepsFXList = footOnSurfaceList;


        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerSFXEmitter foot = other.GetComponentInParent<PlayerSFXEmitter>();
            foot.stepsFXList = GameManager.Instance.levelLoaded.footStepsSFX;


        }
    }
}
