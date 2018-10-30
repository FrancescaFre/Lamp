using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigTarget : MonoBehaviour
{

    public float speed = 5;
    Rigidbody rb;

    public Material DigYes;
    public Material DigNo;

    public bool isDigging = false; // Allows/Denies the circle to move like a playable character

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Moves the character from point A (the character) to point B (the targeted zone)
    /// </summary>
    /// <returns>The final transform position</returns>
    public Vector3 Dig()
    {
        Debug.Log("Scavo 2!");
        return transform.position;
    }

    /// <summary>
    /// Changes the color of the target digging circle
    /// </summary>
    public void CheckTarget()
    {
        if (CanDig())
            GetComponent<MeshRenderer>().material = DigYes;
        else
            GetComponent<MeshRenderer>().material = DigNo;
    }

    /// <summary>
    /// Hides the target digging circle
    /// </summary>
    /// <param name="startingPosition"></param>
    public void StopTarget(Vector3 startingPosition)
    {
        transform.position = startingPosition;
        gameObject.SetActive(false);
        isDigging = false;
    }

    /// <summary>
    /// Checks if the targeted terrain is diggable
    /// </summary>
    /// <returns>True if the player can dig out of the target terrain</returns>
    public bool CanDig()
    {
        // TODO: check terrain ↓
        return transform.position.z <= 0;
    }

    // Normal movement constrained by the flag. The same goes for the character movement
    void FixedUpdate()
    {
        if (isDigging)
        {
            CheckTarget();
            Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            rb.MovePosition(rb.position + transform.TransformDirection(dir) * speed * Time.deltaTime);
        }
    }

}
