using UnityEngine;

public enum DigType { NONE = 0, LINEAR, ZONE }

public class Digging : MonoBehaviour {

    public float speed = 5f;

    public Material digYes;
    public Material digNo;

    public PlayerController player;
    public Caster caster;
    public Transform prefabToSpawn;  

    private DigType _digType;

    private Transform _startingTransform;
    private Transform _target;
    private Rigidbody _targetRB;

    // Update is called once per frame
    void Update ()
    {
        ChangeColor(_digType);
	}

    // The circle moves when _canMove is true  
    void FixedUpdate()
    {
        // TODO: MovementOnSphere su di questo ↓
        if (_targetRB != null && !player.IsCasting)
        {
            Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _targetRB.MovePosition(_targetRB.position + transform.TransformDirection(dir) * speed * Time.deltaTime);
        }
    }
    

    /// <summary>
    /// Performs the digging action (called by caster)
    /// </summary>
    public void Dig()
    {
        if (_digType == DigType.LINEAR)
            player.GetComponent<Rigidbody>().MovePosition(-(player.transform.position)); // Moves the player on the other side of the world
        else
            player.GetComponent<Rigidbody>().MovePosition(_target.transform.position); // Moves the player right on top of the target
      
        // After digging
        StopDig();
        player.IsCasting = false;

    }

    /// <summary>
    /// Checks any result after pressing [LDIG]
    /// </summary>
    public void LinearDig ()
    {
        if (_digType == DigType.LINEAR) // If you already pressed [LDIG]
            if (CheckTerrain(DigType.LINEAR))
            {
                caster.StartCircle(DigType.LINEAR);
                player.IsCasting = true;
            }
            else
                StopDig();

        else if (_digType == DigType.ZONE) // If you press [LDIG] after [ZDIG] it cancels the digging action
            StopDig();

        else // First time the player presses [LDIG]
        {
            _digType = DigType.LINEAR;
            gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Checks any result after pressing [ZDIG]
    /// </summary>
    public void ZoneDig()
    {
        if (player.IsZoneDigging) // If you already pressed [ZDIG] 2 times (activate -> valid start -> now)
            if (_target.GetComponent<Digging>().CheckTerrain(DigType.ZONE))
            {
                caster.StartCircle(DigType.ZONE);
                player.IsCasting = true;
            }
            else
                StopDig();

        else if (_digType == DigType.ZONE) // If you already pressed [ZDIG] (activate -> now)
            if (CheckTerrain(_digType)) // Second condition is true only for the moving circle
            {
                _startingTransform = transform; // Saves the position to restart the circle afterwards
                CreateTarget();
                player.IsZoneDigging = true;  
            }
            else
                StopDig();

        else if (_digType == DigType.LINEAR) // If you press [LDIG] after [ZDIG] it cancels the digging action
            StopDig();

        else // First time the player presses [ZDIG]
        {
            _digType = DigType.ZONE;
            gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Switches the circle's color between green and red
    /// </summary>
    /// <param name="digType"></param>
    private void ChangeColor (DigType digType)
    {
        if (CheckTerrain(digType))
            GetComponent<MeshRenderer>().material = digYes;
        else
            GetComponent<MeshRenderer>().material = digNo;
    }

    /// <summary>
    /// Checks if the dig can be performed
    /// </summary>
    /// <param name="digType"></param>
    /// <returns></returns>
    private bool CheckTerrain (DigType digType)
    {
        // TODO: check terrain ↓
        if (transform.position.z >= 0) // If the circle is not on a penetrable terrain
            return false;
        else if (digType == DigType.LINEAR)
            return true; // TODO: VERTICAL RAYCAST?
        else
            return true; // For the targeted dig there's no other check apart of the terrain
    }

    /// <summary>
    /// Instantiates a target circle and sets it
    /// </summary>
    private void CreateTarget()
    {  
        _target = Instantiate(prefabToSpawn);

        _target.transform.position = transform.position;
        _target.transform.rotation = transform.rotation;

        _target.gameObject.AddComponent<GravityBody>();
        _target.GetComponent<GravityBody>().attractor = player.GetComponent<GravityBody>().attractor;

        _target.GetComponent<Digging>()._targetRB = _target.GetComponent<Rigidbody>();
        _target.GetComponent<Digging>().player = player;
    }

    /// <summary>
    /// Instantiates a target circle and sets it
    /// </summary>
    private void DestroyTarget()
    {
        Destroy(_target.gameObject);
        _target = null;
        _targetRB = null;
    }

    /// <summary>
    /// Hides the dig circle
    /// </summary>
    /// <param name="digType"></param>
    private void StopDig()
    {
        if (player.IsZoneDigging)
        {
            transform.SetPositionAndRotation(_startingTransform.position, _startingTransform.rotation);
            DestroyTarget();
            player.IsZoneDigging = false;
        }         

        gameObject.SetActive(false);
        _digType = DigType.NONE;
    }
}
