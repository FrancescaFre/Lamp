using System.Collections.Generic;
using UnityEngine;
using MEC;

public class EnemySFXEmitter : MonoBehaviour {
    //base.baseClip is the main clip while wandering
    

    public AudioSource source;
    [Range(0f, 200f)]
    public float minAudibleDistance = 1f;
    [Range(0f, 200f)]
    public float maxAudibleDistance = 100f;
    [Range(0f, 1f)]
    public float is2D_or3D = 1f;


    public List<AudioClip> wanderingList;

    public  void Start() {
        source = gameObject.AddComponent<AudioSource>();
        source.volume = AudioManager.Instance.volumeSFX;
         AudioManager.Instance.SFXSourceList.Add(this.source);
        source.minDistance = minAudibleDistance;
        source.maxDistance = maxAudibleDistance;
        source.spatialBlend = is2D_or3D;
        OnWandering();
    }
    // TODO: play different verses 
    public void OnWandering() {
        Timing.RunCoroutine(_wandering(), "wandering");
    }

    IEnumerator<float> _wandering() {
        while (true) {
            
            if (!source.isPlaying) {
                var clip = wanderingList[Random.Range(0, wanderingList.Count)];
                source.Stop();
                source.clip = clip;
                if(source.isActiveAndEnabled)
                    source.Play();
                //yield return Timing.WaitForSeconds(clip.length);
                
            }
            yield return Timing.WaitForOneFrame;
        }
    }

}
