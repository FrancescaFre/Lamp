using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigF : MonoBehaviour {

    public Transform prefab;
    public Vector3 startPosition;
    public Vector3 endPosition;
    bool firstPhase = true;
    bool isValid = true;

    private Rigidbody _rb;
    private Vector3 _moveDir;
    private Vector3 _movement;
    private float _horiz_axis;
    private float _vert_axis;

    public float walkSpeed = 8f;
    public controller1 player;

    Transform go;

    // Use this for initialization
    void Start () {
        _movement = Vector3.zero;
        _moveDir = Vector3.zero;
        _horiz_axis = 0f;
        _vert_axis = 0f;
        player = GetComponentInParent<controller1>();
    }
	
	// Update is called once per frame
	void Update () {
        //check terreno valido per lo scavo
        Debug.Log("PARTITO");
        if (isValid && !firstPhase && Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("primo");
            Teleport();
        }

        if (isValid && firstPhase && Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("primo");
            SecondPhase();
        }

        Debug.Log("firstphase "+!firstPhase +" rb "+(_rb!=null));
        if (!firstPhase && _rb) {
            this._horiz_axis = Input.GetAxis("Horizontal");
            this._vert_axis = Input.GetAxis("Vertical");
            _moveDir.Set(_horiz_axis, 0f, _vert_axis);
            _moveDir.Normalize();
            _movement = transform.TransformDirection(_moveDir) * walkSpeed * Time.deltaTime;
            _rb.MovePosition(_rb.position + _movement);
        }
	}

    private void SecondPhase (){
        Debug.Log("Second");
        //metto il cerchio a terra
        //this.transform.position = new Vector3(0f, -1.4f, 0f);
        startPosition = this.transform.position;
        go = Instantiate(prefab);
        go.transform.position = this.transform.position;
        go.transform.rotation = this.transform.rotation;

        //aggiungo il rb e lo script
        gameObject.AddComponent<GravityBody>();
        _rb = GetComponent<Rigidbody>();
        //aggiorno i bool
        player.blocked= true;
        firstPhase = false;
    }

    private void Teleport() {
        Debug.Log("Teleport");
        //teleport
        player.GetComponent<Rigidbody>().MovePosition(this.transform.position); //sposto il player
        Destroy(go.gameObject); //elimino il cerchio

        //ripristino i flag e tolgo i component
        Destroy(gameObject.GetComponent<GravityBody>());
        Destroy(_rb);
        player.blocked = false;
        firstPhase = true;
        transform.localPosition = new Vector3 (0f, -1.4f, 0f);
        gameObject.SetActive(false);
    }
}
