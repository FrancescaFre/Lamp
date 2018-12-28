using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForSpawn : MonoBehaviour {

    public List<LampBehaviour> lamps = new List<LampBehaviour>();

    private void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
            this.transform.GetChild(i).gameObject.SetActive(false);
    }

    private void Update()
    {
        if (lamps.TrueForAll(IsTurnedOnLamp))
        {
            for (int i = 0; i < this.transform.childCount; i++)
                this.transform.GetChild(i).gameObject.SetActive(true);
        }

    }

    private static bool IsTurnedOnLamp(LampBehaviour lamp)
    {
        return lamp.isTurnedOn;
    }

}
