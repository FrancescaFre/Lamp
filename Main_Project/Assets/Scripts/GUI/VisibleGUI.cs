using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisibleGUI : MonoBehaviour {

    public Sprite Safe;
    public Sprite Seen;
    public Sprite Heard;
    public Sprite Hidden;

    private void LateUpdate() {
        if (GameManager.Instance.currentPC.IsSafe) {
            GetComponent<Image>().sprite = Safe;
            return;
        }
        if (GameManager.Instance.howManySeeing > 0) {
            GetComponent<Image>().sprite = Seen;
            return;
        }
        if (GameManager.Instance.howManyHearing > 0) {
            GetComponent<Image>().sprite = Heard;
            return;
        }

        GetComponent<Image>().sprite = Hidden;


    }

}
