
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuGUI : MonoBehaviour {

    public GameObject aboutUs;
    public GameObject startMenu;

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
}
