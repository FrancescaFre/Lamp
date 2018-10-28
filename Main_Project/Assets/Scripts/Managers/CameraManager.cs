using UnityEngine;

public class CameraManager : MonoBehaviour {//http://youtu.be/MFQhpwc6cKE
    public Transform playerModel;// the model of the player, NOT the pivot
    public Transform planetModel;
    public bool IsFollowingPlayer { get; private set; }

    [Range(10,15)]
    public float smoothSpeed=10.25f;   //the higher it is, the faster the camera will lock on the player

    public Vector3 playerOffset;
    [Range(3,30)]
    public float planetOffset_Z;

    private float currentX = 3f;
    private float currentY = 3f;

    void Awake() {
        IsFollowingPlayer = true;
    }


    void FixedUpdate() {
        if (IsFollowingPlayer) {
            Vector3 desiredPosition = playerModel.position + playerOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
            Debug.Log("follow player");
            transform.LookAt(playerModel);
            return;
        }

        AroundTarget(planetModel);
    }
    /// <summary>
    /// Moves the camera around the planet
    /// </summary>
    /// <param name="X_axis">Movement on X axis</param>
    /// <param name="Y_axis">Movement on Y axis</param>
    public void LookAtPlanet(float X_axis, float Y_axis) {
        currentX += X_axis;
        currentY += Y_axis;
    }
    /// <summary>
    /// Resets the view of the planet to the original position
    /// </summary>
    public void ResetPlanetView() {
        currentX = currentY = 3f;
    }

    /// <summary>
    /// Moves the camera around the given target
    /// </summary>
    /// <param name="target">Target of the camera</param>
    private void AroundTarget(Transform target) {
        Vector3 direction = new Vector3(0, 0, -planetOffset_Z);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0f);
        transform.position = target.position + rotation * direction * 5f;
        transform.LookAt(target.position);
    }

    /// <summary>
    /// Sets either if the camera is looking at the player or not
    /// </summary>
    public void SetCamera() {
        IsFollowingPlayer = !IsFollowingPlayer;
    }
}
