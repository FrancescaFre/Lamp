using UnityEngine;

public class CaveGravityTarget : MonoBehaviour
{

    public CaveGravity gravity;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gravity.ApplyGravity(transform);
    }
}
