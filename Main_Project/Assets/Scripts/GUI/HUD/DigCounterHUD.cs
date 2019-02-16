using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DigCounterHUD : MonoBehaviour {
    private TextMeshProUGUI counter;
    private ParticleSystem glow;
    private int digCount;
    private Color defaultIconColor;
    private Image digIcon;
    public bool canDig = false;

	// Use this for initialization
	void Start () {
        counter = GetComponent<TextMeshProUGUI>();
        glow = GetComponentInChildren<ParticleSystem>();
        digIcon = GetComponentInChildren<Image>();
        defaultIconColor = digIcon.color;
	}

    private void LateUpdate() {
        digCount = GameManager.Instance.currentPC.digCount;
        counter.text = digCount.ToString("00");
        canDig = GameManager.Instance.currentPC.VDig.CanDig() & GameManager.Instance.currentPC.ZDig.CanDig();
        if (digCount > 0 && canDig) {
            //glow.Play();
            glow.gameObject.SetActive(true);
            digIcon.color = defaultIconColor;
        }
        else {
            //glow.Stop();
            glow.gameObject.SetActive(false);
            digIcon.color = Color.gray;
        }
    }
}
