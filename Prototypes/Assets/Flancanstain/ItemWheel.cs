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
