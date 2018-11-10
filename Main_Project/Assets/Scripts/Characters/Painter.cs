using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : MonoBehaviour {

    public GameObject prefab;
    GameObject objd;
    List<GameObject> painterTrail;

    public void ActivePainter() {
        objd = Instantiate(prefab);
        objd.transform.parent = transform;
        painterTrail.Add(objd);

        foreach (GameObject go in painterTrail)
        {
            //active Aura HALO
        }
    }

    //after x seconds or rpressing button of jolly skills
    public void DisablePainter()
    {
        foreach (GameObject go in painterTrail) {
            Destroy(go);
            painterTrail.Remove(go);
        }
        //or painterTrail.Clear();
    }

}
