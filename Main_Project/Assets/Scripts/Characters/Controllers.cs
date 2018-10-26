using UnityEngine;


public static class Controllers {
    public  enum ControllerType{
        PS4,KEYBOARD
    }

    public static void CheckInput() {
        float dPadX = Input.GetAxis("PS4_DPad_X");
        float dPadY = Input.GetAxis("PS4_DPad_Y");

        float rStickX = Input.GetAxis("PS4_RStick_X");
        float rStickY = Input.GetAxis("PS4_RStick_Y");

        if (rStickX > 0)
            Debug.Log("positive rStickX: " + rStickX);
        if (rStickX < 0)
            Debug.Log("negative rStickX: " + rStickX);
        if (rStickY > 0)
            Debug.Log("positive rStickY: " + rStickY);
        if (rStickY < 0)
            Debug.Log("negative rStickY: " + rStickY);
        //Front Left
        if (dPadX > 0)
            Debug.Log("DPad: Right");
        if (dPadX < 0)
            Debug.Log("DPad: Left");
        if (dPadY > 0)
            Debug.Log("DPad: Up");
        if (dPadY < 0)
            Debug.Log("DPad: Down");
        if (Input.GetButtonDown("PS4_Button_LStickClick"))
            Debug.Log("Input: LStickClick");

        //Front Right
        if (Input.GetButtonDown("PS4_Button_Square"))
            Debug.Log("Input: Square");
        if (Input.GetButtonDown("PS4_Button_X"))
            Debug.Log("Input: X");
        if (Input.GetButtonDown("PS4_Button_O"))
            Debug.Log("Input: O");
        if (Input.GetButtonDown("PS4_Button_Triangle"))
            Debug.Log("Input: Triangle");
        if (Input.GetButtonDown("PS4_Button_RStickClick"))
            Debug.Log("Input: RStickClick");

        //Back
        if (Input.GetButtonDown("PS4_L1"))
            Debug.Log("Input: L1");
        if (Input.GetButtonDown("PS4_R1"))
            Debug.Log("Input: R1");
        if (Input.GetButtonDown("PS4_L2"))
            Debug.Log("Input: L2");
        if (Input.GetButtonDown("PS4_R2"))
            Debug.Log("Input: R2");

        //Options
        if (Input.GetButtonDown("PS4_Button_SHARE"))
            Debug.Log("Input: SHARE");
        if (Input.GetButtonDown("PS4_Button_OPTIONS"))
            Debug.Log("Input: OPTIONS");

    }

}
