
using UnityEngine;
using UnityEngine.UI;

public class DigWheel : MonoBehaviour {

    public GameObject DigPanel;

    public Button linearDig;    //from one side to another
    public Button zoneDig;      //select the wayout

    
    private PlayerController _player;

    private float _originalFixedTime;
    void Awake() {
        _originalFixedTime = Time.fixedDeltaTime;
    }
    // Use this for initialization
    void Start () {
        DigPanel.SetActive(false);
        _player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        //input to open the wheel
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetButtonDown("PS4_R1")) {
            DigPanel.SetActive(true);
            /*  Time.timeScale = .7f;   //to slow time
              Time.fixedDeltaTime = .2f * Time.timeScale;*/
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetButtonUp("PS4_R1")) {
            DigPanel.SetActive(false);
            //Time.fixedDeltaTime = _originalFixedTime;
        }

        if (Input.GetButton("PS4_R1")) {//while R1 is held down
            float rStickX = Input.GetAxis("PS4_RStick_X");

            Debug.Log("R1 + ");
            if (rStickX > .8f) {
                EnableLinear();
                Debug.Log("Linear (right)");
                //TODO:create linear digging here

                if (!_player.IsZoneDigging)
                    ;// _player.LinearCheck();
            }
            if (rStickX < -.8f) {
                EnableZone();
                Debug.Log("Zone (left)");
                //TODO:create zone digging here

                //_player.ZoneCheck();
            }

        }
    }

    public void EnableLinear() {
        linearDig.interactable=true;
        zoneDig.interactable = false;
    }

    public void EnableZone() {
        linearDig.interactable = false;
        zoneDig.interactable = true;

    }
}
