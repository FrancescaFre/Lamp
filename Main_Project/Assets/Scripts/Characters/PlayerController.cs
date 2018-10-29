using System.Collections.Generic;
using System;
using UnityEngine;

public enum Status { NORMAL = 0, HALF_CURSED, CURSED }
public enum Visibility { INVISIBLE = 0, WARNING, SPOTTED }

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ItemWheel))]
public class PlayerController : MonoBehaviour {
    [Range(5, 10)]
    public float walkSpeed = 8f;
    [Range(10, 15)]
    public float runSpeed = 10f;
    [Range(1, 5)]
    public float stealthSpeed = 4f;


    public bool IsSafe { get; private set; }
    public Status CurseStatus { get; private set; }
    public Visibility Visible { get; private set; }
    public Dictionary<string, int> items;

    public GameObject CameraGO;



    private Rigidbody _rig;
    private CameraManager _camera;

    void Awake() {
        IsSafe = false;
        CurseStatus = Status.NORMAL;
        Visible = Visibility.INVISIBLE;
        items = new Dictionary<string, int>(6);

    }

    // Use this for initialization
    void Start() {
        _rig = GetComponent<Rigidbody>();
        _camera = CameraGO.GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update() {
        this.CheckMovement();
        this.CheckSkillInteraction();
        this.CheckCamera();
        Debug.Log("PLAYER IS: "+IsSafe);
    }

    /// <summary>
    /// Moves the player if an input is detected
    /// </summary>
    private void CheckMovement() {
        //to move the player
        float horiz_axis = Input.GetAxis("Horizontal");
        float vert_axis = Input.GetAxis("Vertical");
        Vector3 movement = Vector3.zero;

        if ((Input.GetButton("PS4_R2") || Input.GetKey(KeyCode.R)) && (horiz_axis != 0 || vert_axis != 0)) {
            //if is holding down a button and moving use the running animation and speed
            movement = transform.TransformDirection(new Vector3(horiz_axis, 0, vert_axis) * runSpeed * Time.deltaTime);
            Debug.Log("RUN");
        } else if ((Input.GetButton("PS4_L2") || Input.GetKey(KeyCode.T)) && (horiz_axis != 0 || vert_axis != 0)) {
            //if is holding down a button and moving use the stealth animation and speed
            movement = transform.TransformDirection(new Vector3(horiz_axis, 0, vert_axis) * stealthSpeed * Time.deltaTime);
            Debug.Log("STEALTH");
        }
        else if (horiz_axis != 0 || vert_axis != 0) {
            //if only moving use walk animation and speed
            movement = transform.TransformDirection(new Vector3(horiz_axis, 0, vert_axis) * walkSpeed * Time.deltaTime);
            Debug.Log("WALK");
        }
        _rig.MovePosition(transform.position + movement);


    }

    /// <summary>
    /// The skill is used if an input is detected
    /// </summary>
    private void CheckSkillInteraction() {
        if (Input.GetButtonDown("PS4_Button_O") || Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log("SKILL used");
        }
    }
    /// <summary>
    /// If a button is clicked, enables/disables the control of the camera around the player
    /// </summary>
    private void CheckCamera(){
        if (Input.GetButtonDown("PS4_Button_RStickClick") || Input.GetKeyDown(KeyCode.Tab)) {
            Debug.Log("before " + _camera.IsFollowingPlayer);
            _camera.SetCamera();
            _camera.ResetPlanetView();
            Debug.Log("after "+_camera.IsFollowingPlayer);
        }
        if (!_camera.IsFollowingPlayer) {// if the player is not followed by the camera
            float rStickX = Input.GetAxis("PS4_RStick_X");
            float rStickY = Input.GetAxis("PS4_RStick_Y");

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Debug.Log("move camera");
            if ((rStickX != 0 || rStickY != 0) && (mouseX == 0 && mouseY == 0)) {// if only the controller is used
             
                _camera.LookAtPlanet(rStickX, rStickY);
            }
            else {
                _camera.LookAtPlanet(mouseX, mouseY);
            }
        }

    }

    /// <summary>
    /// Change the state of the curse of the character
    /// </summary>
    /// <param name="stat">New state of the curse (ENUM)</param>
    public void ChangeStatus(Status stat) {
        this.CurseStatus = stat;
    }

    /// <summary>
    /// Change the visibility of the character
    /// </summary>
    /// <param name="vis">New visibility of the character\</param>
    public void ChangeVisibility(Visibility vis) {
        this.Visible = vis;
    }
    /// <summary>
    /// Sets the opposite of the current value of safety
    /// </summary>
    public void ChangeSafety() {
        IsSafe = !IsSafe;
        Debug.Log("chiamato");
    }
    /// <summary>
    /// Use an item from the inventory if any
    /// </summary>
    /// <param name="itemKey">Item to be used</param>
    public void UseItem(string itemKey) {
        if (items[itemKey] > 0) {
            ManageItem(itemKey, -1);
            Debug.Log("used item " + itemKey);  //TODO: add use of the item
        }
        else {
            Debug.Log("not enough item " + itemKey);
        }
    }
    /// <summary>
    /// Manage an item in the inventory
    /// </summary>
    /// <param name="itemKey">Managed item</param>
    /// <param name="use">Plus 1 if gathered; Minus 1 if used</param>
    public void ManageItem(string itemKey, int use) {
        items[itemKey] += use;
    }

    #region Collision Detection

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Lamp_Base")|| other.CompareTag("Lamp_Switch")) {//if the character has entered the light of a lamp that is switched on
            IsSafe = true;
            if (other.CompareTag("Lamp_Switch")) {
                other.GetComponent<LampBehaviour>().SwitchOnLamp();
                Debug.Log("lamp_switch: ON");
            }
            //this.ChangeSafety();
            
            
        }

    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Lamp_Base") ) {//if the character has entered the light of a lamp that is switched on

            IsSafe = false;
            
        }
    }


    #endregion
}


