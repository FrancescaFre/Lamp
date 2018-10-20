using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {

    public float height;    //how high the movement is going to be (it will move up and down from zero to 'height')
    public float time;

	// Use this for initialization
	void Start () {
        //moveBy: moves the gameObject by a defined amount in a defined time with a defined curve 
        iTween.MoveBy(gameObject,iTween.Hash("y",this.height,"time",this.time,"looptype","pingpong","easetype",iTween.EaseType.easeInOutSine)); //check minute 8:15 of the tutorial
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
