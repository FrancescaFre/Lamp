using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCamera : MonoBehaviour
{
    public static BasicCamera instance;

    public float cameraSpeed = 5f, smooth_speed = 0.125f;
    public Transform player, planet;
    public Vector3 offset;

    private Quaternion camTurn;
    private float input, angle = 0f, zoom_factor = 1f, offset_look = 0.15f;
    private Vector3 desiredPos, temp_right, temp_up, close_offset=new Vector3(0,1,-2), smooth_pos;


    public float max_zoom = 1.5f, min_zoom = 0.5f;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        temp_up = (player.position - planet.position).normalized;
        temp_right = Vector3.Cross(temp_up, player.forward).normalized;

        if(Input.GetAxis("Mouse ScrollWheel")!=0)
            zoom_factor = Mathf.Clamp(zoom_factor - Input.GetAxis("Mouse ScrollWheel"), min_zoom, max_zoom);

        input = Input.GetAxis(Controllers.PS4_RStick_X) + Input.GetAxis("Mouse X");
        angle += input * cameraSpeed;
        camTurn = Quaternion.AngleAxis(angle, temp_up);

        Vector3 direction = (transform.position - player.position).normalized;
        transform.position = player.position + camTurn * player.TransformVector(offset * zoom_factor);

        if (zoom_factor <= 0.5)
        {
            desiredPos = player.position + camTurn * player.TransformVector(close_offset * zoom_factor);
            Vector3 smooth_pos = Vector3.Lerp(transform.position, desiredPos, smooth_speed);
            transform.position = smooth_pos;
            transform.LookAt(player.position + player.up * offset_look, temp_up);
        }
        else
        {
            transform.position = player.position + camTurn * player.TransformVector(offset * zoom_factor);
            transform.LookAt(player, temp_up);
        }
    }

}