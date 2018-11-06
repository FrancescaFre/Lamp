using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The dig circle under the player, checks the starting
/// position's terrain
/// </summary>
public class DigStarter : MonoBehaviour
{

    public Material DigYes;
    public Material DigNo;

    /// <summary>
    /// Moves the player on the other side of the planet.
    /// Actually unused.
    /// </summary>
    public void Dig()
    {
        //Debug.Log("Scavo 1!");
    }

    /// <summary>
    /// Changes the color of the circle
    /// </summary>
    /// <param name="digType"></param>
    public void CheckDig(Dig digType)
    {
        if (Conditions(digType))
            GetComponent<MeshRenderer>().material = DigYes;
        else
            GetComponent<MeshRenderer>().material = DigNo;
    }

    /// <summary>
    /// Hides the circle
    /// </summary>
    /// <param name="digType"></param>
    public void StopDig (ref Dig digType)
    {
        gameObject.SetActive(false);
        digType = global::Dig.NONE;
    }

    /// <summary>
    /// Checks if the starting terrain is diggable. Also checks the ending point for linear dig
    /// </summary>
    /// <param name="digType"></param>
    /// <returns></returns>
    public bool CanDig(Dig digType)
    {
        return (Conditions(digType));
    }

    // Checks if the player is in a valid starting position to dig
    private bool Conditions(Dig digType)
    {
        // TODO: check terrain ↓
        if (transform.position.z >= 0)
            return false;
        else if (digType == global::Dig.LINEAR)
            return true; // TODO: VERTICAL RAYCAST?
        else
            return true; // For the targeted dig there's no other check apart of the terrain
    }
}