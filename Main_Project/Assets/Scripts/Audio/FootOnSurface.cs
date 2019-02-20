using System.Collections.Generic;
using UnityEngine;

public class FootOnSurface : MonoBehaviour {

    public List<AudioClip> footOnSurfaceList = new List<AudioClip>();

    private void Start() {
        GetComponent<Collider>().isTrigger = true;
    }

    public  void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.Player)) {
            PlayerSFXEmitter foot = other.GetComponentInParent<PlayerSFXEmitter>();
            foot.stepsFXList = footOnSurfaceList;


        }
    }

    public  void OnTriggerStay(Collider other) {
        if (other.CompareTag(Tags.Player)) {
            PlayerSFXEmitter foot = other.GetComponentInParent<PlayerSFXEmitter>();
            foot.stepsFXList = footOnSurfaceList;


        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(Tags.Player)) {
            PlayerSFXEmitter foot = other.GetComponentInParent<PlayerSFXEmitter>();
            foot.stepsFXList = GameManager.Instance.levelLoaded.footStepsSFX;


        }
    }
}
