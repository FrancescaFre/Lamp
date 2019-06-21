using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpaceShip : MonoBehaviour {
    public float speed = 15.0f;
    public float rotationSpeed = 100.0f;

    private float minX= -100;
    private float maxX = 60;
    private float minZ = -100;
    private float maxZ = 60;

    public ParticleSystem [] fire = new ParticleSystem [2];

    private Rigidbody rb;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Planet/Switch"))
            speed -= 10; 
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Planet/Switch"))
            speed += 10; 
    }


    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void FixedUpdate() {
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
        else if (translation == 0.0 || rotation ==0.0)
            foreach (ParticleSystem ps in fire)
                if (ps.isPlaying)
                    ps.Stop();
                else
                    if (!ps.isEmitting )
                    ps.gameObject.SetActive(false);


        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0, 0, translation);

        float posx = Mathf.Clamp(transform.position.x, -minX, minX);
        float posz = Mathf.Clamp(transform.position.z, -minZ, minZ);
        transform.position = new Vector3(posx, 0, posz);

        transform.Rotate(0, rotation, 0);
    }
}
