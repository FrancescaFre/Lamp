using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCamera : MonoBehaviour {
    public static BasicCamera instance;
    public Transform player;
    public Transform planet;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void Awake() {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void FixedUpdate() {
        Vector3 desiredPos, temp_forward, temp_right;

        temp_forward = (planet.position - player.position).normalized;
        temp_right = Vector3.Cross(player.up, temp_forward).normalized;

        desiredPos = player.position;
        desiredPos -= temp_forward * offset.z;
        desiredPos += player.up * offset.y;
        desiredPos += temp_right * offset.x;

        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = smoothedPos;

        transform.LookAt(player, player.up);
    }
}