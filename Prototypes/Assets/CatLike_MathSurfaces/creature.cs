using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creature : MonoBehaviour
{
    //public float speed;
    
    public static float speed = (2 * Mathf.PI) / 20f; //2 PI us 360 dregressm so there are need 5 sec to complete
    public static float r = 5;
    public static float angle = 0;

    public float offset_from_planet;


    public float time;
    public Vector3 variation;

    public enum MathFunctionName { Idle, Line, Circle }
    public MathFunctionName selected; //crea la tendina

    delegateFunctions f;
    static delegateFunctions[] functions = {Idle, Line, Circle };

    Rigidbody rb;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        speed = 5f;
    }

    void Update()
    {
        f = functions[(int)selected];
        variation = f(rb.position.x, rb.position.z, Time.time);
        rb.MovePosition(rb.position + variation * (speed * Time.deltaTime));
    }


    #region Mathematical functions
    //https://www.desmos.com/calculator

     static Vector3 Idle(float x, float z, float t) {
        return new Vector3(0, 0, 0);
    }

     static Vector3 Line(float x, float z, float t)
    {
        return new Vector3(t*x, 0, z);
    }

     static Vector3 Circle(float x, float z, float t)
    {//https://answers.unity.com/questions/596671/circular-rotation-via-the-mathematical-circle-equa.html
        angle += speed *Time.deltaTime;
        return new Vector3(Mathf.Cos(angle)*r, 0, Mathf.Sin(angle)*r);
    }

    #endregion
}
