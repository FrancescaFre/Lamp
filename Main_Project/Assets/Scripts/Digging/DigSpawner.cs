using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigSpawner : MonoBehaviour {

    float progress;
    float time = 10f;

    bool take;
    Transform drill; 

	// Use this for initialization
	void Start () {
        drill = this.gameObject.transform.GetChild(0);
	}

    // Update is called once per frame
    void Update() {
        if (take) {
            progress += Time.deltaTime;
            if (progress > time)
            {
                take = false;
                transform.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (take) return;

        take = true; 
        transform.gameObject.SetActive(false);
        progress = 0; 
    }
}
