using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Range(0f, 10f)]
    public float frequency = 2f;
    [Range(-1f, 10f)]
    public float speed = 0.3f;

    void Update()
    {
        this.transform.position = transform.position + transform.up * Mathf.Sin(Time.time * frequency) * Time.deltaTime * speed;
    }
}
