using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    public GravityAttractor attractor;
    private Rigidbody rb;

    void Start()
    {
        attractor = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttractor>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        attractor.Attract(rb);
    }
}