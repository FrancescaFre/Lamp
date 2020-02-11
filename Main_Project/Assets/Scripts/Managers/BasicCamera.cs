using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class BasicCamera : MonoBehaviour {
    public static BasicCamera instance;

    public float cameraSpeed = 5f, smooth_speed = 0.125f;
    public Transform target, planet;
    public Vector3 offset;


    private Vector3 object_offset = new Vector3(0, 3, -3);

    private Quaternion camTurn;
    private float input, angle = 0f, zoom_factor = 1f, offset_look =0.8f;
    private Vector3 desiredPos, temp_right, temp_up, close_offset = new Vector3(0, 3, -3), smooth_pos;

    public float max_zoom = 1.5f, min_zoom = 0.5f;

    public bool is_character;
    
    //----------
    public void ChangeTarget(Transform tgt)
    {
        target = tgt;
        is_character = tgt.CompareTag(Tags.Player); 
    }


    public Vector3 getDesiredPos() {
        return desiredPos;
    }

    ///PostProcessing
    [Tooltip("Change the vignette smoothness when a character is half-cursed.")]
    [Header("Post Processing")]
    [Range(0f, 1f)]
    public float normalVignetteSmoothness = .2f;

    [Range(0f, 1f)]
    public float curseVignetteSmoothness = 1f;
    private PostProcessingProfile _PPProfile;

    private void Awake() {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }


    private void Start() {
        //transform.position = target.position + target.TransformVector(offset * zoom_factor);

        ///PostProcessing
        _PPProfile = GetComponent<PostProcessingBehaviour>().profile;
        ChangeVignetteSmoothness();
    }

    private void FixedUpdate() {
        temp_up = (target.position - planet.position).normalized;
        temp_right = Vector3.Cross(temp_up, target.forward).normalized;// TODO check if needed

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            zoom_factor = Mathf.Clamp(zoom_factor - Input.GetAxis("Mouse ScrollWheel"), min_zoom, max_zoom);

        input = Input.GetAxis(Controllers.PS4_RStick_X) + Input.GetAxis("Mouse X");
        angle += input * cameraSpeed;
        camTurn = Quaternion.AngleAxis(angle, temp_up);

        Vector3 direction = (transform.position - target.position).normalized;

        Debug.Log(target.name);
        //---- zoom over the player 
        if (is_character && zoom_factor <= 0.5)
        {
            desiredPos = target.position + camTurn * target.TransformVector(close_offset * zoom_factor);
            Vector3 smooth_pos = Vector3.Lerp(transform.position, desiredPos, smooth_speed);
            transform.position = smooth_pos;
            transform.LookAt(target.position + target.up * offset_look, temp_up);
        }
        //---- regular movement of the camera 
        else if (is_character && zoom_factor > 0.5)
        {
            transform.position = target.position + camTurn * target.TransformVector(offset * zoom_factor);

            transform.LookAt(target, temp_up);
        }
        //---- following object
        else if (!is_character) {
            desiredPos = target.position + target.TransformVector(object_offset);
            Vector3 smooth_pos = Vector3.Slerp(transform.position, desiredPos, smooth_speed);
            transform.position = smooth_pos;
            transform.LookAt(target.position, temp_up);
        }
    }

    #region Post Processing

    /// <summary>
    /// Changes the vignette smoothness
    /// </summary>
    /// <param name="halfCurse">Is the player half-cursed? (Default: false)</param>
    public void ChangeVignetteSmoothness(bool halfCurse = false) {
        VignetteModel.Settings cameraVignette = _PPProfile.vignette.settings;
        if (halfCurse)
            cameraVignette.smoothness = curseVignetteSmoothness;
        else
            cameraVignette.smoothness = normalVignetteSmoothness;
        _PPProfile.vignette.settings = cameraVignette;
    }

  //  public void FAperture
    #endregion Post Processing
}