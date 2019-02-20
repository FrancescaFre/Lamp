using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCounterHUD : MonoBehaviour {
    private enum ItemType { KEY = 0, MISSING, DRILL }

    [SerializeField]
    private ItemType item;

    private TextMeshProUGUI counter;
    private ParticleSystem glow;
    private Color defaultIconColor;
    private Image Icon;
    private PlayerController currentPC;

    private bool canBeEnabled = false;
    void Start() {
        counter = GetComponent<TextMeshProUGUI>();
        glow = GetComponentInChildren<ParticleSystem>();
        Icon = GetComponentInChildren<Image>();
        defaultIconColor = Icon.color;
    }

    private void LateUpdate() {
        if (currentPC != GameManager.Instance.currentPC)
            currentPC = GameManager.Instance.currentPC;

        if (item == ItemType.KEY) {
            counter.text = currentPC.keys.ToString("00");
            if (currentPC.keys > 0)
                canBeEnabled = true;
            else
                canBeEnabled = false;


        }
        else if (item == ItemType.MISSING) {
            counter.text = currentPC.missingParts.ToString("00");
            if (currentPC.missingParts > 0)
                canBeEnabled = true;
            else
                canBeEnabled = false;
        }
        else if (item == ItemType.DRILL) {
            counter.text = currentPC.digCount.ToString("00");
            canBeEnabled = currentPC.VDig.CanDig() & currentPC.ZDig.CanDig() & currentPC.digCount > 0;
        }

        glow.gameObject.SetActive(canBeEnabled);

        if (canBeEnabled) {

            Icon.color = defaultIconColor;
        }
        else {

            Icon.color = Color.gray;
        }
    }
}
