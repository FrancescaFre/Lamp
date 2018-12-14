using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterArea : MonoBehaviour {

    public Autodestruct related;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
            related.DieNow = true;
    }
}
