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
    private void Start() {
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

    private void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.F1)) {//Reset level
            levelInfo.Reset();
            foreach (Light l in lights)
                l.gameObject.SetActive(levelInfo.isCompleted);

            fog.gameObject.SetActive(!levelInfo.isCompleted);
            fireflies.Play();
        }
        if (Input.GetKeyDown(KeyCode.F2)) {//free level
            levelInfo.SetFree();
            foreach (Light l in lights)
                l.gameObject.SetActive(levelInfo.isCompleted);

            fog.gameObject.SetActive(!levelInfo.isCompleted);
            fireflies.Stop();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.Player_Ship)) {
            GameManager.Instance.levelLoaded = this.levelInfo;
            NewGuiManager.instance.ShowLevelInfo();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(Tags.Player_Ship)) {
            GameManager.Instance.levelLoaded = null;
            NewGuiManager.instance.HideLevelInfo();
        }
    }
}