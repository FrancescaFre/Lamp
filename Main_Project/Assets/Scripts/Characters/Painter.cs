using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : Skill
{

    public GameObject prefab;
    GameObject objd;
    List<GameObject> painterTrail;
    ParticleSystem particlesSystem;
    PlayerController playerController;
    int skillTime;
    float timePassed;

    public void Start()
    {
        particlesSystem = GetComponent<ParticleSystem>();
        particlesSystem.gameObject.SetActive(false);

        playerController = GetComponentInParent<PlayerController>();
        playerController.skill = this;

        painterTrail = new List<GameObject>();
    }

    override public void ActivateSkill()
    {
        particlesSystem.gameObject.SetActive(true); //start the trail

        //start caster
        Invoke("DeactivateSkill", skillTime);
    }

    public void Update()
    {
        if (playerController.usingSkill)
        {
            DropPosition();
        }
    }

    //after x seconds or rpressing button of jolly skills
    override public void DeactivateSkill()
    {
        if (playerController.usingSkill)
        {
            playerController.usingSkill = false;
            particlesSystem.gameObject.SetActive(false);
            foreach (GameObject go in painterTrail)
                Destroy(go);

            painterTrail.Clear();
        }
        else return;
    }

    private void DropPosition()
    {
        if (timePassed < 0.1f)
            timePassed += Time.deltaTime;
        else
        {
            objd = Instantiate(prefab);
            //objd.layer = 11; //layer 11 = Obstacles
            objd.transform.position = playerController.transform.position;
            painterTrail.Add(objd);

            Debug.Log("new painter trail " + painterTrail.Count);
            timePassed = 0;
        }
    }
}
