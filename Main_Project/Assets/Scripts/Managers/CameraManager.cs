using UnityEngine;

public class CameraManager : MonoBehaviour {

    [Tooltip("Minimum field of view reached zooming in")]
    public float minZoom = 15f;

    [Tooltip("Maximum field of view reached zooming out")]
    public float maxZoom = 90f;

    [Tooltip("How fast are min and max reached")]
    [Range(15f, 25f)]
    public float sensitivity = 20f;

    [Tooltip("True to zoom out scrolling up and vice-versa")]
    public bool inverseZoom = false;

    private Transform _dummyCam; // The dummy camera is used to avoid 3-dimensional inverse revolutions
    private Vector3 _camOffset; // Difference in position between the main camera and the dummy one
    private float yPosition = 0; // Position on Y of the rotating camera (starts from zero)

    // Saves the distance between the original and the dummy camera
    void Start () {
        _dummyCam = GameObject.FindGameObjectWithTag("DummyCam").transform;
        AlignCameras();
        _camOffset = transform.position - _dummyCam.position; 
    }

    // Rotates the camera as the right analog stick is pressed
    private void FixedUpdate()
    {
        yPosition += Input.GetAxis("Mouse X") * Time.deltaTime * 180f; // TESTING
        //yPosition += Input.GetAxis("RightStick X") * Time.deltaTime * 90f; // Real one with the joypad analog stick
        _dummyCam.parent.localRotation = Quaternion.Euler(_dummyCam.parent.localRotation.x, yPosition, _dummyCam.parent.localRotation.z);

        float fov = Camera.main.fieldOfView;
        if (inverseZoom)
            fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        else
            fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minZoom, maxZoom);
        Camera.main.fieldOfView = fov;
    }

    // Every frame takes the same position as the dummy camera, in relation to the player
    private void LateUpdate()
    {
        transform.SetPositionAndRotation(_dummyCam.position + _camOffset, _dummyCam.rotation); 
    }

    private void AlignCameras()
    {
        transform.SetParent(FindObjectOfType<PlayerController>().transform);
        if (_dummyCam.localPosition != transform.localPosition || _dummyCam.localRotation != transform.localRotation)
        {
            _dummyCam.localPosition = transform.localPosition;
            _dummyCam.localRotation = transform.localRotation;
        }
        transform.SetParent(null);
    }
}
