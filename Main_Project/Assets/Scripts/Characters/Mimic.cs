using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : Skill {
    //become invisible to enemies and disable any action, like 
    // switch on lamps, using object, dig
    private PlayerController playerController;
    private int skillTime;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        playerController.skill = this;
    }

    override public void ActivateSkill() {
        //start Caster
        //add flag for block actions on player controller
        playerController.IsMimicOrDash = true;
        gameObject.layer = 0; //default layer
        Invoke("DeactivateSkill", skillTime); //after x seconds disable mimic      
    }

    //the jolly skill will be disabled when the player press again the button, or after x seconds
    override public void DeactivateSkill() {
        if (playerController.usingSkill)
        {
            playerController.usingSkill = false;

            playerController.usingSkill = false;
            playerController.IsMimicOrDash = false;
            this.gameObject.layer = 9; //player layer
        }
        else return;
    }
}

