using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour {

    private List<Light> lights = new List<Light>();
  public LayerMask layer; 
    private void Start()
    {
        layer = gameObject.layer;
        foreach (Light light in this.transform.GetComponentsInChildren<Light>())
                if (light.transform.CompareTag("DigLight"))
                    lights.Add(light);

        foreach (Light light in lights)
            light.enabled = false;
      //--------------DA CORREGGERE
       // if (this.gameObject.layer != LayerMask.GetMask("Obstacle"))
         //    GetComponent<Walls>().enabled = false;


    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() && other.GetComponentInParent<PlayerController>().CompareTag("Player") && lights.Count >0)
            foreach (Light light in lights)
               light.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.gameObject.layer == LayerMask.GetMask("Obstacle") && other.GetComponentInParent<PlayerController>() && other.GetComponentInParent<PlayerController>().CompareTag("Player"))
            foreach (Light light in lights)
                light.enabled = false;
    }

    public void SwitchOff()
    {
        foreach (Light light in lights)
            light.enabled = false;
    }
}
