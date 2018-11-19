using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : MonoBehaviour {
    //become invisible to enemies and disable any action, like 
    // switch on lamps, using object, dig
    private int skillTime;
    private PlayerController pc;

    public void ActivateMimic() {
        pc = GetComponent<PlayerController>();
        //start Caster
        //add flag for block actions on player controller
        pc.IsMimicOrDash = true;
        gameObject.layer = 0; //default layer
        Invoke("DisableMimic", skillTime); //after x seconds disable mimic      
    }

    //the jolly skill will be disabled when the player press again the button, or after x seconds
    public void DisableMimic() {
        if (pc.usingSkill)
        {
            pc.usingSkill = false;
            pc.IsMimicOrDash = false;
            this.gameObject.layer = 9; //player layer
        }
        else return;
    }
}

