using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : MonoBehaviour {

    public GameObject key;

    private void Update()
    {
        if (GameManager.Instance.allyLamps == 7)
            key.SetActive(true);
    }
}
