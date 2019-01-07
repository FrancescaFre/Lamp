using UnityEngine;
using UnityEngine.UI;

public class MovingCircle : ZoneDig {

    private float distanceRadius = 8f;
    private PlayerController pc;

    private Transform _cam;
    private Transform _digCam;
    private Vector3 _movement;
    private Rigidbody _rb;
    private float _speed;
    private float _horizInput, _vertInput;

    new void Start()
    {
        caster = InGameHUD.Instance.InGameHUDPanel.transform.Find("Gauge Panel").Find("Caster").GetComponent<Image>();
        bar = caster.transform.GetChild(0).GetComponent<Image>();
        pc = GameManager.Instance.currentPC;
    }

    public void Setup(Transform start, PlayerController player)
    {
        transform.position = start.position;
        transform.rotation = player.transform.rotation;
        transform.Translate(transform.forward * 0.6f);
        this.player = player;

        _pm = player.GetComponent<PlayerMovement>();
        _cam = FindObjectOfType<CameraManager>().transform;
        _digCam = transform.GetChild(0);
        _movement = Vector3.zero;
        _rb = GetComponent<Rigidbody>();
        _horizInput = 0f;
        _vertInput = 0f;

        _speed = player.gameObject.GetComponent<PlayerMovement>().walkSpeed;
        _cam.GetComponent<CameraManager>().enabled = false;
        _digCam.SetPositionAndRotation(_cam.position, _cam.rotation);
    }

    void FixedUpdate()
    {
        if (!player.IsCasting)
        {
            if (Input.GetAxis("Vertical") > 0)
                _movement = (_cam.forward * _vertInput + _cam.right * _horizInput).normalized * _speed * Time.deltaTime;
            else
                _movement = (_cam.up * _vertInput + _cam.right * _horizInput).normalized * _speed * Time.deltaTime;

            if (Vector3.Distance(_rb.position + _movement, pc.transform.position) < distanceRadius)
                _rb.MovePosition(_rb.position + _movement);
        }
    }

    override protected void Update()
    {
        _horizInput = Input.GetAxis("Horizontal");
        _vertInput = Input.GetAxis("Vertical");
        ChangeColor();
    }

    void LateUpdate()
    {
        _cam.SetPositionAndRotation(_digCam.position, _digCam.rotation);
    }

    void OnDestroy()
    {
        _cam.GetComponent<CameraManager>().enabled = true;
    }
}
