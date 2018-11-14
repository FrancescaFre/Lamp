using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    [Range(30,50)]
    public float angleX = 35;
    [Range(2,6)]
    public float rotationSpeed = 4f;
    public GameObject player;
   
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Rotation Part
        //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up) * offset;
        //transform.LookAt(player.transform.position);

        transform.position = player.transform.position + offset;       
    }

    
}
