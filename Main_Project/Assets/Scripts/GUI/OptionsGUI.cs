using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsGUI : MonoBehaviour {

    public Slider cameraSpeed;
    [Header("Volume Settings")]
    public Slider musicVolume;
    public Slider SFXVolume;
    public Slider ambienceVolume;

    private CameraManager _camManager;
	// Use this for initialization
	void Start () {
        _camManager = Camera.main.GetComponent<CameraManager>();

        cameraSpeed.value = _camManager.cameraSpeed;

        musicVolume.value = AudioManager.Instance.musicSource.volume;
        ambienceVolume.value = AudioManager.Instance.ambienceSource.volume;

        gameObject.SetActive(false);
	}
	

    
    public void OnUpdateCameraSpeed() {
        _camManager.cameraSpeed = cameraSpeed.value;
    }

    public void OnUpdateMusicVolume() {
        AudioManager.Instance.musicSource.volume = musicVolume.value;
    }
    public void OnUpdateAmbienceVolume() {
        AudioManager.Instance.ambienceSource.volume = ambienceVolume.value;
    }
    public void OnUpdateGenericSFXVolume() {
        AudioManager.Instance.SFXSource.volume = SFXVolume.value;
    }
    
}
