using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : MonoBehaviour {
    //become invisible to enemies and disable any action, like 
    // switch on lamps, using object, dig

    public void ActiveMimic() {
        //add flag for block actions on player controller

        gameObject.layer = 0; //default layer
        //after x seconds disable mimic      
    }

    //the jolly skill will be disabled when the player press again the button, or after x seconds
    public void DisableMimic() {
        //enable actions
        this.gameObject.layer = 9; //player layer
    }
}

