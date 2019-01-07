using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {
    public Light[] allLights;
    public Color endColor;
    public List<ColorLerp> lerpers;
    public float fadeTime;


	
	void Start () {
        allLights = GetComponentsInChildren<Light>();
        lerpers = new List<ColorLerp>(allLights.Length);


        for (int i = 0; i < allLights.Length; i++) {
            lerpers.Add(new ColorLerp(allLights[i], allLights[i].color, endColor,fadeTime));
        }
        //eOwner = GetComponent<Enemy>();
	}
    /*
    private void FixedUpdate() {
        if (eOwner)
            if ((eOwner.currentStatus == EnemyStatus.SEARCHING || eOwner.currentStatus == EnemyStatus.SEEKING) && Lock) {
                if (previusState == EnemyStatus.SEARCHING) return;        // if the enemy sees the player again while in seeking
                Debug.Log("LERP COLOR "+eOwner.currentStatus);
                this.Lerp();
                Lock = false;
                previusState = eOwner.currentStatus;
            }
            else if (eOwner.currentStatus == EnemyStatus.RETURN && Lock) {
                Debug.Log("REVERSE LERP COLOR " + eOwner.currentStatus);
                this.ReverseLerp();
                Lock = false;
                previusState = eOwner.currentStatus;
            }
            else if ((eOwner.currentStatus == EnemyStatus.WANDERING || eOwner.currentStatus == EnemyStatus.SEARCHING) && !Lock) {
                Debug.Log("RELEASE LOCK" + eOwner.currentStatus);
                Lock = true;
                previusState = eOwner.currentStatus;
            }
    }
    */
    public void Lerp() {
        for (int i = 0; i < lerpers.Count; i++) {
            lerpers[i].LerpColors();
        }
    }

    public void ReverseLerp() {
        for (int i = 0; i < lerpers.Count; i++) {
            lerpers[i].ReverseLerpColors();
        }
    }




}
