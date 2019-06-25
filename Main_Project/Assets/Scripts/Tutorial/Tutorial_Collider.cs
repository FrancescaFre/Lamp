using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Collider : MonoBehaviour
{
    private TutorialController tutorialController;
    private int code; 

	// Use this for initialization
	void Start ()
    {
        tutorialController = gameObject.GetComponentInParent<TutorialController>();
        code = int.Parse(this.name.Substring(this.name.Length - 2, 2));
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
            tutorialController.Notify(code);    
    }


}
