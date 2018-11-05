using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSebastianCov : MonoBehaviour {
    public float moveSpeed = 6f;

    Rigidbody rb;
    Camera viewCamera;
    Vector3 velocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        viewCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 mousePos = viewCamera.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        Debug.Log(mousePos);
        transform.LookAt(mousePos + Vector3.up * transform.position.y) ; //la riga sopra serve per rilevare la posizione del mouse, questa riga serve per far guardare il giocatore sempre verso il puntatore
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
