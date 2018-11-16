using UnityEngine;

public class RotatingCamera : MonoBehaviour
{
    public Transform camPivot;
    public Transform fakeCamera;

    private float heading = 0f;

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        heading += Input.GetAxis("Mouse X") * Time.deltaTime * 90f; // Rotates 90° per second 
        camPivot.rotation = Quaternion.Euler(fakeCamera.localRotation.x, heading, fakeCamera.localRotation.z);
    }  
}
