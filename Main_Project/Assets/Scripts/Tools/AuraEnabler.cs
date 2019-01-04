using UnityEngine;
using AuraAPI;

public class AuraEnabler : MonoBehaviour {

    private void Start() {
       var x= GetComponent<AuraLight>();
        var y=GetComponent<AuraVolume>();
        if (x) x.enabled = true;
        if (y) y.enabled = true;

    }
}
