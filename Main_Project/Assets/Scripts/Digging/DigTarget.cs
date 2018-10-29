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

    // TODO: Targeted Digging
    public void Dig()
    {
        Debug.Log("Scavo 2!");
    }

    public void CheckTarget()
    {
        if (CanDig())
            GetComponent<MeshRenderer>().material = DigYes;
        else
            GetComponent<MeshRenderer>().material = DigNo;
    }

    public void StopTarget(Vector3 startingPosition)
    {
        transform.position = startingPosition;
        gameObject.SetActive(false);
        isDigging = false;
    }

    // Returns true if the target is in a valid terrain to dig
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
