using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigCam : MonoBehaviour {

    public GameObject pivot;
    private float yPosition = 0; // Position on Y of the rotating camera (starts from zero)
    private float _input;
    private float cameraSpeed = 90f;

    // Rotates the camera as the right analog stick is pressed
    private void FixedUpdate()
    {
        yPosition += _input * Time.deltaTime * cameraSpeed;
        // yPosition += Input.GetAxis("Mouse X") * Time.deltaTime * cameraSpeed; // TESTING
        // yPosition += Input.GetAxis("PS4_RStick_X") * Time.deltaTime * cameraSpeed; // Real one with the joypad analog stick
        pivot.transform.localRotation = Quaternion.Euler(pivot.transform.localRotation.x, yPosition, pivot.transform.localRotation.z);
    }

    // Every frame takes the same position as the dummy camera, in relation to the player
    private void LateUpdate()
    {
        _input = Input.GetAxis(Controllers.PS4_RStick_X) + Input.GetAxis("Mouse X");      //if one is zero the other one will not
    }
}
