using UnityEngine;

public class VerticalDig : Digging
{
    private bool otherSideIsWrong = false;

    private void OnTriggerEnter(Collider terrain)
    {
        if (terrain.CompareTag(Tags.Water) ||
            terrain.CompareTag(Tags.Leaves) ||
            terrain.CompareTag(Tags.Ice) ||
            terrain.CompareTag(Tags.Solid) ||
            terrain.gameObject.layer == 11)

            otherSideIsWrong = true;
    }

    private void OnTriggerStay(Collider terrain)
    {
        if (terrain.CompareTag(Tags.Water) ||
            terrain.CompareTag(Tags.Leaves) ||
            terrain.CompareTag(Tags.Ice) ||
            terrain.CompareTag(Tags.Solid) ||
            terrain.gameObject.layer == 11)

            otherSideIsWrong = true;
    }

    private void OnTriggerExit(Collider terrain)
    {
        if (terrain.CompareTag(Tags.Water) ||
            terrain.CompareTag(Tags.Leaves) ||
            terrain.CompareTag(Tags.Ice) ||
            terrain.CompareTag(Tags.Solid) ||
            terrain.gameObject.layer == 11)

            otherSideIsWrong = false;
    }

    // DEBUGGING
    override protected void Start()
    {
        base.Start();
        GetComponent<BoxCollider>().enabled = false;
    }
    // DEBUGGING

    /// <summary>
    /// Checks if the terrain under the player is ok to be dug
    /// </summary>
    /// <returns></returns>
    override public bool CanDig()
    {
        return !(_pm.OnIce ||  _pm.OnWater || _pm.OnSolidFloor || otherSideIsWrong);
    }

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
            if (CanDig())
            {
                player.drillGO.SetActive(true);
                AnimationManager.Anim_StarDigging(player.characterAnimator);
                StartCasting();
                Invoke("HideDrillGO", AnimationManager.Anim_LenghtAnim(player.characterAnimator, "Dig And Plant Seeds"));
                player.IsCasting = true;
            }
            else
                Cancel();

        else if (player.ZDig.isActiveAndEnabled) // If you already pressed [ZDIG]
            player.ZDig.Cancel();

        else // If it's the first time you press [VDIG]
            gameObject.SetActive(true); 
    }

    override public void Cancel()
    {
        base.Cancel();
        otherSideIsWrong = false;
    }
}
