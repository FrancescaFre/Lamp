
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/*I/DPAD_UP show/hide the lamp panel
 */

public class InGameHUD : MonoBehaviour {
    public static InGameHUD Instance;
    public GameObject InGameHUDPanel;
    public LampHUD lampHUDPanel;
    public PauseManagerGUI pManager;
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


        pManager = GetComponent<PauseManagerGUI>();
        InGameHUDPanel = lampHUDPanel.transform.parent.gameObject;        
    }


    private void LateUpdate() {
        if (GameManager.Instance) {
            allyLampCounter.text = string.Format("{0}/{1}", GameManager.Instance.allyLamps.ToString("00"), GameManager.Instance.levelLoaded.allyLamps.ToString("00"));
            enemyLampCounter.text = string.Format("{0}/{1}", GameManager.Instance.enemyLamps.ToString("00"), GameManager.Instance.levelLoaded.enemyLamps.ToString("00"));
        }
    }
    private void FixedUpdate() {
        

        if (Input.GetAxis(Controllers.PS4_DPad_Y) > 0 || Input.GetKeyDown(KeyCode.I)) {
            lampHUDPanel.gameObject.SetActive(!lampHUDPanel.gameObject.activeInHierarchy);
        }
    }
}
