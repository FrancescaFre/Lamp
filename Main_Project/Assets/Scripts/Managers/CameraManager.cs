using UnityEngine;

public class CameraManager : MonoBehaviour {//http://youttu.be/MFQhpwc6cKE
    public Transform playerModel;// the model of the player, NOT the pivot

    [Range(0,1)]
    public float smoothSpeed=.125f;   //the higher it is, the faster thecamera will lock on the player

    public Vector3 offset;



    void FixedUpdate() {
        Vector3 desiredPosition = playerModel.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed/*Time.deltaTime*/);
        transform.position = smoothedPosition;

        transform.LookAt(playerModel);
    }

}
