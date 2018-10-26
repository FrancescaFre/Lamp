using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterControllerM : MonoBehaviour {
    public float movementSpeed = 18f;
    

    private Rigidbody _rig;
	// Use this for initialization
	void Start () {
        _rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        //to move the player
        float horiz_axis = Input.GetAxis("Horizontal");
        float vert_axis = Input.GetAxis("Vertical");

        Vector3 movement = transform.TransformDirection(new Vector3(horiz_axis,0,vert_axis)*movementSpeed*Time.deltaTime);

        _rig.MovePosition(transform.position+movement);

    }


}
