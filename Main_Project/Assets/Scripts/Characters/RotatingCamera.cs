using UnityEngine;

public class RotatingCamera : MonoBehaviour
{
    public Transform camPivot;

    private float heading = 0f;

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        heading += Input.GetAxis("Mouse X") * Time.deltaTime * 90f; // Rotates 180° per second 
        camPivot.rotation = Quaternion.Euler(0, heading, 0);
    }  
}
