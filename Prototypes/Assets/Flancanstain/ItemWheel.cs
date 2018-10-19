using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWheel : MonoBehaviour {//should be attached to the game manager
    public GameObject Wheel;
    public GameObject Item1;
    public GameObject Item2;
    public GameObject Item3;
    public GameObject Item4;
	// Use this for initialization
	void Start () {
        Wheel.SetActive(false);
        Item1.SetActive(false);
        Item2.SetActive(false);
        Item3.SetActive(false);
        Item4.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.I) || Input.GetButtonDown("PS4_L1"))
            Wheel.SetActive(true);
        if (Input.GetKeyUp(KeyCode.I) || Input.GetButtonUp("PS4_L1"))
            Wheel.SetActive(false);

        //to select via controller
        if (Input.GetButton("PS4_L1")) {//while L1 is held down
            float rStickX = Input.GetAxis("PS4_RStick_X");
            float rStickY = Input.GetAxis("PS4_RStick_Y");

            print("L1 + ");
            #region Vertical Items
            //if the RIGHT stick is moved on the vertical axis, the top or bottom item is selected
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
            ////if the RIGHT stick is moved on the Horrizontal axis, the left or right item is selected
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
    /*it does not matter which one is in in which position
     */
   public void EnableTop() {  // this enables the top item
        Item1.SetActive(true);
        Item2.SetActive(false);
        Item3.SetActive(false);
        Item4.SetActive(false);
    }

    public void EnableRight() {
        Item1.SetActive(false);
        Item2.SetActive(true);
        Item3.SetActive(false);
        Item4.SetActive(false);
    }
    public void EnableDown() {
        Item1.SetActive(false);
        Item2.SetActive(false);
        Item3.SetActive(true);
        Item4.SetActive(false);
    }
    public void EnableLeft() {
        Item1.SetActive(false);
        Item2.SetActive(false);
        Item3.SetActive(false);
        Item4.SetActive(true);
    }
    #endregion
}
