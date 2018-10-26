using UnityEngine;
using UnityEngine.UI;

public class ItemWheel : MonoBehaviour {//should be attached to the game manager

    public GameObject Wheel;    // canvas of the wheel

    //this GO are the items that the character will have in its hand
    public GameObject Item1;
    public GameObject Item2;
    public GameObject Item3;
    public GameObject Item4;

    //buttons of the wheel
    public Button top;
    public Button bottom;
    public Button left;
    public Button right;

    private float _originalFixedTime;
    void Awake() {
        _originalFixedTime = Time.fixedDeltaTime;
    }
    // Use this for initialization
    void Start () {
        Wheel.SetActive(false);
      /*  Item1.SetActive(false);
        Item2.SetActive(false);
        Item3.SetActive(false);
        Item4.SetActive(false);*/

        //this function sets all the buttons of the wheel to be interactable
        _ActivateWheel();
    }



    // Update is called once per frame
    void Update () {

        //input to open the wheel
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown("PS4_L1")) {
            Wheel.SetActive(true);
            Time.timeScale = .7f;
            Time.fixedDeltaTime = .2f * Time.timeScale;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetButtonUp("PS4_L1")) {
            Wheel.SetActive(false);
            Time.fixedDeltaTime = _originalFixedTime;
        }

        //to select item via controller
        if (Input.GetButton("PS4_L1")) {//while L1 is held down
            float rStickX = Input.GetAxis("PS4_RStick_X");
            float rStickY = Input.GetAxis("PS4_RStick_Y");

            print("L1 + ");
            #region Vertical Items
            //if the RIGHT stick is moved on the vertical axis, the top or bottom item is selected
            //the use of the tresholds guarantee a correct selection even in the worst case scenario
            if (rStickX > -0.1f && rStickX < 0.1f && rStickY < -0.9f) {
                EnableTop();
                print("up" + rStickX + rStickY);
            }
            if (rStickX > -0.1f && rStickX < 0.1f && rStickY > 0.9f) {
                EnableDown();
                print("Down" + rStickX + rStickY);
            }
            #endregion

            #region Horizontal Items
            //if the RIGHT stick is moved on the Horrizontal axis, the left or right item is selected
            //the use of the tresholds guarantee a correct selection even in the worst case scenario
            if (rStickY > -0.1f && rStickY < 0.1f && rStickX < -0.9f) {
                EnableLeft();
                print("Left" + rStickX + rStickY);
            }
            if (rStickY > -0.1f && rStickY < 0.1f && rStickX > 0.9f) {
                EnableRight();
                print("Right" + rStickX + rStickY);
            }
            #endregion
        }

    }

    #region Item Enable
    /**it does not matter which item is in in which position
     */
    public void EnableTop() {  // this enables the top item 
        if(Item1)// if the player has an item in slot "top"
            Item1.SetActive(true);
        if (Item2)
            Item2.SetActive(false);
        if (Item3)
            Item3.SetActive(false);
        if (Item4)
            Item4.SetActive(false);

        _SetTopInteractable();
    }

    public void EnableRight() {
        if (Item1)
            Item1.SetActive(false);
        if (Item2)
            Item2.SetActive(true);
        if (Item3)
            Item3.SetActive(false);
        if (Item4)
            Item4.SetActive(false);

        _SetRightInteractable();
    }
    public void EnableDown() {
        if (Item1)
            Item1.SetActive(false);
        if (Item2)
            Item2.SetActive(false);
        if (Item3)
            Item3.SetActive(true);
        if (Item4)
            Item4.SetActive(false);

        _SetBottomInteractable();
    }
    public void EnableLeft() {
        if (Item1)
            Item1.SetActive(false);
        if (Item2)
            Item2.SetActive(false);
        if (Item3)
            Item3.SetActive(false);
        if (Item4)
            Item4.SetActive(true);

        _SetLeftInteractable();
    }
    #endregion

    #region Set Interactable
    /**this functions sets as interactable the buttons
     * to let the player have a visual feedback on which item is selected
     * 
     * Each buttons have a component "Event trigger" that listen to the event
     * "on mouse enter" on the area of a slice of the wheel
     * when the mouse enter the area it calls the corresponding function  (EnableXXXX)
     * that enables the corresponding item
     */
    private void _ActivateWheel() {
        top.interactable = true;
        bottom.interactable = true;
        left.interactable = true;
        right.interactable = true;
    }
    private void _SetTopInteractable() {
       top.interactable = true;
        bottom.interactable = false;
        left.interactable = false;
        right.interactable = false;

    }

    private void _SetBottomInteractable() {
        top.interactable = false;
        bottom.interactable = true;
        left.interactable = false;
        right.interactable = false;
    }

    private void _SetLeftInteractable() {
        top.interactable = false;
        bottom.interactable = false;
        left.interactable = true;
        right.interactable = false;

    }

    private void _SetRightInteractable() {
        top.interactable = false;
        bottom.interactable = false;
        left.interactable = false;
        right.interactable = true;

    }
    #endregion
}
