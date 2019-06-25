using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialController : MonoBehaviour{
    public GameObject TutorialPanel;

    int tutorialPage = 0;
    List<Transform> cameraTrace = new List<Transform>();
    Vector3 destination = Vector3.zero;
    float progress = 0f;
    private bool fill_camera = false;
    private float time_to_wait = 3f;
    private bool disableVerticalDig = true;
    private bool disableZoneDig = true;

    [Header("Tutorial texts")]
    public Tutorial_SO tutorialTexts;

    public TextMeshProUGUI GeneralTextField;
    public GameObject CompassField;
    public GameObject SpaceBar;

    [Header("Tutorial checkpoints")]
    public List<GameObject> checkPoints;

    //to setActive
    [Header("Tutorial items")]
    public GameObject key;
    public GameObject drill_1;
    public GameObject drill_2;
    public GameObject missing;

    private bool stopCamera = false;
    private bool nextText = false;

    private void Start() {
        CompassField.SetActive(false);
    }

    // Update is called once per frame
    private void Update () {
         CheckEvent();
         SpaceBar.SetActive(!nextText);

         if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown(Controllers.PS4_Button_X)){
             stopCamera = false;
             nextText = true;
         }

         if(!stopCamera)
            MoveCamera();

    }

    private void MoveCamera() {  //---- skip
        if (Input.GetKeyDown(KeyCode.P)) {
            cameraTrace.Clear();
        }

        progress += Time.deltaTime;
        if (progress >= time_to_wait && cameraTrace.Count > 0) {
            progress = 0;

            BasicCamera.instance.ChangeTarget(cameraTrace[0]);
            cameraTrace.RemoveAt(0);
        }

        if (progress >= time_to_wait && cameraTrace.Count == 0){
            progress = 0;
            BasicCamera.instance.ChangeTarget(GameManager.Instance.currentPC.transform);
        }
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
                if (GameManager.Instance.allyLamps == 1)
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
        }

    }

    public void Notify(int n) {
        switch (n) {
            case 1:
            case 2: nextText = true;
                break;
            case 6: if (tutorialPage == 6) step6(); break;
            case 7: if (tutorialPage == 7) step7(); break;
            case 8: if (tutorialPage == 8) step8(); break;
            case 10: if (tutorialPage == 10) step10(); break;
            case 11: if (tutorialPage == 11) step11(); break;
        }
    }

   private void PanelON(){
        TutorialPanel.SetActive(true);
    }
    private void PanelOFF(){
        nextText = false;
        GeneralTextField.text = "";
        TutorialPanel.SetActive(false);
    }

    //-----------------STEPS
    private void step0()
    {
        
        if (!fill_camera)
        {
            StopPlayerMovement(true);
            FillCameraTrace();
            fill_camera = true;

            PanelON();
            GeneralTextField.text = tutorialTexts.step0_1;
            

            //print: complete the level...
        }

        if (cameraTrace.Count == 2)
        {
            stopCamera = true;
            
            if (nextText) {
                GeneralTextField.text = tutorialTexts.step0_2;
                stopCamera = false;
               
            }

            //print: these are the light to follow..
        }

        if (cameraTrace.Count == 0)
        {
            //end
            StopPlayerMovement(false);
            fill_camera = false;
            tutorialPage++;
            PanelOFF();
        }

    }

    private void step1()
    {
        PanelON();
        GeneralTextField.text = tutorialTexts.step1_1;

        //print movement tutorial

        if (nextText){
            tutorialPage++;
            PanelOFF();
        }
    }

    private void step2() {
        
        PanelON();
        CompassField.SetActive(true);
        //print compass tutorial

        if (nextText) {
            PanelOFF();
            tutorialPage++;
            CompassField.SetActive(false);
        }
    }

    private void step3()
    {
        if (!fill_camera)
        {   
            StopPlayerMovement(true);
            key.SetActive(true);

            PanelON();
            GeneralTextField.text = tutorialTexts.step3_1;

            FillCameraTrace();
            fill_camera = true;
        }

        

        if (cameraTrace.Count == 0)
        {
            stopCamera = true;

            if (!nextText) return;

            stopCamera = false;

            PanelOFF();

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


}
