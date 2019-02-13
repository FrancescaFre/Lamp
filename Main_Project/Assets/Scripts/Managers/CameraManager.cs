using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraManager : MonoBehaviour {

    [Tooltip("Minimum distance from the player when zooming in")]
    [Range(1,3)]
    public float nearZoom = 1;

    [Tooltip("Maximum distance from the player when zooming out")]
    [Range(4,8)]
    public float farZoom = 5;

    [Tooltip("How fast are min and max reached")]
    [Range(.05f, .20f)]
    public float sensitivity = .10f;

    [Tooltip("The speed at which the camera revolves around the player")]
    [Range(0f,200f)]
    public float cameraSpeed = 90f;

    private Transform _dummyCam; // The dummy camera is used to avoid 3-dimensional inverse revolutions
    private Vector3 _camOffset; // Difference in position between the main camera and the dummy one
    private Transform _realDummyCam; // Used when switching to zone dig
    private float yPosition = 0; // Position on Y of the rotating camera (starts from zero)

    private float _input;

    ///PostProcessing
    [Tooltip("Change the vignette smoothness when a character is half-cursed.")]
    [Header("Post Processing")]
    [Range(0f,1f)]
    public float normalVignetteSmoothness = .2f;
    [Range(0f, 1f)]
    public float curseVignetteSmoothness = 1f;
    private PostProcessingProfile _PPProfile;

    // Saves the distance between the original and the dummy camera
    void Start () {
        _dummyCam = GameObject.FindGameObjectWithTag(Tags.DummyCam).transform;
        AlignCameras(FindObjectOfType<PlayerController>().transform);
        _camOffset = transform.position - _dummyCam.position;

        ///PostProcessing
        _PPProfile = GetComponent<PostProcessingBehaviour>().profile;
    }

    // Rotates the camera as the right analog stick is pressed
    private void FixedUpdate()
    {
        yPosition += _input * Time.deltaTime * cameraSpeed; 
       // yPosition += Input.GetAxis("Mouse X") * Time.deltaTime * cameraSpeed; // TESTING
        // yPosition += Input.GetAxis("PS4_RStick_X") * Time.deltaTime * cameraSpeed; // Real one with the joypad analog stick
        _dummyCam.parent.localRotation = Quaternion.Euler(_dummyCam.parent.localRotation.x, yPosition, _dummyCam.parent.localRotation.z);

        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0 || Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.K)) // [ZOOM keys]
            CheckZoom();
    }

    // Every frame takes the same position as the dummy camera, in relation to the player
    private void LateUpdate()
    {
        _input = Input.GetAxis(Controllers.PS4_RStick_X) + Input.GetAxis("Mouse X");      //if one is zero the other one will not
        transform.SetPositionAndRotation(_dummyCam.position + _camOffset, _dummyCam.rotation); 
    }

    private void CheckZoom()
    {
        if (GameManager.Instance.currentPC.IsZoneDigging) return; // No zoom while zone digging

        if ((Input.GetAxisRaw("Mouse ScrollWheel") > 0 && Vector3.Distance(transform.position, GameManager.Instance.currentPC.transform.position) > nearZoom) ||
            (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && Vector3.Distance(transform.position, GameManager.Instance.currentPC.transform.position) < farZoom))
            _dummyCam.transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel"), Space.Self);

        if (Input.GetKey(KeyCode.J) && Vector3.Distance(transform.position, GameManager.Instance.currentPC.transform.position) > nearZoom)
            _dummyCam.transform.Translate(0, 0, sensitivity, Space.Self);
        else if (Input.GetKey(KeyCode.K) && Vector3.Distance(transform.position, GameManager.Instance.currentPC.transform.position) < farZoom)
            _dummyCam.transform.Translate(0, 0, -sensitivity, Space.Self);
    }

    private void AlignCameras(Transform subjectToAlign)
    {
        transform.SetParent(subjectToAlign);
        //transform.SetParent(GameManager.Instance.currentPC.transform);
        if (_dummyCam.localPosition != transform.localPosition || _dummyCam.localRotation != transform.localRotation)
        {
            _dummyCam.localPosition = transform.localPosition;
            _dummyCam.localRotation = transform.localRotation;
        }
        transform.SetParent(null);
    }

    /// <summary>
    /// Detaches the camera from the player to follow the zone digging moving circle
    /// </summary>
    public void DetachForZoneDig(Transform newDummyCam)
    {
        _realDummyCam = _dummyCam;
        _dummyCam = newDummyCam;
        AlignCameras(FindObjectOfType<MovingCircle>().transform);
    }

    /// <summary>
    /// Detaches the camera from the player to follow the zone digging moving circle
    /// </summary>
    public void RestoreDummyCam()
    {
        _dummyCam = _realDummyCam;
    }

    #region Post Processing

    /// <summary>
    /// Changes the vignette smoothness
    /// </summary>
    /// <param name="halfCurse">Is the player half-cursed? (Default: false)</param>
    public void ChangeVignetteSmoothness(bool halfCurse = false) {
        VignetteModel.Settings cameraVignette = _PPProfile.vignette.settings;
        if(halfCurse)
            cameraVignette.smoothness = curseVignetteSmoothness;
        else
            cameraVignette.smoothness = normalVignetteSmoothness;
        _PPProfile.vignette.settings = cameraVignette;

    }
    
    #endregion
}
