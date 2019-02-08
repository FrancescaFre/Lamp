using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXEmitter : MonoBehaviour {
    private AudioSource _source;
    public List<AudioClip> digAudioList;
    public List<AudioClip> stepsFXList;
    [Range(0f,1f)]
    public float sneakAudioReduction = .6f;
    [Range(1f,1.5f)]
    public float runAudioIncrease = 1.2f;

    // Use this for initialization
    void Start() {
        _source = gameObject.AddComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.volume = AudioManager.Instance.volumeSFX;
        stepsFXList = GameManager.Instance.levelLoaded.footStepsSFX;
        AudioManager.Instance.SFXSourceList.Add(this._source);
    }


    public void DigEffect() {
        _source.volume = AudioManager.Instance.volumeSFX;
        _playSFX(digAudioList);
    }

    #region FootSteps
    public void Step() {
        _source.volume = AudioManager.Instance.volumeSFX;
        _playSFX(stepsFXList);
    }
    public void SneakStep() {
        _source.volume = AudioManager.Instance.volumeSFX * sneakAudioReduction;
        _playSFX(stepsFXList);
    }
    public void RunStep() {
        _source.volume = AudioManager.Instance.volumeSFX * runAudioIncrease;
        _playSFX(stepsFXList);
    }


    #endregion

    private void _playSFX(List<AudioClip> SFXList) {
        AudioClip clip = SFXList[Random.Range(0, SFXList.Count)];
        _source.PlayOneShot(clip);
    }
}

