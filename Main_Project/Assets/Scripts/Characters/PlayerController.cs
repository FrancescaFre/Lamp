using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ItemWheel))]
public class PlayerController : MonoBehaviour {
    [Range(5,10)]
    public float walkSpeed = 8f;
    [Range(10,15)]
    public float runSpeed = 10f;
    [Range(1,5)]
    public float stealthSpeed = 4f;
    public bool isSafe { get; private set; }
    public Status curseStatus { get; private set; }
    public Visibility visible { get; private set; }
    public Dictionary<string, int> items;




    private Rigidbody _rig;

    void Awake() {
        isSafe = false;
        curseStatus = Status.NORMAL;
        visible = Visibility.INVISIBLE;
        items = new Dictionary<string, int>(6);
    }

    // Use this for initialization
    void Start () {
        _rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        this.CheckMovement();
        this.CheckSkillInteraction();

    }
    /// <summary>
    /// Moves the player if an input is detected
    /// </summary>
    private void CheckMovement() {
        //to move the player
        float horiz_axis = Input.GetAxis("Horizontal");
        float vert_axis = Input.GetAxis("Vertical");
        Vector3 movement=Vector3.zero;
        
        if((Input.GetButton("PS4_R2") || Input.GetKey(KeyCode.R))&& (horiz_axis != 0 || vert_axis != 0)) {
            //if is holding down a button and moving use the running animation and speed
            movement= transform.TransformDirection(new Vector3(horiz_axis, 0, vert_axis) * runSpeed * Time.deltaTime);
            Debug.Log("RUN");
        }else if ((Input.GetButton("PS4_L2") || Input.GetKey(KeyCode.T))&& (horiz_axis != 0 || vert_axis != 0)) {
            //if is holding down a button and moving use the stealth animation and speed
            movement= transform.TransformDirection(new Vector3(horiz_axis, 0, vert_axis) * stealthSpeed * Time.deltaTime);
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
    /// Change the state of the curse of the character
    /// </summary>
    /// <param name="stat">New state of the curse (ENUM)</param>
    public void ChangeStatus(Status stat) {
        this.curseStatus = stat;
    }

    /// <summary>
    /// Change the visibility of the character
    /// </summary>
    /// <param name="vis">New visibility of the character\</param>
    public void ChangeVisibility(Visibility vis) {
        this.visible = vis;
    }
    /// <summary>
    /// Sets the opposite of the current value of safety
    /// </summary>
    public void ChangeSafety() {
        isSafe = !isSafe;
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
}

public enum Status { NORMAL=0, HALF_CURSED, CURSED }
public enum Visibility { INVISIBLE=0 , WARNING, SPOTTED}
