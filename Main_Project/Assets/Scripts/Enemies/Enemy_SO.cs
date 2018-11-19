using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Enemy_SO : ScriptableObject {

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
    public float stop_search_after_x_seconds;
    public EnemyStatus enemy_initial_status;

}
