using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public static GravityAttractor instance;
    public float gravity = -10f;

    private void Awake() {
        BasicCamera.instance.planet = transform;

    }

    public void Attract(Rigidbody bodyTransform)
    {
        Vector3 gravityUp = (bodyTransform.position - transform.position).normalized;
        Vector3 bodyUp = bodyTransform.transform.up;

        bodyTransform.AddForce(gravityUp * gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * bodyTransform.rotation;
        bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, targetRotation, 50 * Time.deltaTime);
    }
}