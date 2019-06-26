using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigSpawner : MonoBehaviour {

    float progress;
    float time = 10f;

    bool exist;
    Transform drill; 

	// Use this for initialization
	void Start () {
        exist = true; 
        drill = this.gameObject.transform.GetChild(0);
        drill.gameObject.SetActive(true);
	}

    // Update is called once per frame
    void Update() {
        if (!exist) {
            progress += Time.deltaTime;
            if (progress > time)
            {
                exist = true;
                drill.gameObject.SetActive(true);
                progress = 0; 
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag(Tags.Player)) return;
        
        if (exist) {
            

            exist = false;
            drill.gameObject.SetActive(false);
            progress = 0;
        }
    }
}
