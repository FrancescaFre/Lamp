using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    int tutorialPage = 0;
    List<Transform> cameraTrace = new List<Transform>();
    Vector3 destination = Vector3.zero;
    int nextDest = 0;
    float progress = 0f; 

    private float time_to_wait = 3f; 
    public List<GameObject> checkPoints; 

	// Use this for initialization
	void Start () {
	    //instantiate of all prefab 	
	}
	
	// Update is called once per frame
	void Update () {

        switch (tutorialPage) {
            case 0: Step0(); break; 
        }

        //---- skip
        if (Input.GetKeyDown(KeyCode.P))
        {
            cameraTrace.Clear();
        }

        progress += Time.deltaTime;
        if (progress >= time_to_wait && cameraTrace.Count > 0) {
            progress = 0;
            
            BasicCamera.instance.ChangeTarget(cameraTrace[0]);
            cameraTrace.RemoveAt(0);
        }

        if (progress >= time_to_wait && cameraTrace.Count == 0) {
            nextDest=0;
            BasicCamera.instance.ChangeTarget(GameManager.Instance.currentPC.transform);
        }
	}

    void Step0() {
        foreach (Transform child in checkPoints[tutorialPage].transform)
            cameraTrace.Add(child);

        BasicCamera.instance.ChangeTarget(cameraTrace[0]);
        cameraTrace.RemoveAt(0);

        tutorialPage++; 
    }
}
