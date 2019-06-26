
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using MEC;

public class StartMenuGUI : MonoBehaviour {

    public GameObject aboutUs;
    public GameObject credits;
    public GameObject startMenuPanel;
    
    public GameObject creditsBack;
    public GameObject AboutBack;
    public GameObject play;

    public GameObject Lamp;
    public GameObject logo;


    private void Start() {

        startMenuPanel.SetActive(true);
        aboutUs.SetActive(false);
        credits.SetActive(false);
    }

    public void LoadLevelSelection() {
        
        //SceneManager.LoadScene(1);
        SceneLoader.instance.LoadSceneAsync(1);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void AboutUs() {
        aboutUs.SetActive(true);
        startMenuPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(AboutBack);

    }
    public void BackFromAboutUs() {
        aboutUs.SetActive(false);
        startMenuPanel.SetActive(true);
    }
    public void Credits() {
        
        credits.SetActive(true);
        startMenuPanel.SetActive(false);
        Lamp.SetActive(false);
        logo.SetActive(false);
        EventSystem.current.SetSelectedGameObject(creditsBack);
    }


    public void BackFromCredits() {

        credits.SetActive(false);
        startMenuPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(play);
        Lamp.SetActive(true);
        logo.SetActive(true);

    }
    public void OpenFBLink() {
        Application.OpenURL("https://www.facebook.com/pg/LampGameProject");
    }

    IEnumerator _AsyncLoadScene(){

        AsyncOperation async= SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);
        //async.allowSceneActivation = false;
        while (!async.isDone){
            Debug.Log(async.progress);
            yield return null;
        }
      
        //async.allowSceneActivation = true;
    }
}
