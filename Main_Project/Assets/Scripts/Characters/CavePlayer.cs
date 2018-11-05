using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavePlayer : MonoBehaviour
{
    public float speed = 10;
    public float jumpPower = 500;
    bool isJumping = false;

    bool isDigging = false; // Flag to stop player movement with targeted dig
    Dig digType = Dig.NONE; // 0 = Not Digging, 1 = Vertical Dig, 2 = Targeted Dig

    Rigidbody rb;
    DigStarter digStarter;
    DigTarget digTarget;
    Vector3 targetStartingPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        digStarter = GetComponentInChildren<DigStarter>(includeInactive: true);
        digTarget = GetComponentInChildren<DigTarget>(includeInactive: true);
    }

    void Update()
    {
        // Can remove, just for fun
        //if (Input.GetButtonDown("Jump") && !isJumping)
        //    isJumping = true;

        // Vertical Dig (input digType=1)
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDigging)
            if (digType == Dig.LINEAR) // If you already pressed shift
                if (digStarter.CanDig(digType))
                {
                    digStarter.Dig();

                    // After digging
                    digStarter.StopDig(ref digType);
                }
                else
                    digStarter.StopDig(ref digType); // Also resets digType to 0

            else if (digType == Dig.ZONE) // If you press shift after ctrl it cancels the digging action
                digStarter.StopDig(ref digType);
            else
            {
                digType = Dig.LINEAR;
                digStarter.gameObject.SetActive(true);
                digStarter.CheckDig(digType); // Type 1 for vertical dig
            }

        // Targeted Dig (input digType=2)
        if (Input.GetKeyDown(KeyCode.LeftControl))
            if (isDigging) // If you're searching for a target to dig
                if (digTarget.CanDig())
                {
                    digTarget.Dig();

                    // After digging
                    isDigging = false;
                    digTarget.StopTarget(targetStartingPosition);
                    digStarter.StopDig(ref digType);
                }
                else
                {
                    isDigging = false;
                    digTarget.StopTarget(targetStartingPosition);
                }
            else if (digType == Dig.ZONE) // If you already pressed ctrl
                if (digStarter.CanDig(digType))
                {
                    isDigging = true;
                    digTarget.isDigging = true;
                    digTarget.gameObject.SetActive(true);
                    targetStartingPosition = digTarget.transform.position; // Saves the position to restart the target
                    digTarget.CheckTarget();
                }
                else
                    digStarter.StopDig(ref digType);

            else if (digType == Dig.LINEAR) // If you press ctrl after shift it cancels the digging action
                digStarter.StopDig(ref digType);
            else
            {
                digType = Dig.ZONE;
                digStarter.gameObject.SetActive(true);
                digStarter.CheckDig(digType); // Type 2 for vertical dig
            }

        // Walking around with the dig button active
        if (digType != 0)
            digStarter.CheckDig(digType);

    }

    void FixedUpdate()
    {
        if (!isDigging)
        {
            Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            rb.MovePosition(rb.position + transform.TransformDirection(dir) * speed * Time.deltaTime);

            if (isJumping)
            {
                rb.AddForce(transform.up * jumpPower);
                isJumping = false;
            }
        }
    }
}