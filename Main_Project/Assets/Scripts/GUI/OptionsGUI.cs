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

        musicVolume.value = AudioManager.Instance.volumeMusic;
        SFXVolume.value = AudioManager.Instance.volumeSFX;
        ambienceVolume.value = AudioManager.Instance.volumeAmbience;


        gameObject.SetActive(false);
	}
	

    
    public void OnUpdateCameraSpeed() {
        _camManager.cameraSpeed = cameraSpeed.value;
    }

    public void OnUpdateMusicVolume() {
        AudioManager.Instance.musicSource.volume = musicVolume.value;
    }

    public void OnUpdateAmbienceVolume() {
     

        foreach (var s in AudioManager.Instance.ambienceSourceList)
            s.volume = ambienceVolume.value;
    }

    public void OnUpdateGenericSFXVolume() {
        AudioManager.Instance.SFXSource.volume = SFXVolume.value;

        foreach (var s in AudioManager.Instance.SFXSourceList)
            s.volume = SFXVolume.value;
    }
    
}
