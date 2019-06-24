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


    public bool canBeEnabled = false;
    void Start() {
        counter = GetComponent<TextMeshProUGUI>();
        glow = GetComponentInChildren<ParticleSystem>();
        Icon = GetComponentInChildren<Image>();
        defaultIconColor = Icon.color;
    }

    private void LateUpdate() {

        if (!GameManager.Instance.currentPC) return;

        if (item == ItemType.KEY) {
            counter.text = GameManager.Instance.keys.ToString("00");

            if (GameManager.Instance.keys > 0)
                canBeEnabled = true;
            else
                canBeEnabled = false;
        }
        else if (item == ItemType.MISSING) {
            counter.text = GameManager.Instance.missingParts.ToString("00");

            if (GameManager.Instance.missingParts > 0)
                canBeEnabled = true;
            else
                canBeEnabled = false;
        }
        else if (item == ItemType.DRILL) {
            counter.text = GameManager.Instance.digCount.ToString("00");

          /*  canBeEnabled = GameManager.Instance.currentPC.VDig.CanDig() 
                            & GameManager.Instance.currentPC.ZDig.CanDig() 
                            & GameManager.Instance.digCount > 0;
            */

            canBeEnabled=(DigBehaviour.instance.CanZoneDig() || DigBehaviour.instance.CanZoneDig()) && GameManager.Instance.digCount > 0;
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
