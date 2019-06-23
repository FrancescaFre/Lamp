using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    public GravityAttractor attractor;
    private Rigidbody rb;

    void Start()
    {
       
        attractor = BasicCamera.instance.planet.GetComponent<GravityAttractor>();

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
    }


    void FixedUpdate()
    {
        attractor.Attract(rb);
    }
        
}