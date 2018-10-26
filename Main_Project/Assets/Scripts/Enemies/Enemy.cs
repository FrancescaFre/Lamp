using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour{
    
    public Enemy_SO data_enemy;

    //---Level details
    public int level;
    public bool instant_curse;

    //---Movement
    public float speed;
    public float distance_from_planet;

    //---Cone of view parameters
    public float cov_distance_wander;
    public float cov_distance_seek;
    public float cov_distance_search;

    public float cov_angle_wander;
    public float cov_angle_seek;
    public float cov_angle_search;

    //---AI

    //---Object
    private Rigidbody rb;

    public void Start()
    {
        //Initialize with scriptable obeject data
        level = data_enemy.level;
        instant_curse = data_enemy.instant_curse;

        speed = data_enemy.speed;
        distance_from_planet = data_enemy.distance_from_planet;

        cov_distance_wander = data_enemy.cov_distance_wander;
        cov_distance_search = data_enemy.cov_distance_search;
        cov_distance_seek = data_enemy.cov_distance_seek;

        cov_angle_wander = data_enemy.cov_angle_wander;
        cov_angle_search = data_enemy.cov_angle_search;
        cov_angle_seek = data_enemy.cov_angle_seek;

        //
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        
    }

    

}

