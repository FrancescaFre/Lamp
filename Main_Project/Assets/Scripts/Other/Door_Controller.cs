using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Controller : MonoBehaviour {

    PlayerController _player;
    // Use this for initialization
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Tags.Player)) {
            Debug.Log("Player collider");
            if (collision.collider.GetComponent<PlayerController>().keys > 0)
            {
                Debug.Log("Player1");
                _player = collision.collider.GetComponent<PlayerController>();
                _player.keys--;
                AnimationManager.Anim_OpenDoor(_player.characterAnimator);

                //Lock movement
                _player.runningAnimation = true;
                Invoke ("UnlockDoor", AnimationManager.Anim_LenghtAnim(_player.characterAnimator, "Opening"));
                //Debug.Log("time " + AnimationManager.Anim_LenghtAnim(_player.characterAnimator, "Opening"));
              //  GameObject.Find("WASD").GetComponent<Autodestruct>().DieNow = true;
                this.GetComponent<Collider>().isTrigger = true; 
            }
        }
    }

    private void UnlockMovement() {
        _player.runningAnimation = false;
    }

    private void UnlockDoor() { 

        this.GetComponent<Collider>().isTrigger = true;
        foreach (Animator anim in this.transform.GetComponentsInChildren<Animator>())
        {
            GetComponent<SFXEmitter>().PlayOneShot();
            anim.SetBool("OpenTheGate", true);
            anim.GetComponent<Collider>().isTrigger = true;
            Invoke("UnlockMovement", 1f);
        }
       
    }


}

