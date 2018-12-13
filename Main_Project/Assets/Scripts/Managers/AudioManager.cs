using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;
    public AudioSource Source { get { return GetComponent<AudioSource>(); } }

    public AudioClip MenuClip;
    [Range(0f, 1f)]
    public float speed = 0f;
    [Range(0f, 1f)]
    public float maxVolume = 0f;

    private void Awake() {
        if (!Instance) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }

        Source.clip = MenuClip;
        Source.loop = true;
        //StartCoroutine(FadeIn());
        Source.volume = .8f;
        Source.Play();
        
        
    }

    /// <summary>
    /// Plays the level music
    /// </summary>
    /// <param name="audio">Music to play (default null plays menu music)</param>
    public void PlayAudio(AudioClip audio = null) {
        //StartCoroutine(FadeOut());
        Source.Stop();
        if (!audio) {
            Source.clip = MenuClip;
            Source.Play();
           // StartCoroutine(FadeIn());
        }
        else {
            Source.clip = audio;
            Source.Play();
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
}
