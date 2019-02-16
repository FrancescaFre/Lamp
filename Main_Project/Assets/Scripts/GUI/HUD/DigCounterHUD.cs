using UnityEngine;
using TMPro;

public class DigCounterHUD : MonoBehaviour {
    public TextMeshProUGUI counter;
    public ParticleSystem glow;
    private int digCount;

    public bool isOnSolid = false;

	// Use this for initialization
	void Start () {
        counter = GetComponent<TextMeshProUGUI>();
        glow = GetComponentInChildren<ParticleSystem>();
	}

    private void LateUpdate() {
        digCount= GameManager.Instance.currentPC.digCount;
        counter.text = digCount.ToString("00");

        if (digCount > 0 && !isOnSolid)
            glow.Play();
        else
            glow.Stop();
    }
}
