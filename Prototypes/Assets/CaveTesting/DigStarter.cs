using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigStarter : MonoBehaviour {

    public Material DigYes;
    public Material DigNo;

    // This is only for the vertical dig. The type 2 is within DigTarget
    public void Dig()
    {
        // TODO: Vertical dig
        Debug.Log("Scavo 1!");     
    }

    // Colors the circle under the player in green or red, based on various checks
    // _____Type 1 for vertical dig
    // _____Type 2 for targeted dig
    public void CheckDig(int digType) 
    {
        if (Conditions(digType))
            GetComponent<MeshRenderer>().material = DigYes;
        else
            GetComponent<MeshRenderer>().material = DigNo;
    }

    public void StopDig(out int digType)
    {
        gameObject.SetActive(false);
        digType = 0;
    }

    // Returns true if the starting circle is green
    public bool CanDig(int digType)
    {
        return (Conditions(digType));
    }

    // Checks if the player is in a valid starting position to dig
    // _____Type 1 for vertical dig
    // _____Type 2 for targeted dig
    private bool Conditions(int digType)
    {
        // TODO: check terrain ↓
        if (transform.position.z >= 0)
            return false;
        else if (digType == 1)
            return true; // TODO: VERTICAL RAYCAST?
        else
            return true; // For the targeted dig there's no other check apart of the terrain
    }
}
