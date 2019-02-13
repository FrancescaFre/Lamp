using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class EnemyFOV : MonoBehaviour {

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;


   /// [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    public List<Transform> earedTargets = new List<Transform>();
    
    public Collider[] targetsInViewRadius;
    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;

    public float maskCutawayDst = .1f;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    public Enemy parentEnemy;
    private SphereCollider colliderEar;

    public ParticleSystem hearingParticle;

    public float WaitingTime=.2f;
    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        //StartCoroutine("FindTargetsWithDelay", WaitingTime);

        Timing.RunCoroutine(FindTargetsWithDelay(WaitingTime));

        foreach (ParticleSystem ps in transform.GetComponentsInChildren<ParticleSystem>())
            if (ps.CompareTag(Tags.HearingAreaEnemy))
                hearingParticle = ps;
    
    }

   /* IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }*/
    IEnumerator<float> FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return Timing.WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void OnTriggerStay(Collider other)
    {//se il player entra da sneeky non viene cathcato dal ontrigger enter, ma se una volta dentro non è più sneaky non verrebbe più visto, quindi uso on stay


        //se diventa safe quando è ancora dentro la visione, è giusto che si corregga
        if (other.CompareTag(Tags.Player) && other.transform.GetComponent<PlayerController>().IsSafe && earedTargets.Contains(other.transform))
        {
            earedTargets.Remove(other.transform);
        }

        //se il player non è in sneaky e non è contenuto nella lista, allora gtfo
        if (other.CompareTag(Tags.Player) && !other.transform.GetComponent<PlayerController>().isSneaking && !other.transform.GetComponent<PlayerController>().IsSafe && !earedTargets.Contains(other.transform))
        {
            earedTargets.Add(other.transform);
            
        }
    }


    private void OnTriggerExit(Collider other)
    {//se non contiene quella transform vuol dire che il player è entrato e uscito come sneaky
        if (other.CompareTag(Tags.Player) && earedTargets.Contains(other.transform))
        {
            earedTargets.Remove(other.transform);
            
        }
    }

 /* void LateUpdate()
    {
        DrawFieldOfView();
    }
*/
    /// <summary>
    /// set parameters of FOV, first the radius (how long can see) and angle (how wide is the field of view)
    ///i don't use a property because with one call i can assign 2 values. 
    ///This is needed because my values (radius and angle) change during the game (Influenced by AI) 
    /// </summary>
    public void FOVSetParameters(float radius, float angle, Transform parentEnemy) {
        viewRadius = radius;
        viewAngle = angle;

        //radius is 1/scale of the parent * how is big the fov
        colliderEar = GetComponent<SphereCollider>();
        colliderEar.radius = (1 / parentEnemy.transform.localScale.x)* viewRadius;
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        earedTargets.Clear();

        targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);        

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            earedTargets.Add(target);

            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                //if i hit an obstacle = TRUE, don't add this target else add it
                Debug.DrawRay(transform.position, dirToTarget, Color.blue);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, LayerMask.GetMask("Obstacle")))
                {
                    visibleTargets.Add(target);
                }
            }
        }

    }
    #region DRAW MESH
    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            Quaternion angleQ = this.transform.localRotation;

            float angle = -viewAngle / 2 + stepAngleSize * i;
           // Debug.DrawLine(this.transform.position, transform.position + transform.TransformDirection(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)) * viewRadius, Color.blue);
           
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }
            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            float angle = -viewAngle / 2 + stepAngleSize * i;
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.forward * maskCutawayDst;
           
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }


    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        //Vector3 dir = DirFromAngle(globalAngle, true);
        Vector3 dir = transform.TransformDirection(Mathf.Sin(globalAngle * Mathf.Deg2Rad), 0, Mathf.Cos(globalAngle * Mathf.Deg2Rad));
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
    #endregion
}
