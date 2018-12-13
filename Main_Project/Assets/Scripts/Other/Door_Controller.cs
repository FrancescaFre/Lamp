using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Controller : MonoBehaviour {

    // Use this for initialization
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) {
            Debug.Log("Player collider");
            if (collision.collider.GetComponent<PlayerController>().keys > 0)
            {
                Debug.Log("Player1");
                collision.collider.GetComponent<PlayerController>().keys--;
                foreach (Animator anim in this.transform.GetComponentsInChildren<Animator>())
                {
                    anim.SetBool("OpenTheGate", true);
                    anim.GetComponent<Collider>().isTrigger = true;
                }
                this.GetComponent<Collider>().isTrigger = true; 
            }
        }
    }
}

