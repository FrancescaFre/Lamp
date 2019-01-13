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
       
	}
    
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
