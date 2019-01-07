using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDig : Digging {

    public GameObject movingCirclePrefab;
    private MovingCircle _movingCircle;

    public float radius = 10f; 
    public SphereCollider sphereC;
    public  Collider[] colliders; 

    private void Awake()
    { 
        sphereC = this.gameObject.AddComponent<SphereCollider>();
        sphereC.isTrigger = true;
        sphereC.radius = radius;
        sphereC.enabled = false; 
    }

    /// <summary>
    /// Performs the digging action (called by caster)
    /// </summary>
    override public void Dig()
    {
        base.Dig();
        player.GetComponent<Rigidbody>().MovePosition(_movingCircle.transform.position); // Moves the player right on top of the target

        // After digging
        Cancel();
        player.IsCasting = false;
        player.digCount--;
    }

    override public void CheckInput()
    {
        sphereC.enabled = true;
        gameObject.layer = 14; // TESTING -> custom layer for SphereC

        if (player.IsZoneDigging) // If you already pressed [ZDIG] 2 times (activate -> valid start -> now)
            if (_movingCircle.CanDig())
            {
                StartCasting();
                player.IsCasting = true;
            }
            else
            {
                Cancel();
            }

        else if (isActiveAndEnabled) // If you already pressed [ZDIG] (activate -> now)
            if (CanDig())
            {
                _movingCircle = Instantiate(movingCirclePrefab).GetComponent<MovingCircle>();
                _movingCircle.Setup(transform, player);
                player.IsZoneDigging = true;
            }
            else
            {
                Cancel();
            }

        else if (player.VDig.isActiveAndEnabled) // If you already pressed [VDIG]
        {
            player.VDig.Cancel();    
        }

        else // First time the player presses [ZDIG]
        {
            gameObject.SetActive(true);
        }
    }

    public override void Cancel()
    {
        if (player.IsZoneDigging)
        {
            Destroy(_movingCircle.gameObject);
            _movingCircle = null;
            player.IsZoneDigging = false;
        }

        gameObject.SetActive(false);
        SwitchOffCollider();
       
    }

    private void SwitchOffCollider()
    {
        colliders = Physics.OverlapSphere(this.transform.position, radius);
        foreach (Collider collider in colliders)
            if (collider.GetComponent<Walls>())
                collider.GetComponent<Walls>().SwitchOff();
        sphereC.enabled = false;
    }
}
