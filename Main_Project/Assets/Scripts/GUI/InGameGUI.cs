
using UnityEngine;
using UnityEngine.UI;

/*I/DPAD_UP show/hide the lamp panel
 */

public class InGameGUI : MonoBehaviour {

    public GameObject InGameGUIPanel;
    public GameObject lampPanel;
    public PauseManagerGUI pManager;

    private void Start() {
        pManager = GetComponent<PauseManagerGUI>();
        GameManager.Instance.lampGUI = GetComponentInChildren<LampGUI>();
        lampPanel.SetActive(false);
    }

    private void FixedUpdate() {
        InGameGUIPanel.SetActive(!pManager.PausePanel.activeInHierarchy);   // mutual exclusion

        if (Input.GetAxis("PS4_DPad_Y")>0 || Input.GetKeyDown(KeyCode.I)) {
            lampPanel.SetActive(!lampPanel.activeInHierarchy);
        }
    }
}
