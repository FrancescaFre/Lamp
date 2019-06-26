using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Collider : MonoBehaviour
{
    private TutorialController tutorialController;
    private int code;
    private List<int> exit = new List<int>{ 1, 2,5, 7 };
    private List<int> enter = new List<int> { 6, 7, 8, 10, 11 };

	// Use this for initialization
	void Start ()
    {
        tutorialController = gameObject.GetComponentInParent<TutorialController>();
        code = int.Parse(this.name.Substring(this.name.Length - 2, 2));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player) && enter.Contains(code)) {
            tutorialController.EnterNotify(code);
            Debug.Log("enter code "+code);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(Tags.Player) && exit.Contains(code)) {
            tutorialController.ExitNotify(code);
            Debug.Log("exit code " + code);
        }
    }


}
