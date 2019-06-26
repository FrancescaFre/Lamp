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
    private int insideSteps = 0; 
    

    [Header("Tutorial texts")]
    public Tutorial_SO tutorialTexts;

    public TextMeshProUGUI GeneralTextField;
    public GameObject CompassField;
    public GameObject SpaceBar;
    public List<GameObject> Step3_steps;
    public List<GameObject> Step5_steps;
    public GameObject step6_1;
    public List<GameObject> Step10_steps;


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
        SpaceBar.SetActive(false);

        Step3_steps.ForEach(GO => GO.SetActive(false));
        Step5_steps.ForEach(GO => GO.SetActive(false));
        step6_1.SetActive(false); ;
        Step10_steps.ForEach(GO => GO.SetActive(false));
        
    }

    // Update is called once per frame
    private void Update () {
         CheckEvent();
         SpaceBar.SetActive(!nextText);

         if (TutorialPanel.activeInHierarchy && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown(Controllers.PS4_Button_X))){
             stopCamera = false;
             nextText = true;
         }

         if(!stopCamera)
            MoveCamera();

        if (disableVerticalDig) {
            DigBehaviour.instance.isVerticalActive = false; 
        }

        if (disableZoneDig) {
            DigBehaviour.instance.isZoneActive = false; 
        }

    }

    private void MoveCamera() {  
        //---- skip
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
            case 0: Step0(); break;

            case 1: Step1(); break;

            case 2:               
                if (GameManager.Instance.allyLamps == 1)
                    Step2(); break;

            case 3:
                if(GameManager.Instance.allyLamps == 2)
                    Step3(); break;

            case 4:
                if (GameManager.Instance.allyLamps == 3)
                    Step4(); break;

            case 5:
                if (GameManager.Instance.digCount > 0)
                    Step5(); break;

            case 9:
                if(GameManager.Instance.enemyLamps == 0)
                    Step9(); break;
        }

    }

    public void EnterNotify(int n) {
        switch (n) {
            case 6: if (tutorialPage == 6) Step6(); break;
            case 7: if (tutorialPage == 7) Step7(); break;
            case 8: if (tutorialPage == 8) Step8(); break;
            case 10: if (tutorialPage == 10) Step10(); break;
            case 11: nextText = true;  if (tutorialPage == 11) Step11(); break;
        }
    }

    public void ExitNotify(int n) {
         nextText = true; 
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
    //Zoom over lamp and candles ---
    private void Step0()
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

    //WASD ---
    private void Step1()
    {
        //print movement tutorial
        PanelON();
        GeneralTextField.text = tutorialTexts.step1_1;

        if (!nextText) return;

        tutorialPage++;
        PanelOFF();
    }

    //COMPASS ---
    private void Step2() {
        //print compass tutorial
        PanelON();
        CompassField.SetActive(true);
       
        if (!nextText) return;

        PanelOFF();
        tutorialPage++;
        CompassField.SetActive(false);
    }

    //Zoom over key and gate ---
    private void Step3()
    {
        if (!fill_camera)
        {   
            StopPlayerMovement(true);
            key.SetActive(true);
            insideSteps = 0;

            //show key + text
            PanelON();
            Step3_steps[insideSteps].SetActive(true);

            nextText = false;
            FillCameraTrace();
            fill_camera = true;
            insideSteps++;
        }

        if (cameraTrace.Count == 0 && insideSteps == 1 && nextText) {
            stopCamera = true;
            //show gate + text
            Step3_steps[0].SetActive(false);
            Step3_steps[insideSteps].SetActive(true);
            insideSteps++;
            nextText = false; 
        }

        if (cameraTrace.Count == 0 && insideSteps == 2 && nextText)
        {
            //last space to close the 
            stopCamera = false;
            Step3_steps[1].SetActive(false) ;
            PanelOFF();

            StopPlayerMovement(false);
            tutorialPage++;
            fill_camera = false;
        }
    }

    //Zoom over diggable object (no text)
    private void Step4()
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

    //Tutorial Zone-DIG
    private void Step5()
    {
        if (!fill_camera)
        {
            //tutorial zone dig
            PanelON();
            insideSteps = 0;
            Step5_steps[insideSteps].SetActive(true);

            disableZoneDig = false;
            FillCameraTrace();
            fill_camera = true;
            insideSteps++;
            nextText = false;
        }

        if (nextText && insideSteps == 1) {
            Step5_steps[insideSteps].SetActive(true);
            insideSteps++;
            nextText = false;
        }

        if (nextText && insideSteps == 2) {
            Step5_steps[insideSteps].SetActive(true);
            insideSteps++;
            nextText = false;
        }

        if (nextText && insideSteps == 3) {

            Step5_steps[0].SetActive(false);
            Step5_steps[1].SetActive(false);
            Step5_steps[2].SetActive(false);
            Step5_steps[insideSteps].SetActive(true);
            insideSteps++;
            nextText = false;
        }

        if (cameraTrace.Count == 0  && insideSteps == 4)
        {
            if (!nextText) return;

            Step5_steps[3].SetActive(false);
            PanelOFF();

            insideSteps = 0; 
            fill_camera = false;
            tutorialPage++;
        }
    }

    //Tutorial advanced movement and enemies
    private void Step6() {
        if (!fill_camera) {
            StopPlayerMovement(true);

            //tutorial approach to the enemies
            PanelON();
            step6_1.SetActive(true);
            
            FillCameraTrace();
            fill_camera = true;
            insideSteps = 0;
        }

        //if the camera have as target the player
        if (BasicCamera.instance.is_character && nextText == true && insideSteps == 0) {
            //just to warn...
            step6_1.SetActive(false);
            GeneralTextField.text = tutorialTexts.step6_2;
            //alterate vignette
            BasicCamera.instance.ChangeVignetteSmoothness(true);

            nextText = false;
            insideSteps++;
        }

        if (cameraTrace.Count == 0 && nextText == true && insideSteps == 1) {
            //ripristinate vignentte
            BasicCamera.instance.ChangeVignetteSmoothness(false);

            nextText = false;
            insideSteps++;
        }

        if (cameraTrace.Count == 0 && nextText && insideSteps == 2) {
            PanelOFF();

            insideSteps = 0;
            StopPlayerMovement(false);
            fill_camera = false; 
            tutorialPage++; 
        }
    }

    //Repeat zoneDIG
    private void Step7()
    {   
        //repeat drill tutorial 
        if (insideSteps == 0) {
            Step5_steps[3].SetActive(true);
            var temp = Step5_steps[3].GetComponent<TextMeshProUGUI>();
            temp.text = "Remember \n" + temp.text;
            insideSteps++;
        }

        if (!nextText) return;
        Step5_steps[3].SetActive(false);
        insideSteps = 0; 
        tutorialPage++;
        PanelOFF();
    }

    //In front of the cursed lamp
    private void Step8()
    {
        if (!fill_camera)
        {
            StopPlayerMovement(true);
            PanelON();
            GeneralTextField.text = tutorialTexts.step8_1;
            //these lamp have some problem
            FillCameraTrace();
            fill_camera = true;
        }

        if (cameraTrace.Count == 1)
        {
            if (!nextText) return;
            //this cursed lamp...
            GeneralTextField.text = tutorialTexts.step8_2;
            nextText = false; 
        }

        if (cameraTrace.Count == 0)
        {
            stopCamera = true; 

            if (!nextText) return;

            stopCamera = false; 
            PanelOFF();
            StopPlayerMovement(false);
            fill_camera = false;
            tutorialPage++;
        }
    }

    //go to the new drill
    private void Step9()
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
            stopCamera = true;
            if (!nextText) return;

            stopCamera = false; 
            PanelOFF();
            StopPlayerMovement(false);
            fill_camera = false;
            tutorialPage++;
        }
    }

    //vertical tutorial 
    private void Step10() {
        if (insideSteps == 0) {
            PanelON();

            disableVerticalDig = false;
            //tutorial vertical dig
            Step10_steps[insideSteps].SetActive(true);
            insideSteps++;
            nextText = false;
        }

        if (insideSteps == 1 && nextText) {
            Step10_steps[insideSteps].SetActive(true);
            insideSteps++;
            nextText = false;
        }

        if (insideSteps == 2 && nextText) {
            Step10_steps[0].SetActive(false);
            Step10_steps[1].SetActive(false);
            Step10_steps[insideSteps].SetActive(true);
            insideSteps++;
            nextText = false;
        }

        if (insideSteps == 3 && nextText) return;
        {
            Step10_steps[2].SetActive(false);
            PanelOFF();
            tutorialPage++;
            insideSteps = 0;
        }
    }

    //missing piece
    private void Step11()
    {
        if (!fill_camera)
        {
            missing.SetActive(true);
            StopPlayerMovement(true);
            //these lamp have some problem...
            PanelON();
            GeneralTextField.text = tutorialTexts.step11_1;

            FillCameraTrace();
            fill_camera = true;

            nextText = false;
            insideSteps = 0;
        }

        if (cameraTrace.Count == 0 && insideSteps == 0 && nextText)
        {
            //piece...
            GeneralTextField.text = tutorialTexts.step11_1;
            insideSteps++;

            nextText = false;
        }

        if (cameraTrace.Count == 0 && insideSteps == 1 && nextText)
        {
            PanelOFF(); 
            StopPlayerMovement(false);
            fill_camera = false;
            tutorialPage++;
            insideSteps = 0;
        }
    }

    private void StopPlayerMovement(bool mov)
    {
        GameManager.Instance.currentPC.GetComponent<PlayerMovement>().OnIce = mov;
    }


}
