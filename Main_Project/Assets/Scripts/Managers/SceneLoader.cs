using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using MEC;

public class SceneLoader : MonoBehaviour {
    public static SceneLoader instance;
    [SerializeField]
    private Canvas LoadingCanvas;
    [SerializeField]
    private Slider loadingBar;
    [SerializeField]
    private TextMeshProUGUI loadingValue;
    public AsyncOperation async;

    private void Awake() {
        if (!instance) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        LoadingCanvas = GetComponentInChildren<Canvas>();
        loadingBar = GetComponentInChildren<Slider>();
        loadingValue= GetComponentInChildren<TextMeshProUGUI>();
        LoadingCanvas.gameObject.SetActive(false);
    }

    private void UpdateProgressUI(float progress) {
        loadingBar.value = progress;
        loadingValue.text = (int)(progress * 100f) + "%";
        //loadingValue.text = string.Format("{0,00}%", (progress * 100f));
    }

    public void LoadSceneAsync(int sceneIndex = -1, string sceneName = "") {
        if (sceneIndex == -1 && sceneName.Equals("")) {
            Debug.Log("no scene to load");
            return;
        }

        UpdateProgressUI(0);
        

        /*if (sceneIndex >= 0) {
            Timing.RunCoroutine(_LoadSceneAsync(sceneIndex));
            return;
        }
        if (!sceneName.Equals("")) {
            Timing.RunCoroutine(_LoadSceneAsync(sceneName: sceneName));
            return;
        }/**/

        if (sceneIndex >= 0) {
            StartCoroutine(_LoadSceneAsync(sceneIndex));
            return;
        }
        if (!sceneName.Equals("")) {
            StartCoroutine(_LoadSceneAsync(sceneName: sceneName));
            return;
        }/**/
    }

    /*private IEnumerator<float> _LoadSceneAsync(int sceneIndex=-1, string sceneName="") {
        LoadingCanvas.gameObject.SetActive(true);

        if (sceneIndex >= 0)
             async = SceneManager.LoadSceneAsync(sceneIndex);
        else
            async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone) {
            UpdateProgressUI(async.progress);
            yield return Timing.WaitForOneFrame;
        }
        UpdateProgressUI(async.progress);
        async = null;
        LoadingCanvas.gameObject.SetActive(false);
        loadingBar.value = 0f;
    }/**/

    private IEnumerator _LoadSceneAsync(int sceneIndex = -1, string sceneName = "") {
        LoadingCanvas.gameObject.SetActive(true);

        if (sceneIndex >= 0)
            async = SceneManager.LoadSceneAsync(sceneIndex);
        else
            async = SceneManager.LoadSceneAsync(sceneName);
            
        while (!async.isDone) {
            
            UpdateProgressUI(async.progress);
            yield return null;
        }
        
        UpdateProgressUI(async.progress);
        async = null;
        LoadingCanvas.gameObject.SetActive(false);
    }/**/
}
