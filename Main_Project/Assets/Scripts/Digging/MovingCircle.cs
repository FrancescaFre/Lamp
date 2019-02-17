using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovingCircle : ZoneDig {

    public GameObject dummyCirclePrefab;

    private float distanceRadius = 8f;
    private PlayerController pc;

    private Transform _cam;
    private GameObject _dummyCircle;
    private Vector3 _dummyOffset;
    private Vector3 _movement;
    private Rigidbody _rb;
    private float _speed;
    private float _horizInput, _vertInput;

    private bool canDig = true;

    protected void OnTriggerEnter(Collider terrain)
    {
        if (terrain.gameObject.CompareTag(Tags.Solid) ||
            terrain.gameObject.CompareTag(Tags.Water) ||
            terrain.gameObject.CompareTag(Tags.Ice) ||
            terrain.gameObject.CompareTag(Tags.Leaves))
        {
            canDig = false;
        }
    }

    protected void OnTriggerStay(Collider terrain)
    {
        if (terrain.gameObject.CompareTag(Tags.Solid) ||
            terrain.gameObject.CompareTag(Tags.Water) ||
            terrain.gameObject.CompareTag(Tags.Ice) ||
            terrain.gameObject.CompareTag(Tags.Leaves))
        {
            canDig = false;
        }
    }

    protected void OnTriggerExit(Collider terrain)
    {
        if (terrain.gameObject.CompareTag(Tags.Solid) ||
            terrain.gameObject.CompareTag(Tags.Water) ||
            terrain.gameObject.CompareTag(Tags.Ice) ||
            terrain.gameObject.CompareTag(Tags.Leaves))
        {
            canDig = true;
        }
    }

    new void Start()
    {
       
        pc = GameManager.Instance.currentPC;
    }

    public void Setup(Transform zoneDig, PlayerController player)
    {
        this.player = player;
        _pm = player.GetComponent<PlayerMovement>();
        _cam = FindObjectOfType<CameraManager>().transform;
        _movement = Vector3.zero;
        _rb = GetComponent<Rigidbody>();
        _horizInput = 0f;
        _vertInput = 0f;
        _speed = player.gameObject.GetComponent<PlayerMovement>().walkSpeed;

        Reposition(zoneDig);
        CreateDummy();
        StartCoroutine(AdjustDummyPosition());
    }

    void FixedUpdate() {
        if (!player.caster) return;

        player.caster.transform.position = player.MainCamera.GetComponent<Camera>().WorldToScreenPoint(transform.position);
        if (!player.IsCasting)
        {
            if (Input.GetAxis(Controllers.Vertical) > 0)
                _movement = (_cam.forward * _vertInput + _cam.right * _horizInput).normalized * _speed * Time.deltaTime;
            else
                _movement = (_cam.up * _vertInput + _cam.right * _horizInput).normalized * _speed * Time.deltaTime;

            if (Vector3.Distance(_rb.position + _movement, pc.transform.position) < distanceRadius)
            {
                _rb.MovePosition(_rb.position + _movement);
                _dummyCircle.GetComponent<Rigidbody>().MovePosition(_dummyCircle.GetComponent<Rigidbody>().position + _movement);
            }
        }
    }

    override protected void Update()
    {
        _horizInput = Input.GetAxis(Controllers.Horizontal);
        _vertInput = Input.GetAxis(Controllers.Vertical);
        ChangeColor();
    }

    // Uses its own CanDig method, and checks own collisions instead of player's (uses the canDig boolean)
    public override bool CanDig()
    {
        return canDig;
    }

    void OnDestroy()
    {
        _cam.GetComponent<CameraManager>().RestoreDummyCam();
        Destroy(_dummyCircle);
    }

    private void Reposition(Transform zoneDig)
    {
        // The first part makes the circle appear in front/rear of the player, depending if there's an obstacle on the way
        transform.SetParent(zoneDig);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _cam.transform.SetParent(transform);

        if (Physics.Raycast(player.transform.position, player.transform.forward, 1f, ~(1 << 9))) // If it hits something in front of the player (except the player, that's layer 9)
            transform.localPosition += new Vector3(0, 0, -1); // Spawns the circle in rear of the player
        else
            transform.localPosition += new Vector3(0, 0, 1); // Spawns the circle in front of the player 

        _cam.transform.SetParent(null);
        transform.SetParent(null);
        transform.localScale = new Vector3(.6f, .01f, .6f); // Prefab values to re-scale the circle correctly. Bad code, i know, didn't want to access to Resources
        //_movingCircle.transform.rotation = player.transform.rotation;
    }

    private void CreateDummy()
    {
        _dummyOffset = _pm.DummyOffset;
        _dummyCircle = Instantiate(dummyCirclePrefab);
        _dummyCircle.transform.position += _dummyOffset;
        _dummyCircle.transform.rotation = transform.rotation;
        //_dummyCircle.transform.GetChild(0).localRotation = _pm.DummyPlayer.transform.GetChild(0).localRotation; // --- TODO FIX CAMROTATION BUG
        //_dummyCircle.transform.GetChild(0).localRotation = Quaternion.identity;                                 // --- TODO FIX CAMROTATION BUG
        _cam.GetComponent<CameraManager>().DetachForZoneDig(_dummyCircle.transform.GetChild(0).GetChild(0));
    }

    /// <summary>
    /// Does its best to adjust position bugs
    /// </summary>
    /// <returns></returns>
    private IEnumerator AdjustDummyPosition()
    {
        while (true)
        {
            if (_dummyCircle.transform.position - transform.position != _dummyOffset)
            {
                //transform.position = dummyPlayer.transform.position - _dummyOffset; // Less adjustments, but harder to force teleports and transform movements outside
                _dummyCircle.transform.position = transform.position + _dummyOffset; // More adjustments, but the player can now do the fuck it wants with its transform
            }
            yield return new WaitForFixedUpdate();
        }
    }

}
