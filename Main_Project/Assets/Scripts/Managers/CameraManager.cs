using UnityEngine;

public class CameraManager : MonoBehaviour {//http://youtu.be/MFQhpwc6cKE
    public Transform playerModel;// the model of the player, NOT the pivot
    public Transform planetModel;
    public bool IsFollowingPlayer { get; private set; }

    [Range(10,15)]
    public float smoothSpeed=10.25f;   //the higher it is, the faster the camera will lock on the player

    public Vector3 playerOffset;
    [Range(3,80)]
    public float planetOffset_Z;

    private float _currentX = 0f;
    private float _currentY = 0f;

    void Awake() {
        IsFollowingPlayer = true;
    }


    void FixedUpdate() {
        if (IsFollowingPlayer)
            AroundTarget(playerModel, playerOffset);
        else
            AroundTarget(planetModel, new Vector3(0, 0, -planetOffset_Z));
    }
    /// <summary>
    /// Moves the camera around the Target
    /// </summary>
    /// <param name="X_axis">Movement on X axis</param>
    /// <param name="Y_axis">Movement on Y axis</param>
    public void LookAtTarget(float X_axis, float Y_axis) {
        _currentX += X_axis;
        _currentY += Y_axis;
        _currentY = Mathf.Clamp(_currentY, -50f, 50f);  //prevents the camera from doing a revolution around the player on the Y axis
    }
    /// <summary>
    /// Resets the view of the planet to the original position
    /// </summary>
    public void ResetPlanetView() {
        _currentX = _currentY = 3f;
    }

    /// <summary>
    /// Resets the view of the player to the original position
    /// </summary>
    public void ResetPlayerView() {
        _currentX = _currentY = 0f;
    }



    /// <summary>
    /// Moves the camera around the given target
    /// </summary>
    /// <param name="target">Target of the camera</param>
    private void AroundTarget(Transform target, Vector3 offset) {
        
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0f);
        transform.position = target.position + rotation * offset ;
        transform.LookAt(target.position);
    }

    /// <summary>
    /// Sets either if the camera is looking at the player or not and resets the camera accordingly
    /// </summary>
    public void SetCamera() {
        IsFollowingPlayer = !IsFollowingPlayer;
        if (IsFollowingPlayer)
            ResetPlayerView();
        else
            ResetPlanetView(); 
    }
}
