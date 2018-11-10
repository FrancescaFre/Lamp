using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : MonoBehaviour {

    public GameObject prefab;
    GameObject objd;
    List<GameObject> painterTrail;
    PlayerController pc;
    int skillTime; 


    public void ActivePainter() {
        pc = GetComponent<PlayerController>();
        Invoke("DisablePainter",skillTime);
    }

    void FixedUpdate()
    {
        if (pc.usingSkill)
        {
            objd = Instantiate(prefab);
            objd.layer = 11; //layer 11 = Obstacles
            objd.transform.parent = transform;
            painterTrail.Add(objd);
        }
    }

    //after x seconds or rpressing button of jolly skills
    public void DisablePainter()
    {
        if (pc.usingSkill)
        {
            pc.usingSkill = false;
            foreach (GameObject go in painterTrail)
                Destroy(go);

            painterTrail.Clear();
        }
        else return; 
    }
  
}
