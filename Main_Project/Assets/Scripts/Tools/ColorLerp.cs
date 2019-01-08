using System.Collections.Generic;
using UnityEngine;
using MEC;

public class ColorLerp  {
    private Light lightSource;
    public Color startColor;
    public Color endColor;
    public float fadeTime;

    public ColorLerp(Light lit, Color startColor, Color endColor,float fadeTime) {
        lightSource = lit;
        this.startColor = startColor;
        this.endColor = endColor;
        this.fadeTime = fadeTime;
    }

    public void LerpColors() {
        if (!lightSource) return;
        Timing.RunCoroutine(_LerpAtoB(),"Lerp");
    }

    private IEnumerator<float> _LerpAtoB() {
        //Timing.KillCoroutines("ReverseLerp");
        for (float t = 0.01f; t < fadeTime; t+=0.1f) {
            lightSource.color = Color.Lerp(startColor,endColor,t/fadeTime);
            yield return Timing.WaitForOneFrame;
        }
    }

    public void ReverseLerpColors() {
        if (!lightSource) return;
        Timing.RunCoroutine(_LerpBtoA(),"ReverseLerp");
    }

    private IEnumerator<float> _LerpBtoA() {
        //Timing.KillCoroutines("Lerp");
        for (float t = 0.01f; t < fadeTime; t+=0.1f) {
            lightSource.color = Color.Lerp(endColor, startColor, t/fadeTime);
            yield return Timing.WaitForOneFrame;
        }
    }


}
