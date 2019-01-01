using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEmitter : MonoBehaviour {
    public AudioSource source;
    [Range(0f,200f)]
    public float maxAudibleDistance = 100f;
    [Range(0f,1f)]
    public float is2D_or3D = 1f;
    public AudioClip clip;

    public void Awake() {
        source = gameObject.AddComponent<AudioSource>();
    }

    // Use this for initialization
    public virtual void Start () {
        
        source.clip = clip;

        //source.volume = AudioManager.Instance.volumeAmbience;
        source.maxDistance = maxAudibleDistance;
        source.loop = true;
        source.spatialBlend = is2D_or3D;
        source.Play();
    }
	

}
