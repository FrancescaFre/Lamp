using UnityEngine;

public class VerticalDig : Digging
{
    /// <summary>
    /// Performs the digging action (called by caster)
    /// </summary>
    override public void Dig()
    {
        base.Dig();
        player.GetComponent<Rigidbody>().MovePosition(-(player.transform.position)); // Moves the player on the other side of the world
      
        // After digging
        Cancel();
        player.IsCasting = false;
        player.digCount--;
    }

    /// <summary>
    /// Checks any result after pressing [VDIG]
    /// </summary>
    override public void CheckInput ()
    {
        if (isActiveAndEnabled) // If you already pressed [VDIG]
            if (canDig)
            {
                StartCasting();
                player.IsCasting = true;
            }
            else
                Cancel();

        else if (player.ZDig.isActiveAndEnabled) // If you already pressed [ZDIG]
            player.ZDig.Cancel();

        else // If it's the first time you press [VDIG]
            gameObject.SetActive(true); 
    }
}
