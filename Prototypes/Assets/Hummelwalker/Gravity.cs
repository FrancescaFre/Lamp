using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

    float gravityforce = -20;

    public void ApplyGravity(Transform receiver)
    {
        Rigidbody rb = receiver.GetComponent<Rigidbody>();
        Vector3 forceUp = receiver.position - transform.position;
        Vector3 dir;
        dir = gravityforce * forceUp.normalized;

        rb.AddForce(dir);
        Vector3 receiverUp = receiver.up;

        Quaternion rot = Quaternion.FromToRotation(receiverUp, forceUp) * receiver.rotation;

        receiver.rotation = rot;
    }
}
