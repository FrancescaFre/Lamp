using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityAttract : MonoBehaviour {

    public float gravity = -10f;

    public void Attract(Rigidbody bodyTransform) {
        Vector3 gravityUp = (bodyTransform.position - transform.position).normalized;
        Vector3 bodyUp = bodyTransform.transform.up;

        bodyTransform.AddForce(gravityUp * gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * bodyTransform.rotation; 
        bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, targetRotation, 50 * Time.deltaTime);
    }
}
