using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour {
    private AudioSource _source;
    public List<AudioClip> stepsFX;
	// Use this for initialization
	void Start () {
        _source = gameObject.AddComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.volume = 0.7f;
        stepsFX = GameManager.Instance.levelLoaded.stepSFX;
	}
	
    public void Step() {
        _source.volume = 0.7f;
        AudioClip clip = stepsFX[Random.Range(0, stepsFX.Count)];
        _source.PlayOneShot(clip);
    }

    public void SneakStep() {
        _source.volume = .5f;
        AudioClip clip = stepsFX[Random.Range(0, stepsFX.Count)];
        _source.PlayOneShot(clip);
    }
}
