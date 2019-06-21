using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBehaviour : MonoBehaviour {

    public Level_SO levelInfo;
    [SerializeField]
    private List<Light> lights;
    [SerializeField]
    private ParticleSystem fireflies;
    [SerializeField]
    private Light fog;
    

	// Use this for initialization
	void Start () {
        lights = new List<Light>();

        foreach (Light l in GetComponentsInChildren<Light>()) {
            if (l.transform.parent.CompareTag(Tags.Lamp_Switch) || l.transform.parent.CompareTag(Tags.Lamp_Base)) {
                lights.Add(l);
                l.gameObject.SetActive(levelInfo.isCompleted);
            }
            else {
                fog = l;
                fog.gameObject.SetActive(!levelInfo.isCompleted);
            }
        }     
        fireflies = GetComponentInChildren<ParticleSystem>();

        fireflies.gameObject.SetActive(!levelInfo.isCompleted);
    }

    private void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {     
        if (other.CompareTag(Tags.Player_Ship)){
            GameManager.Instance.levelLoaded = this.levelInfo;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(Tags.Player_Ship)) {
            GameManager.Instance.levelLoaded = null;
        }
    }
}
