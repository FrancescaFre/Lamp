using UnityEngine;
using TMPro;

public class DigCounterHUD : MonoBehaviour {
    public TextMeshProUGUI counter;
    public ParticleSystem glow;
    private int digCount;

    public bool canDig = false;

	// Use this for initialization
	void Start () {
        counter = GetComponent<TextMeshProUGUI>();
        glow = GetComponentInChildren<ParticleSystem>();
	}

    private void LateUpdate() {
        digCount= GameManager.Instance.currentPC.digCount;
        counter.text = digCount.ToString("00");
        canDig = GameManager.Instance.currentPC.VDig.CanDig() & GameManager.Instance.currentPC.ZDig.CanDig();
        if (digCount > 0 && canDig)
            glow.Play();
        else
            glow.Stop();
    }
}
