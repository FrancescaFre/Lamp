using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour {
    private AudioSource _source;
    public List<AudioClip> stepsFX;
    [Range(0f,1f)]
    public float sneakAudioReduction = .6f;
    // Use this for initialization
    void Start() {
        _source = gameObject.AddComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.volume = AudioManager.Instance.volumeSFX;
        stepsFX = GameManager.Instance.levelLoaded.stepSFX;
        AudioManager.Instance.SFXSourceList.Add(this._source);
    }

    public void Step() {
        _source.volume = AudioManager.Instance.volumeSFX;
        _Play();
    }
    public void SneakStep() {
        _source.volume = AudioManager.Instance.volumeSFX * sneakAudioReduction;
        _Play();
    }

    private void _Play() {
        AudioClip clip = stepsFX[Random.Range(0, stepsFX.Count)];
        _source.PlayOneShot(clip);
    }
}

