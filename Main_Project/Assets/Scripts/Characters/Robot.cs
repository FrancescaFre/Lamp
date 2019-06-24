using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Robot : Skill
{
    [Range(5, 10)]
    public float walkSpeed = 5f;

    [Range(300, 600)]
    public float batteryDuration = 420f;
    private float _progress = 0f;

    public PlayerController playerController;
    public bool pickable = false;
    public Image battery;
    public Image batteryProgress;

    private Rigidbody _rb;
    private Camera _cam;
    private Vector3 _moveDir;
    private Vector3 _movement;
    private float _horiz_axis;
    private float _vert_axis;

    /// <summary>
    /// Spawns the robot in front of the player
    /// </summary>
    public override void ActivateSkill()
    {
        transform.position = playerController.transform.position; // TODO

        gameObject.SetActive(true);
        enabled = true;
        _progress = 0f;
        battery.gameObject.SetActive(true);
    }

    /// <summary>
    /// Picks Up the sleeping robot
    /// </summary>
    public void PickUp()
    {
        pickable = false; 
        gameObject.SetActive(false);
        _cam.gameObject.SetActive(true);
    }

    /// <summary>
    /// Shuts down the robot and returns control to the character
    /// </summary>
   // public void DisableRobot()
    public override void DeactivateSkill()
    {
        if (playerController.usingSkill)
        {
            playerController.usingSkill = false;

            enabled = false;
            _cam.gameObject.SetActive(false);
            _progress = 0f;

            playerController.isCasting = false;
            playerController.enabled = true;
            //   player.playerCamera.gameObject.SetActive(true);

            pickable = true;

            battery.gameObject.SetActive(false);
            batteryProgress.fillAmount = 0;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Tags.Player))
        {
            PickUp();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        pickable = false;
    }

    // Use this for initialization
    void Start()
    {
        _movement = Vector3.zero;
        _moveDir = Vector3.zero;
        _horiz_axis = 0f;
        _vert_axis = 0f;
        _cam = GetComponentInChildren<Camera>();
        _rb = GetComponent<Rigidbody>();
        playerController = GetComponentInParent<PlayerController>();
        playerController.skill = this;

        battery = InGameHUD.Instance.InGameHUDPanel.transform.Find("Gauge Panel").Find("Battery").GetComponent<Image>();
        batteryProgress = battery.transform.GetChild(0).GetComponent<Image>();
        battery.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        MoveRobot();

        _progress++;
        batteryProgress.fillAmount += 1.0f / batteryDuration;

        if (_progress >= batteryDuration || Input.GetKeyDown(KeyCode.P))
            DeactivateSkill();
    }

    /// <summary>
    /// Same movement code as the character
    /// </summary>
    private void MoveRobot()
    {
        this._horiz_axis = Input.GetAxis(Controllers.Horizontal);
        this._vert_axis = Input.GetAxis(Controllers.Vertical);
        _moveDir.Set(_horiz_axis, 0f, _vert_axis);
        _moveDir.Normalize();
        _movement = transform.TransformDirection(_moveDir) * walkSpeed * Time.deltaTime;
        _rb.MovePosition(_rb.position + _movement);
    }
}