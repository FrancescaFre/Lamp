using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXEmitter : AudioEmitter {

    public override void Start() {
        base.Start();

        source.volume = AudioManager.Instance.volumeSFX;
        AudioManager.Instance.SFXSourceList.Add(this.source);
    }
	

}
