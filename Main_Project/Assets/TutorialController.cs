using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    int tutorialPage = 1;
    List<Transform> cameraTrace;
    Vector3 destination;
    int nextDest = 0; 

    public List<GameObject> checkPoints; 

	// Use this for initialization
	void Start () {
	    //instantiate of all prefab 	
	}
	
	// Update is called once per frame
	void Update () {

        switch (tutorialPage) {
            case 1: Step1(); break; 
        }


        //movment of the camera
        if (cameraTrace.Count > 0 && destination != Vector3.zero) {
            if (Vector3.Distance(destination, transform.position) < 0.3f)
            {
                cameraTrace.RemoveAt(0);
                destination = cameraTrace[0].position;
            }
        }
        if (cameraTrace.Count == 0) {
            nextDest=0;
            destination = Vector3.zero;
            BasicCamera.instance.ChangeTarget(GameManager.Instance.currentPC.transform);
        }



	}

    void Step1() {
        foreach (Transform child in checkPoints[tutorialPage].transform)
            cameraTrace.Add(child);
        destination = cameraTrace[nextDest].position;
        BasicCamera.instance.ChangeTarget(cameraTrace[nextDest]);

    }
}
