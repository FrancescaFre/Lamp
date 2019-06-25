using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    int tutorialPage = 0;
    List<Transform> cameraTrace = new List<Transform>();
    Vector3 destination = Vector3.zero;
    float progress = 0f;
    private bool fill_camera = false;
    private float time_to_wait = 3f;
    private bool disableVerticalDig = true;
    private bool disableZoneDig = true;

    public List<GameObject> checkPoints;
    
    //to setActive
    public GameObject key;
    public GameObject drill_1;
    public GameObject drill_2;
    public GameObject missing; 

	// Use this for initialization
	private void Start () {
	    //instantiate of all prefab 	
	}
	
	// Update is called once per frame
	private void Update () {

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
            
            BasicCamera.instance.ChangeTarget(GameManager.Instance.currentPC.transform);
        }

        CheckEvent(); 
    }

    void FillCameraTrace() {
        foreach (Transform child in checkPoints[tutorialPage].transform)
            cameraTrace.Add(child);

        BasicCamera.instance.ChangeTarget(cameraTrace[0]);
        cameraTrace.RemoveAt(0);

     
    }

    

    private void CheckEvent()
    {
        switch (tutorialPage)
        {
            case 0: step0(); break;

            case 1: step1(); break;

            case 2: 
                if(GameManager.Instance.allyLamps == 1)
                    step2(); break;

            case 3:
                if(GameManager.Instance.allyLamps == 2)
                    step3(); break;

            case 4:
                if (GameManager.Instance.allyLamps == 3)
                    step4(); break;

            case 5:
                if (GameManager.Instance.digCount > 0)
                    step5(); break;

            case 9:
                if(GameManager.Instance.enemyLamps == 0)
                    step9(); break;

            default: break;
        }

    }

    private void step0()
    {
        
        if (!fill_camera)
        {
            StopPlayerMovement(true);
            FillCameraTrace();
            fill_camera = true;

            //print: complete the level...
        }

        if (cameraTrace.Count == 4)
        {
            //print: these are the light to follow..
        }

        if (cameraTrace.Count == 0)
        {
            //end
            StopPlayerMovement(false);
            fill_camera = false;
            tutorialPage++; 
        }

    }

    private void step1()
    {
        //print movement tutorial
        tutorialPage++;
    }

    private void step2()
    {
        //print compass tutorial
        tutorialPage++;
    }

    private void step3()
    {
        if (!fill_camera)
        {
            StopPlayerMovement(true);
            key.SetActive(true);

            FillCameraTrace();
            fill_camera = true;
        }

        if (cameraTrace.Count == 0)
        {
            StopPlayerMovement(false);
            tutorialPage++;
            fill_camera = false;
        }
    }

    private void step4()
    {
        if (!fill_camera)
        {
            drill_1.SetActive(true);
            FillCameraTrace();
            fill_camera = true;
        }

        if (cameraTrace.Count == 0)
        {
            fill_camera = false;
            tutorialPage++;
        }
    }

    private void step5()
    {
        if (!fill_camera)
        {
            //tutorial zone dig
            disableZoneDig = false;
            FillCameraTrace();
            fill_camera = true; 
        }

        if (cameraTrace.Count == 0)
        {
            fill_camera = false;
            tutorialPage++;
        }
    }

    private void step6()
    {
        if (!fill_camera)
        {
            StopPlayerMovement(true);
            //tutorial approach to the enemies
            FillCameraTrace();
            fill_camera = true;
        }

        if (cameraTrace.Count == 0)
        {
            StopPlayerMovement(false);
            fill_camera = false;
            tutorialPage++; 
        }

    }

    private void step7()
    {
        //repeat drill tutorial 
        tutorialPage++;
    }

    private void step8()
    {
        if (!fill_camera)
        {
            StopPlayerMovement(true);
            //these lamp have some problem
            FillCameraTrace();
            fill_camera = true;
        }

        if (cameraTrace.Count == 1)
        {
            //this cursed lamp...
        }

        if (cameraTrace.Count == 0)
        {
            StopPlayerMovement(false);
            fill_camera = false;
            tutorialPage++;
        }
    }

    private void step9()
    {
        if (!fill_camera)
        {
            drill_2.SetActive(true);
            StopPlayerMovement(true);
            //reach these 4 candles
            FillCameraTrace();
            fill_camera = true;
        }

        if (cameraTrace.Count == 0)
        {
            StopPlayerMovement(false);
            fill_camera = false;
            tutorialPage++;
        }
    }

    private void step10()
    {
        disableVerticalDig = false; 
        //tutorial vertical dig
        tutorialPage++;

    }

    private void step11()
    {
        if (!fill_camera)
        {
            missing.SetActive(true);
            StopPlayerMovement(true);
            //these lamp have some problem...
            FillCameraTrace();
            fill_camera = true;
        }

        if (cameraTrace.Count == 1)
        {
            //piece...
        }

        if (cameraTrace.Count == 0)
        {
            StopPlayerMovement(false);
            fill_camera = false;
            tutorialPage++;
        }
    }

    private void StopPlayerMovement(bool mov)
    {
        GameManager.Instance.currentPC.GetComponent<PlayerMovement>().OnIce = mov;
    }

    public void Notify(int n)
    {
        switch (n)
        {
            case 06: if (tutorialPage == 6) step6(); break;
            case 7: if (tutorialPage == 7) step7(); break;
            case 8: if (tutorialPage == 8) step8(); break;
            case 10: if (tutorialPage == 10) step10(); break;
            case 11: if (tutorialPage == 11) step11(); break;
        }
    }
}
