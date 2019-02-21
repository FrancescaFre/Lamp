
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuGUI : MonoBehaviour {

    public GameObject aboutUs;
    public GameObject startMenu;


    private void Start() {
        startMenu.SetActive(true);
        aboutUs.SetActive(false);
    }

    public void LoadLevelSelection() {
        SceneManager.LoadScene(1);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void AboutUs() {
        aboutUs.SetActive(true);
        startMenu.SetActive(false);
    }
    public void Back() {
        aboutUs.SetActive(false);
        startMenu.SetActive(true);
    }
    public void OpenFBLink() {
        Application.OpenURL("https://www.facebook.com/pg/LampGameProject");
    }
}
