using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //needed for NavMeshAgent


public enum EnemyStatus { WANDERING = 0, SEEKING, SEARCHING, RETURN}

public class Enemy : MonoBehaviour
{

    public Enemy_SO data_enemy;

    //---Level details
    int level;
    public bool instant_curse;

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
    float stop_search_after_x_seconds;
    public Transform player;

    public Vector3 destination;
    public int pathIndex;

    public EnemyStatus currentStatus;
    private float timePassed;

    private Queue<Vector3> hanselGretelGPS;
    private int pathIndexReturn;

    private Vector3 lastPlayerPosition;


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
        ChangeStatus();
        ChangeDirection();
        
        if (Vector3.Distance(destination, transform.position) > 0.3f)
        {
            direction = (destination - transform.position).normalized;
            //add adjust for floking
            velocity = direction * speed * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, direction.magnitude);
            rb.MovePosition(rb.position + velocity);
        }

        //THANKS Mister ANTONIO
        dirY = Vector3.ProjectOnPlane(destination - this.transform.position, transform.up);
        //Debug.DrawLine(this.transform.position, this.transform.position + dirY * 10f, Color.red);
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
    }

    private void ChangeStatus() {
        //1- if the enemy is wandering and it see an enemy and this is not safe--> seeking the player (player)
        if (currentStatus == EnemyStatus.WANDERING && fov.visibleTargets.Count > 0 && !fov.visibleTargets[0].GetComponent<PlayerController>().IsSafe) {
            Debug.Log("WANDERING TO SEEKING");
            player = fov.visibleTargets[0];
            currentStatus = EnemyStatus.SEEKING;
        }

        //2- if the player isn't reachable by any rayCast, stop and start to Search
        if (currentStatus == EnemyStatus.SEEKING){
            Debug.Log("SEEKING TO SEARCHING");
            Vector3 dirToTarget = (player.position - transform.position).normalized;
            float dstToTarget = Vector3.Distance(transform.position, player.position);
            //if the player is safe or the raycast can't reach the player (cause some obstacles), stop seek
            if ( player.GetComponent<PlayerController>().IsSafe || !Physics.Raycast(transform.position, dirToTarget, dstToTarget, fov.targetMask))
            {
                player = null; ///WARNING!
                currentStatus = EnemyStatus.SEARCHING;
                timePassed = 0;
            }
        }

        //3- if the player isn't visible for an ammount of time, stop looking for it
        if (currentStatus == EnemyStatus.SEARCHING && timePassed < stop_search_after_x_seconds)
        {
            Debug.Log("SEARCHING TO RETURN");
            timePassed += Time.deltaTime;
        }
        else
        {
            currentStatus = EnemyStatus.RETURN;
            //hanselGretelGPS.Reverse();
            //pathIndexReturn = 0;
        }

        //4- if the enemy is correctly returned at the last waypoint
        if (currentStatus == EnemyStatus.RETURN) {
            Debug.Log("RETURN TO WANDERING");
            if (Vector3.Distance(path.GetChild(pathIndex).position, transform.position) > 0.3f) {
                currentStatus = EnemyStatus.WANDERING;
                hanselGretelGPS.Clear();
            }
        }

    }

    private void ChangeDirection() {
        if (currentStatus == EnemyStatus.RETURN)
        {
            //destination -> backtrace of the hansel_gretelGPS
            if (Vector3.Distance(path.GetChild(pathIndex).position, transform.position) > 0.3f)
            {
                //pathIndexReturn = (pathIndexReturn + 1) % hanselGretelGPS.Count;
                //destination = hanselGretelGPS[pathIndex];
                destination = hanselGretelGPS.Count>0 ? hanselGretelGPS.Dequeue() : this.transform.position;
            }
        }

        if (currentStatus == EnemyStatus.SEARCHING) {
            destination = lastPlayerPosition;
            if (Vector3.Distance(destination, transform.position) > 0.3f) {
                //rotate 360 on local y axis
            }
        }

        if (currentStatus == EnemyStatus.SEEKING)
        {
            if (player)
            {
                lastPlayerPosition = player.position;
                destination = player.position;
            }
        }

        if (currentStatus == EnemyStatus.WANDERING) {
            pathIndex = (pathIndex + 1) % wanderPath.Length;
            destination = wanderPath[pathIndex].position;
        }
    }

    private void FixedUpdate() {
        
        while (currentStatus == EnemyStatus.SEEKING) {
            // hanselGretelGPS.Add(this.transform.position); 
            hanselGretelGPS.Enqueue(this.transform.position);

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

