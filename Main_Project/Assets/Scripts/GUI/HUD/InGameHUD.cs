
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InGameHUD : MonoBehaviour {
    public static InGameHUD Instance;
    public GameObject InGameHUDPanel;

    public PauseManagerGUI pauseManager;
    public Image victory, defeat;
    public TextMeshProUGUI allyLampCounter;
    public TextMeshProUGUI enemyLampCounter;
    [Header("In-HUD GO prefabs")]
    public GameObject questionMarkPrefab;
    public GameObject CasterPrefab;

    private void Awake() {
        if (!Instance) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }



    }

    private void Start() {
        pauseManager = GetComponent<PauseManagerGUI>();

        foreach (Transform child in transform)
            if (child.CompareTag(Tags.HUDInGame))
                InGameHUDPanel = child.gameObject;
    }
    private void LateUpdate() {
        if (GameManager.Instance) {
            if (!GameManager.Instance.levelLoaded) return;
            allyLampCounter.text = string.Format("{0}/{1}", GameManager.Instance.allyLamps.ToString("00"), GameManager.Instance.levelLoaded.allyLamps.ToString("00"));
            enemyLampCounter.text = string.Format("{0}/{1}", GameManager.Instance.enemyLamps.ToString("00"), GameManager.Instance.levelLoaded.enemyLamps.ToString("00"));
        }
    }

    public void CreateHUDReferences(ref Image questionMark, ref Slider caster) {

        questionMark = Instantiate(questionMarkPrefab, InGameHUDPanel.transform).GetComponent<Image>();
        questionMark.gameObject.SetActive(false);

        caster = Instantiate(CasterPrefab, InGameHUDPanel.transform).GetComponent<Slider>();
        caster.gameObject.SetActive(false);

    }

}
