using UnityEngine;

public enum DigType { NONE = 0, LINEAR, ZONE }

public class Digging : MonoBehaviour {

    public float speed = 5f;

    public Material digYes;
    public Material digNo;
    public Caster caster;
    public bool isTheTarget;

    private PlayerController _player;
    private Vector3 _startingPosition;
    private DigType _digType;
    private bool _canMove;
    

    // Use this for initialization
    void Start ()
    {
        _player = GetComponentInParent<PlayerController>();
        _startingPosition = transform.position;
        _canMove = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        ChangeColor(_digType);
	}

    // The circle moves when _canMove is true
    
    void FixedUpdate()
    {
        // TODO: MovementOnSphere su di questo ↓
        if (isTheTarget && _canMove)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            rb.MovePosition(rb.position + transform.TransformDirection(dir) * speed * Time.deltaTime);
        }
    }
    

    /// <summary>
    /// Performs the digging action (called by caster)
    /// </summary>
    public void Dig()
    {
        if (_digType == DigType.LINEAR)
            _player.transform.position = -(_player.transform.position); // Moves the player on the other side of the world
        else
            _player.transform.position = transform.position; // Moves the player right on top of the target

            
        // After digging
        StopDig();
        _player.IsCasting = false;

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
                _player.IsCasting = true;
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
        if (_canMove) // If you already pressed [ZDIG] 2 times (activate -> valid start -> now)
            if (CheckTerrain(DigType.ZONE))
            {
                caster.StartCircle(DigType.ZONE);
                _canMove = false;
                _player.IsCasting = true;
            }
            else
            {
                _canMove = false;
                StopDig();
            }

        else if (_digType == DigType.ZONE) // If you already pressed [ZDIG] (activate -> now)
            if (CheckTerrain(_digType)) // Second condition is true only for the moving circle
            {
                Instantiate(this,_player.transform);
                gameObject.AddComponent<Rigidbody>();
                GetComponent<GravityBody>().enabled = true;
                _canMove = true;
                _player.IsZoneDigging = true;
                _startingPosition = transform.position; // Saves the position to restart the circle afterwards
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
    /// Hides the dig circle
    /// </summary>
    /// <param name="digType"></param>
    private void StopDig()
    {
        if (_player.IsZoneDigging)
        {
            transform.position = _startingPosition;
            GetComponent<GravityBody>().enabled = false;
            Destroy(GetComponent<Rigidbody>());
            _player.IsZoneDigging = false;
        }         

        gameObject.SetActive(false);
        _canMove = false;
        _digType = DigType.NONE;
    }
}
