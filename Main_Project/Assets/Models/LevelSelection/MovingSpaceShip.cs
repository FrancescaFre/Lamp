using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovingSpaceShip : MonoBehaviour {
    public GameObject shipModel;

    [Header("Speed")]
    public float speed = 15.0f;
    public float rotationSpeed = 100.0f;

    [Header("Pan values")]
    public float minX = -100;
    public float maxX = 60;
    public float minZ = -100;
    public float maxZ = 60;
    public bool inFrontOf = false;
    public Transform trigger;

    public ParticleSystem[] fire = new ParticleSystem[2];

    private Rigidbody rb;
    private SFXEmitter emitter;

    private GameObject confirmLabel;
    private Camera mainCamera;

    public bool toCharSelection = false;

    // Use this for initialization
    private void Start() {
        rb = this.GetComponent<Rigidbody>();
        emitter = GetComponent<SFXEmitter>();
        NewGuiManager.instance.spaceShip = this;

        NewGuiManager.instance.CreateHUDReference(ref confirmLabel);
        mainCamera = CameraFollowSpaceship.instance.GetComponent<Camera>();
    }

    // Update is called once per frame
    private void FixedUpdate() {
        ShipMovement();
    }

    private void ShipMovement() {
        if (toCharSelection) return;

        float translation = Input.GetAxis("Vertical") * speed;

        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        if (translation != 0.0 || rotation != 0.0) {
            foreach (ParticleSystem ps in fire) {
                ps.gameObject.SetActive(true);
                ps.Play();
            }
            if (!emitter.source.isPlaying)
                emitter.source.Play();
        }
        else if (translation == 0.0 || rotation == 0.0)
            foreach (ParticleSystem ps in fire)
                if (ps.isPlaying)
                    ps.Stop();
                else
                    if (!ps.isEmitting) {
                    ps.gameObject.SetActive(false);
                    emitter.source.Stop();
                }

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0, 0, translation);

        float posx = Mathf.Clamp(transform.position.x, minX, maxX);
        float posz = Mathf.Clamp(transform.position.z, minZ, maxZ);
        transform.position = new Vector3(posx, transform.position.y, posz);

        transform.Rotate(0, rotation, 0);
    }

    private void LateUpdate() {
        confirmLabel.transform.position = mainCamera.WorldToScreenPoint(transform.position + Vector3.up);
        confirmLabel.SetActive(!toCharSelection && inFrontOf);
        if (!toCharSelection && inFrontOf && Input.GetKeyDown(KeyCode.Space)) {
            toCharSelection = true;

            shipModel.SetActive(false);
            NewGuiManager.instance.SwitchCharANDLevel(toCharSelection);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.Planet_Switch)) {
            trigger = other.transform;
            speed -= 10;
            inFrontOf = true;

            confirmLabel.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(Tags.Planet_Switch)) {
            speed += 10;
            inFrontOf = false;

            confirmLabel.gameObject.SetActive(false);
        }
    }
}