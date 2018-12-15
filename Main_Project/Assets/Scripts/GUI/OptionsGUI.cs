using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsGUI : MonoBehaviour {

    public Slider cameraSpeed;
    private CameraManager _camManager;
	// Use this for initialization
	void Start () {
        _camManager = Camera.main.GetComponent<CameraManager>();
        cameraSpeed = GetComponentInChildren<Slider>();
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void OnUpdateCameraSpeed() {
        _camManager.cameraSpeed = cameraSpeed.value;
    }
}
