using UnityEngine;
using TMPro;

public class DigCounterHUD : MonoBehaviour {
    public TextMeshProUGUI counter;
	// Use this for initialization
	void Start () {
        counter = GetComponent<TextMeshProUGUI>();
	}

    private void LateUpdate() {
        counter.text = GameManager.Instance.currentPC.digCount.ToString("00");
    }
}
