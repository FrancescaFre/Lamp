using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //needed for NavMeshAgent


public enum EnemyStatus { WANDERING = 0, SEEKING, SEARCHING, RETURN}

public class Enemy : MonoBehaviour
{

    public Enemy_SO data_enemy;

    //---Level details
    [Header ("Level Details")]
    int level;
    public bool instant_curse;

    //---Movement
    [Header("Movement")]
    public float speed;

    //---Cone of view parameters
    float cov_distance_wander;

    float cov_angle_wander;


    //---AI
    [Header("AI Variables")]
    public GameObject returnWaypointPrefab;
    GameObject objDrop;
    Transform toDestroy;

    float stop_search_after_x_seconds;
    public Transform player;

    public Transform path;
    public Vector3 destination;
    public int pathIndex;
    
    public EnemyStatus currentStatus;
    private float timePassed;
    private float timePassedToDrop;

    private Stack<Transform> hanselGretelGPS;
    public int pathIndexReturn;

    private Vector3 lastPlayerPosition;

    bool teleport = false;

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
        
        cov_angle_wander = data_enemy.cov_angle_wander;
       
        stop_search_after_x_seconds = data_enemy.stop_search_after_x_seconds;

        currentStatus = data_enemy.enemy_initial_status;

        rb = this.gameObject.GetComponent<Rigidbody>();

        //assign the first set of values to FOV
        fov = this.GetComponent<EnemyFOV>();
        fov.FOVSetParameters(cov_distance_wander, cov_angle_wander);

        pathIndex = 0;
        //destination = wanderPath[pathIndex].position;
        destination = path.GetChild(0).position;
        rb.position = destination;
       
        hanselGretelGPS = new Stack<Transform>();
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

        if (other.CompareTag("Player")) {
            //after a collide with a player, the enemy start his returns.
            player = null; ///WARNING!
           
            currentStatus = EnemyStatus.RETURN;
           
            if(hanselGretelGPS.Count>0)
                toDestroy = hanselGretelGPS.Peek();
            destination = hanselGretelGPS.Pop().position;
        }
    }

    private void ChangeStatus() {
        //1- if the enemy is wandering or returning and it see an enemy and this is not safe--> seeking the player (player)
        if ((currentStatus == EnemyStatus.WANDERING || currentStatus == EnemyStatus.RETURN) && fov.visibleTargets.Count > 0 && !fov.visibleTargets[0].GetComponent<PlayerController>().IsSafe)
        {
            player = fov.visibleTargets[0];
            timePassedToDrop = 0f;

            currentStatus = EnemyStatus.SEEKING;
        }

        //2- if the player isn't reachable by any rayCast, stop and start to Search
        else if (currentStatus == EnemyStatus.SEEKING && player)
        {
            DropPosition();

            Vector3 dirToTarget = (player.position - transform.position).normalized;
            float dstToTarget = Vector3.Distance(transform.position, player.position);

            //if the player is safe or the raycast can't reach the player (cause some obstacles), stop seek
            if (player.GetComponent<PlayerController>().IsSafe || !Physics.Raycast(transform.position, dirToTarget, dstToTarget, fov.targetMask))
            {
                player = null; ///WARNING!
                currentStatus = EnemyStatus.SEARCHING;
                timePassed = 0;
            }
        }

        //3- if the player isn't visible for an ammount of time, stop looking for it
        else if (currentStatus == EnemyStatus.SEARCHING && timePassed < stop_search_after_x_seconds && player == null)
        {
            timePassed += Time.deltaTime;
        }
        else if (currentStatus == EnemyStatus.SEARCHING && timePassed >= stop_search_after_x_seconds)
        {
            currentStatus = EnemyStatus.RETURN;

            if (toDestroy)
                DestroyImmediate(toDestroy.gameObject);
            toDestroy = hanselGretelGPS.Peek();
            destination = hanselGretelGPS.Pop().position;
        }

        //4- if the enemy is correctly returned at the last waypoint
        else if (currentStatus == EnemyStatus.RETURN && hanselGretelGPS.Count == 0)
        {
            if (Vector3.Distance(path.GetChild(pathIndex).position, transform.position) > 0.3f)
            {
                currentStatus = EnemyStatus.WANDERING;
                destination = path.GetChild(pathIndex).position;
            }
        }
    }

    private void ChangeDirection() {
        if (currentStatus == EnemyStatus.WANDERING)
        {
            
            if (Vector3.Distance(path.GetChild(pathIndex).position, transform.position) < 0.3f)
            {
                pathIndex = (pathIndex + 1) % path.childCount;
                while (path.GetChild(pathIndex).GetComponent<SingleWaypoint>().underALamp)
                {
                    pathIndex = (pathIndex + 1) % path.childCount;
                    teleport = true;
                }
                if (teleport)
                {
                    //animation for the teleport HERE
                    this.rb.position = path.GetChild(pathIndex).position;
                    destination = path.GetChild(pathIndex).position;
                    teleport = false; 
                }
                else
                    destination = path.GetChild(pathIndex).position;
            }
        }
  
        else if (currentStatus == EnemyStatus.RETURN)
        {
            //destination -> backtrace of the hansel_gretelGPS
            if (Vector3.Distance(destination, transform.position) < 0.3f)
            {
                if (hanselGretelGPS.Count != 0 && hanselGretelGPS.Peek()!=null)
                {
                    while (hanselGretelGPS.Peek().GetComponent<SingleWaypoint>().underALamp)
                    {
                        DestroyImmediate(toDestroy.gameObject);
                        toDestroy = hanselGretelGPS.Peek();
                        teleport = true; 
                    }
                }

                if (teleport)
                {
                    //animation for the teleport HERE
                    this.rb.position = hanselGretelGPS.Pop().position;
                    teleport = false;
                }
                else
                {
                    if (toDestroy)
                        DestroyImmediate(toDestroy.gameObject);
                    toDestroy = hanselGretelGPS.Peek();
                    destination = hanselGretelGPS.Pop().position;
                }
            }
        }

        else if (currentStatus == EnemyStatus.SEARCHING) {
            destination = lastPlayerPosition;
            if (Vector3.Distance(destination, transform.position) > 0.3f) {
                //rotate 360 on local y axis
            }
        }

        else if (currentStatus == EnemyStatus.SEEKING)
        {
            if (player)
            {
                lastPlayerPosition = player.position;
                destination = lastPlayerPosition;
            }
        }
    }

    private void DropPosition() {
        if ((currentStatus == EnemyStatus.SEEKING && player!=null) || currentStatus == EnemyStatus.SEARCHING)
        {
            if (timePassedToDrop < 0.5f) //TEST THE PERFORMANCES
                timePassedToDrop += Time.deltaTime;
            else {
                objDrop = Instantiate(returnWaypointPrefab);
                objDrop.transform.position = this.transform.position;

                hanselGretelGPS.Push(objDrop.transform);

                timePassedToDrop = 0;
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
                Gizmos.DrawSphere(waypoint.position, .1f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine(previousPosition, startPosition);
        }

        if (hanselGretelGPS!=null && hanselGretelGPS.Count > 0)
        {
            // Vector3 startPosition = this.hanselGretelGPS[0].position;
            List<Transform> hans = new List<Transform>(hanselGretelGPS.ToArray());
            Vector3 startPosition = hans[0].position;
            Vector3 previousPosition = startPosition;

            foreach (Transform waypoint in hans) { 

                Gizmos.DrawSphere(waypoint.position, .1f);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine(previousPosition, startPosition);
        }
    }
    #endregion
}

