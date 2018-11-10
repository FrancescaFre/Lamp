using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The dig circle under the player, checks the starting
/// position's terrain
/// </summary>
public class DigStarter : MonoBehaviour
{

    public Material digYes;
    public Material digNo;

    private DigType _digType; // Actual digging state (None, Linear, Zone)

    /*
    /// <summary>
    /// Moves the player on the other side of the planet.
    /// Actually unused.
    /// </summary>
    public void Dig()
    {
        //Debug.Log("Scavo 1!");
    }
    */

    /// <summary>
    /// Changes the color of the circle
    /// </summary>
    /// <param name="digType"></param>
    public void CheckDig(DigType digType)
    {
        if (Conditions(digType))
            GetComponent<MeshRenderer>().material = digYes;
        else
            GetComponent<MeshRenderer>().material = digNo;
    }

    /// <summary>
    /// Hides the circle
    /// </summary>
    /// <param name="digType"></param>
    public void StopDig (ref DigType digType)
    {
        gameObject.SetActive(false);
        digType = DigType.NONE;
    }

    /// <summary>
    /// Checks if the starting terrain is diggable. Also checks the ending point for linear dig
    /// </summary>
    /// <param name="digType"></param>
    /// <returns></returns>
    public bool CanDig(Digging digType)
    {
        return (Conditions(digType));
    }

    /// <summary>
    /// Checks the conditions for the linear dig (valid terrain both
    /// at start and end) and eventually awakes the Casting Circle
    /// </summary>
    public void Activate (DigType dt)
    {
        _digType = dt;

        if (_digType == DigType.LINEAR) // If you already pressed [LDIG]
            if (CanDig(_digType))
            {
                caster.StartCircle(_digType);
                IsCasting = true;
            }
            else
                StopDig(ref _digType);

        else if (_digType == DigType.ZONE) // If you press [LDIG] after [ZDIG] it cancels the digging action
            StopDig(ref _digType);

        else // First time the player presses [LDIG]
        {
            _digType = DigType.LINEAR;
            gameObject.SetActive(true);
            CheckDig(_digType);
        }
    }

    // Checks if the player is in a valid starting position to dig
    private bool Conditions(DigType digType)
    {
        // TODO: check terrain ↓
        if (transform.position.z >= 0)
            return false;
        else if (digType == DigType.LINEAR)
            return true; // TODO: VERTICAL RAYCAST?
        else
            return true; // For the targeted dig there's no other check apart of the terrain
    }
}