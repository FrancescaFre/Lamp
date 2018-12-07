
using UnityEngine;
using UnityEngine.UI;

/*I/DPAD_UP show/hide the lamp panel
 */

public class InGameHUD : MonoBehaviour {

    public GameObject InGameHUDPanel;
    public LampHUD lampHUDPanel;
    public PauseManagerGUI pManager;

    private void Start() {
        pManager = GetComponent<PauseManagerGUI>();
       
        lampHUDPanel.gameObject.SetActive(false);
    }

    private void FixedUpdate() {
        InGameHUDPanel.SetActive(!pManager.PausePanel.activeInHierarchy);   // mutual exclusion

        if (Input.GetAxis("PS4_DPad_Y")>0 || Input.GetKeyDown(KeyCode.I)) {
            lampHUDPanel.gameObject.SetActive(!lampHUDPanel.gameObject.activeInHierarchy);
        }
    }
}
