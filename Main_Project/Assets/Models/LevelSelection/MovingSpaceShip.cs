using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpaceShip : MonoBehaviour {
    public float speed = 15.0f;
    public float rotationSpeed = 100.0f;

    private float minX = -100;
    private float maxX = 60;
    private float minZ = -100;
    private float maxZ = 60;
    public bool inFrontOf = false;
    public Transform trigger; 

    public ParticleSystem[] fire = new ParticleSystem[2];

    private Rigidbody rb;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet"))
        {
            trigger = other.transform;
            speed -= 10;
            inFrontOf = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
        if (other.CompareTag("Planet"))
        {
            speed += 10;
            inFrontOf = false;
        }
    }


    // Use this for initialization
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float translation = Input.GetAxis("Vertical") * speed;

        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        if (translation != 0.0 || rotation != 0.0)
        {
            foreach (ParticleSystem ps in fire)
            {
                ps.gameObject.SetActive(true);
                ps.Play();
            }
        }
        else if (translation == 0.0 || rotation == 0.0)
            foreach (ParticleSystem ps in fire)
                if (ps.isPlaying)
                    ps.Stop();
                else
                    if (!ps.isEmitting)
                    ps.gameObject.SetActive(false);


        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0, 0, translation);

        float posx = Mathf.Clamp(transform.position.x, minX, maxX);
        float posz = Mathf.Clamp(transform.position.z, minZ, maxZ);
        transform.position = new Vector3(posx, 0, posz);

        transform.Rotate(0, rotation, 0);
    }


}
