
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuGUI : MonoBehaviour {

	public void LoadLevelSelection() {
        SceneManager.LoadScene(1);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
