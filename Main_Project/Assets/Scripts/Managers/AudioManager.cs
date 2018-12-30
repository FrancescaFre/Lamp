using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;
    public AudioSource musicSource;
    public AudioSource SFXSource;
    public AudioSource ambienceSource;
    [Range(0f,1f)]
    public float volumeMusic=.8f;
    [Range(0f, 1f)]
    public float volumeSFX=.5f;
    [Range(0f, 1f)]
    public float volumeAmbience=.5f;
    public AudioClip MenuClip;
    [Range(0f, 1f)]
    public float speed = 0f;


    private void Awake() {
        if (!Instance) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = MenuClip;
        musicSource.loop = true;
        //StartCoroutine(FadeIn());
        musicSource.volume = volumeMusic;
        musicSource.Play();
        
        
    }

    /// <summary>
    /// Plays the level music
    /// </summary>
    /// <param name="musicList">Music to play (default null plays menu music)</param>
    public void PlayMusic(List<AudioClip> musicList = null) {
        //StartCoroutine(FadeOut());
        musicSource.Stop();
        if (musicList==null) {
            musicSource.clip = MenuClip;
            musicSource.Play();
           // StartCoroutine(FadeIn());
        }
        else {
            int i = Random.Range(0,musicList.Count);


            musicSource.clip = musicList[i];
            musicSource.Play();
           // StartCoroutine(FadeIn());
        }

    }
    /*bool fadeIn;
    bool fadeOut;

    IEnumerator FadeIn() {
        fadeIn = true;
        fadeOut = false;

        Source.volume = 0;
        float audioVolume = 0;

        while (Source.volume<maxVolume && fadeIn) {

            audioVolume += speed;
            Source.volume = audioVolume;
            yield return new WaitForSeconds(.1f);
        }
        

    }
    IEnumerator FadeOut( ) {
        fadeIn = true;
        fadeOut = false;

        
        float audioVolume = Source.volume;

        while (Source.volume>=speed && fadeIn) {

            audioVolume -= speed*2;
            Source.volume = audioVolume;
            yield return new WaitForSeconds(.1f);
        }
        

    }
    */

    public void OnStartGame() {
        SFXSource = gameObject.AddComponent<AudioSource>();
        SFXSource.playOnAwake = false;
        SFXSource.volume = volumeSFX;

        ambienceSource = gameObject.AddComponent<AudioSource>();
        ambienceSource.clip = GameManager.Instance.levelLoaded.ambienceSFX;
        ambienceSource.loop = true;
        ambienceSource.volume = volumeAmbience;
        ambienceSource.Play();
        
    }

    public void OnEndGame() {

        Destroy(SFXSource);
        Destroy(ambienceSource);
    }
}
