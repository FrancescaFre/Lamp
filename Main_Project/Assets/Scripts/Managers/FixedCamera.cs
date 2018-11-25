using UnityEngine;

public class FixedCamera : MonoBehaviour {

    [Tooltip("The dummy player's camera transform (must be dummy's child)")]
    public Transform dummyCam; // The dummy camera is used to avoid 3-dimensional inverse revolutions

    [Tooltip("Attach it to the real player")]
    public PlayerController player;

    private Vector3 _camOffset; // Difference in position between the main camera and the dummy one
    private float yPosition = 0; // Position on Y of the rotating camera (starts from zero)

    // Saves the distance between the original and the dummy camera
    void Start () {
        _camOffset = transform.position - dummyCam.position;
    }
   
    // Rotates the camera as the right analog stick is pressed
    private void FixedUpdate()
    {
        yPosition += Input.GetAxis("Mouse X") * Time.deltaTime * 180f; // TESTING
        //yPosition += Input.GetAxis("RightStick X") * Time.deltaTime * 90f; // Real one
        dummyCam.parent.localRotation = Quaternion.Euler(dummyCam.parent.localRotation.x, yPosition, dummyCam.parent.localRotation.z);
    }

    // Every frame takes the same position as the dummy camera, in relation to the player
    private void LateUpdate()
    {
        transform.SetPositionAndRotation(dummyCam.position + _camOffset, dummyCam.rotation); 
    }
}
