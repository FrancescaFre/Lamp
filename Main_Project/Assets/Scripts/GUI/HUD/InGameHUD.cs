
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/*I/DPAD_UP show/hide the lamp panel
 */

public class InGameHUD : MonoBehaviour {
    public static InGameHUD Instance;
    public GameObject InGameHUDPanel;
    
    public PauseManagerGUI pauseManager;
    public Image victory, defeat;
    public TextMeshProUGUI allyLampCounter;
    public TextMeshProUGUI enemyLampCounter;

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
            allyLampCounter.text = string.Format("{0}/{1}", GameManager.Instance.allyLamps.ToString("00"), GameManager.Instance.levelLoaded.allyLamps.ToString("00"));
            enemyLampCounter.text = string.Format("{0}/{1}", GameManager.Instance.enemyLamps.ToString("00"), GameManager.Instance.levelLoaded.enemyLamps.ToString("00"));
        }
    }

}
