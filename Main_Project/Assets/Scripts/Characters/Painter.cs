using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : MonoBehaviour {

    public GameObject prefab;
    GameObject objd;
    List<GameObject> painterTrail;
    ParticleSystem ps;
    PlayerController pc;
    int skillTime; 


    public void ActivatePainter() {
        pc = GetComponent<PlayerController>();
        ps = GetComponentInChildren<ParticleSystem>();
        ps.gameObject.SetActive(true); //start the trail

        //start caster
        Invoke("DisablePainter",skillTime);
    }

    void FixedUpdate()
    {
        if (pc.usingSkill)
        {
            objd = Instantiate(prefab);
            objd.layer = 11; //layer 11 = Obstacles
            painterTrail.Add(objd);
        }
    }

    //after x seconds or rpressing button of jolly skills
    public void DisablePainter()
    {
        if (pc.usingSkill)
        {
            pc.usingSkill = false;
            ps.gameObject.SetActive(false);
            foreach (GameObject go in painterTrail)
                Destroy(go);

            painterTrail.Clear();
        }
        else return; 
    }
  
}
