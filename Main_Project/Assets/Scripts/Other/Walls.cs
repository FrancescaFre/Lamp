using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour {

    private List<Light> lights = new List<Light>();
    

    private void Start()
    {
      //  layer = this.gameObject.layer;
        foreach (Light light in this.transform.GetComponentsInChildren<Light>())
            if (light.transform.CompareTag(Tags.DigLight))
                lights.Add(light);

        foreach (Light light in lights)
            light.enabled = false;

        // remember this is the same as (i>1 && i<3)
        if ( !(((1 << gameObject.layer) & LayerMask.GetMask("Obstacle")) != 0))// if "this" wall is NOT an obstacle
        {
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
