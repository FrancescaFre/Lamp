using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour {

    public List<Material> materials;

    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material = materials[0];
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
            gameObject.GetComponent<MeshRenderer>().material = materials[1];
    }
}
