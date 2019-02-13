using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour {

    private List<Light> lights = new List<Light>();
    public bool t = false; 
    private void Start()
    {
      //  layer = this.gameObject.layer;
        foreach (Light light in this.transform.GetComponentsInChildren<Light>())
            if (light.transform.CompareTag(Tags.DigLight))
                lights.Add(light);

        foreach (Light light in lights)
            light.enabled = false;
       


        if ( !(((1 << gameObject.layer) & LayerMask.GetMask("Obstacle")) != 0))
        {
            
            t = true;
            Destroy(GetComponent<Walls>()); //.enabled = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() && other.GetComponentInParent<PlayerController>().CompareTag(Tags.Player) && lights.Count >0)
            foreach (Light light in lights)
               light.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() && other.GetComponentInParent<PlayerController>().CompareTag(Tags.Player))
            foreach (Light light in lights)
                light.enabled = false;
    }

    public void SwitchOff()
    {
        foreach (Light light in lights)
            light.enabled = false;
    }
}
