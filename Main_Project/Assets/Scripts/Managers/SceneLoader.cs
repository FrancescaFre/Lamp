using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


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
        else {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;  // add a delegate to be run everytime a scene is loaded
    }

    // Use this for initialization
    void Start () {
        LoadingCanvas = GetComponentInChildren<Canvas>();
        loadingBar = GetComponentInChildren<Slider>();
        loadingValue= GetComponentInChildren<TextMeshProUGUI>();
        LoadingCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Delegate for every scene loaded
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

        if (scene.name.StartsWith("0_") || scene.name.StartsWith("1_")) return;
        Debug.Log("OnSceneLoaded: " + scene.name + " " + scene.buildIndex);
        if (!EventSystem.current) {
            GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem));
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        AudioManager.Instance.OnStartGame();

        GameManager.Instance.StartGame();
    }

    /// <summary>
    /// Updates the loading bar and label with the specified value
    /// </summary>
    /// <param name="progress"></param>
    private void UpdateProgressUI(float progress) {
        loadingBar.value = progress;
        loadingValue.text = (int)(progress * 100f) + "%";
        //loadingValue.text = string.Format("{0,00}%", (progress * 100f));
    }

    /// <summary>
    /// Load a scene asynchronously
    /// </summary>
    /// <param name="sceneIndex">Index of the scene (default:-1)</param>
    /// <param name="sceneName">Name of the scene (default: "")</param>
    public void LoadSceneAsync(int sceneIndex = -1, string sceneName = "") {
        if (sceneIndex == -1 && sceneName.Equals("")) {
            Debug.Log("no scene to load");
            return;
        }

        UpdateProgressUI(0);

        if (sceneIndex >= 0) {
            StartCoroutine(_LoadSceneAsync(sceneIndex));
            return;
        }
        if (!sceneName.Equals("")) {
            StartCoroutine(_LoadSceneAsync(sceneName: sceneName));
            return;
        }
    }



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
    }
}
