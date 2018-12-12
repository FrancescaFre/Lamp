using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // if (other.GetComponent<PlayerController>().keys > 0)
            {
                //other.GetComponent<PlayerController>().keys--;
                foreach (Animator anim in this.transform.GetComponentsInChildren<Animator>()) {
                    anim.SetBool("OpenTheGate", true);
                    anim.GetComponent<Collider>().isTrigger = true;
                }
            }

        }
    }
}
