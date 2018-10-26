using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ItemWheel))]
public class CharacterControllerM : MonoBehaviour {
    public float movementSpeed = 18f;
    public Status curseStatus { get; private set; };


    private Rigidbody _rig;

    void Awake() {
        curseStatus = Status.NORMAL;  
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

        Vector3 movement = transform.TransformDirection(new Vector3(horiz_axis, 0, vert_axis) * movementSpeed * Time.deltaTime);

        _rig.MovePosition(transform.position + movement);
    }
    
    /// <summary>
    /// The skill is used if an input is detected
    /// </summary>
    private  void CheckSkillInteraction() {
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
}

public enum Status { NORMAL=0, HALF_CURSED, CURSED }