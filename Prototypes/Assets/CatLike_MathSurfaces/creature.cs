using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creature : MonoBehaviour {
    public float speed;
    public float offset_from_planet;
    Rigidbody rb;

    public float time;
    public float variation;

    public enum MathFunctionName {SineFunction, Circle}
    public MathFunctionName selected; //crea la tendina

    delegateFunctions f;
    delegateFunctions[] functions = {Sine, Circle};

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        offset_from_planet = 15 ;
        speed = 5;
    }

    void Update () {
        f = functions[(int)selected];
        time = Time.deltaTime;
        //  variation = f(rb.position.x, rb.position.z, time);
        // rb.position = new Vector3(rb.position.x, variation.y, rb.position.z);
        // rb.position += offset_from_planet;
        rb.position = new Vector3 (rb.position.x, Mathf.Sin(0.5f * time) + offset_from_planet, rb.position.z);
        rb.MovePosition(rb.position * speed * Time.deltaTime);
	}

    #region Mathematical functions
    //https://www.desmos.com/calculator

    static Vector3 Sine(float x, float z, float t)
    { 
        return new Vector3(0,Mathf.Sin(0.5f * t),0);
    }

    static Vector3 Circle(float x, float z, float t) {
        return new Vector3(Mathf.Cos(x),0,Mathf.Sin(x));
    }
    #endregion
}
