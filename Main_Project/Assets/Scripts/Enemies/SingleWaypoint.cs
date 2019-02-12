using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleWaypoint : MonoBehaviour
{

    public bool underALamp = false;

    //private void OnTriggerStay(Collider other)
   
    private void OnTriggerEnter (Collider other)
    { 
        if (other.CompareTag(Tags.Lamp_Base))
        {
            Debug.Log("Under a lamp");
            underALamp = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Lamp_Base))
        {
            Debug.Log("Under a lamp");
            underALamp = false;
        }
    }

}
