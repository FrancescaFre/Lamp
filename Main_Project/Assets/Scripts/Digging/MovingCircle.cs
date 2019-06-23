using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovingCircle : ZoneDig {
    //public GameObject dummyCirclePrefab;

    private float distanceRadius = 8f;
    private PlayerController pc;

    // private Vector3 _dummOffset;
    private Vector3 _movement;
    private Rigidbody _rb;
    private float _speed;
    private float _horizInput, _vertInput;

    private bool canDig = true;

    protected void OnTriggerEnter(Collider terrain) {
        if (terrain.gameObject.CompareTag(Tags.Solid) ||
            terrain.gameObject.CompareTag(Tags.Water) /*||
            terrain.gameObject.CompareTag(Tags.Ice) ||
            terrain.gameObject.CompareTag(Tags.Leaves)*/) {
            canDig = false;
        }
    }

    protected void OnTriggerStay(Collider terrain) {
        if (terrain.gameObject.CompareTag(Tags.Solid) ||
            terrain.gameObject.CompareTag(Tags.Water) /*||
            terrain.gameObject.CompareTag(Tags.Ice) ||
            terrain.gameObject.CompareTag(Tags.Leaves)*/) {
            canDig = false;
        }
    }

    protected void OnTriggerExit(Collider terrain) {
        if (terrain.gameObject.CompareTag(Tags.Solid) ||
            terrain.gameObject.CompareTag(Tags.Water) /*||
            terrain.gameObject.CompareTag(Tags.Ice) ||
            terrain.gameObject.CompareTag(Tags.Leaves)*/) {
            canDig = true;
        }
    }

    private new void Start() {
        pc = GameManager.Instance.currentPC;
    }

    public void Setup(Transform zoneDig, PlayerController player) {
        this.player = player;
        _pm = player.GetComponent<PlayerMovement>();
        // _cam = FindObjectOfType<CameraManager>().transform;
        _movement = Vector3.zero;
        _rb = GetComponent<Rigidbody>();
        _horizInput = 0f;
        _vertInput = 0f;
        _speed = player.gameObject.GetComponent<PlayerMovement>().walkSpeed;
        BasicCamera.instance.player = transform;

        Reposition(zoneDig);
    }

    private void FixedUpdate() {
        if (!player.caster) return;

        player.caster.transform.position = BasicCamera.instance.GetComponent<Camera>().WorldToScreenPoint(transform.position);
        if (!player.IsCasting) {
            if (Input.GetAxis(Controllers.Vertical) > 0)
                _movement = (BasicCamera.instance.transform.forward * _vertInput + BasicCamera.instance.transform.right * _horizInput).normalized * _speed * Time.deltaTime;
            else
                _movement = (BasicCamera.instance.transform.up * _vertInput + BasicCamera.instance.transform.right * _horizInput).normalized * _speed * Time.deltaTime;

            if (Vector3.Distance(_rb.position + _movement, pc.transform.position) < distanceRadius) {
                _rb.MovePosition(_rb.position + _movement);
            }
        }
    }

    override protected void Update() {
        _horizInput = Input.GetAxis(Controllers.Horizontal);
        _vertInput = Input.GetAxis(Controllers.Vertical);
        ChangeColor();
    }

    // Uses its own CanDig method, and checks own collisions instead of player's (uses the canDig boolean)
    public override bool CanDig() {
        return canDig;
    }

    // This needs to change color, instead
    protected override void ChangeColor() {
        if (CanDig())
            GetComponent<MeshRenderer>().material = digYes;
        else
            GetComponent<MeshRenderer>().material = digNo;
    }

    public void OnDestroy() {
        BasicCamera.instance.player = GameManager.Instance.currentPC.transform;
    }

    private void Reposition(Transform zoneDig) {
        // The first part makes the circle appear in front/rear of the player, depending if there's an obstacle on the way
        transform.SetParent(zoneDig);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (Physics.Raycast(player.transform.position, player.transform.forward, 1f, ~(1 << 9))) // If it hits something in front of the player (except the player, that's layer 9)
            transform.localPosition += new Vector3(0, 0, -1); // Spawns the circle in rear of the player
        else
            transform.localPosition += new Vector3(0, 0, 1); // Spawns the circle in front of the player

        transform.SetParent(null);
        transform.localScale = new Vector3(.6f, .01f, .6f); // Prefab values to re-scale the circle correctly. Bad code, i know, didn't want to access to Resources
        //_movingCircle.transform.rotation = player.transform.rotation;
    }
}