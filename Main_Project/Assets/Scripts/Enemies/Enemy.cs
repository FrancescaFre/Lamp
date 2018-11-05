using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //needed for NavMeshAgent


public enum EnemyStatus { WANDERING = 0, SEEKING, SEARCHING }

public class Enemy : MonoBehaviour
{

    public Enemy_SO data_enemy;

    //---Level details
    int level;
    bool instant_curse;

    //---Movement
    public float speed;

    //---Cone of view parameters
    float cov_distance_wander;
    float cov_distance_seek;
    float cov_distance_search;

    float cov_angle_wander;
    float cov_angle_seek;
    float cov_angle_search;

    //---AI
    public Transform[] wanderPath;
    public Transform path;
    public NavMeshAgent agent;
    float stop_search_after_x_seconds;

    public Vector3 destination;
    public int pathIndex;

    public EnemyStatus currentStatus;

    //---Object
    private Rigidbody rb;
    private EnemyFOV fov;
    Quaternion quaternionTo;
    Vector3 direction;
    Vector3 velocity;
    float rotationSpeed = 2f;
    Vector3 dirY;

    public void Start()
    {
        //Initialize with scriptable obeject data
        level = data_enemy.level;
        instant_curse = data_enemy.instant_curse;

        speed = data_enemy.speed;

        cov_distance_wander = data_enemy.cov_distance_wander;
        cov_distance_search = data_enemy.cov_distance_search;
        cov_distance_seek = data_enemy.cov_distance_seek;

        cov_angle_wander = data_enemy.cov_angle_wander;
        cov_angle_search = data_enemy.cov_angle_search;
        cov_angle_seek = data_enemy.cov_angle_seek;

        stop_search_after_x_seconds = data_enemy.stop_search_after_x_seconds;

        currentStatus = data_enemy.enemy_initial_status;

        rb = this.gameObject.GetComponent<Rigidbody>();

        //assign the first set of values to FOV
        fov = this.GetComponent<EnemyFOV>();
        fov.FOVSetParameters(cov_distance_wander, cov_angle_wander);

        pathIndex = 0;
        //destination = wanderPath[pathIndex].position;
        destination = path.GetChild(0).position;
    }

    public void Update()
    {
        if (Vector3.Distance(destination, transform.position) > 1f)
        {
            //  agent.SetDestination(destination); //CON UNA NAVMESH SAREBBE BELLO E SEMPLICE PORCA PUTTANA
            direction = (destination - transform.position).normalized;
            velocity = direction * speed * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, direction.magnitude);
            rb.MovePosition(rb.position + velocity);
        }
        else
        {
            // pathIndex = (pathIndex + 1) % wanderPath.Length;
            pathIndex = (pathIndex + 1) % path.childCount;
            //destination = wanderPath[pathIndex].position;
            destination = path.GetChild(pathIndex).position;
        }

        //THANKS Mister ANTONIO
        dirY = Vector3.ProjectOnPlane(destination - this.transform.position, transform.up);
        Debug.DrawLine(this.transform.position, this.transform.position + dirY * 10f, Color.red);
        transform.LookAt(transform.position + dirY, transform.up);
    }
 

    //when the enemy interact with a something
    private void OnTriggerEnter(Collider other)
    {
        //collide with another enemy
        if (other.CompareTag("Enemy"))
        {
            if (level < other.GetComponent<Enemy>().level)
                if (Random.Range(0, 9) % 3 == 0)
                    path = other.GetComponent<Enemy>().path;
            //start following the path of touched enemy
        }

        //collide with the player
        if (other.CompareTag("Player"))
        {
            if (instant_curse || other.GetComponent<PlayerController>().CurseStatus == Status.HALF_CURSED)
                other.GetComponent<PlayerController>().ChangeStatus(Status.CURSED); //DIE
            else
            {
                if (other.GetComponent<PlayerController>().CurseStatus == Status.NORMAL)
                    other.GetComponent<PlayerController>().ChangeStatus(Status.HALF_CURSED);
            }
        }
    }


    #region GIZMO
    private void OnDrawGizmos()
    {
        if (path.childCount > 0)
        {
            Vector3 startPosition = this.path.GetChild(0).position;
            Vector3 previousPosition = startPosition;

            foreach (Transform waypoint in path)//prendo tutti i figli in teoria
            {
                Gizmos.DrawSphere(waypoint.position, .3f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine(previousPosition, startPosition);
        }
    }
    #endregion
}

