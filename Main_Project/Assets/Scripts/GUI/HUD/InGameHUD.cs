
using UnityEngine;
using UnityEngine.UI;

/*I/DPAD_UP show/hide the lamp panel
 */

public class InGameHUD : MonoBehaviour {
    public static InGameHUD Instance;
    public GameObject InGameHUDPanel;
    public LampHUD lampHUDPanel;
    public PauseManagerGUI pManager;
    public Image victory, defeat;

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

    

    private void FixedUpdate() {
        InGameHUDPanel.SetActive(!pManager.PausePanel.activeInHierarchy);   // mutual exclusion

        if (Input.GetAxis("PS4_DPad_Y") > 0 || Input.GetKeyDown(KeyCode.I)) {
            lampHUDPanel.gameObject.SetActive(!lampHUDPanel.gameObject.activeInHierarchy);
        }
    }
}
