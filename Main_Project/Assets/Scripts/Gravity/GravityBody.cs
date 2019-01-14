using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    public GravityAttractor attractor;
    private Rigidbody rb;

    void Start()
    {
        if (!attractor)
            if (CompareTag(Tags.DummyPlayer))
                attractor = GameObject.FindGameObjectWithTag(Tags.DummyPlanet).GetComponent<GravityAttractor>();
            else
                attractor = GameObject.FindGameObjectWithTag(Tags.Planet).GetComponent<GravityAttractor>();

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
    }


    void FixedUpdate()
    {
        attractor.Attract(rb);
    }
        
}