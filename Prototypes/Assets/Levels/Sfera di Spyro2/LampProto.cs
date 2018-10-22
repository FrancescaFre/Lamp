using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampProto : MonoBehaviour {

    public List<Material> materials;

    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material = materials[0];
    }
    private void OnTriggerEnter(Collider other)
    {
        // Quando ho fatto un altro playercontroller dava problemi, quindi ho rinominato il mio.
        // Questa versione di script usa questo (ma commentata sotto c'è l'originale)

        if (other.GetComponent<PlayerControllerHummel>())
            gameObject.GetComponent<MeshRenderer>().material = materials[1];

        /*
        if (other.GetComponent<PlayerController>())
            gameObject.GetComponent<MeshRenderer>().material = materials[1];
        */
    }
}
