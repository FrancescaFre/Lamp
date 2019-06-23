using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDig : Digging {
    public GameObject movingCirclePrefab;
    public MovingCircle movingCircle;

    public float radius = 10f;
    public SphereCollider sphereC;
    public Collider[] colliders;

    private void Awake() {
        sphereC = this.gameObject.AddComponent<SphereCollider>();
        sphereC.isTrigger = true;
        sphereC.radius = radius;
        sphereC.enabled = false;
    }
    protected override void ChangeColor() {
        
    }
    /// <summary>
    /// Performs the digging action (called by caster)
    /// </summary>
    override public void Dig() {
        base.Dig();
        player.GetComponent<Rigidbody>().MovePosition(movingCircle.transform.position); // Moves the player right on top of the target

        // After digging
        Cancel();
        player.IsCasting = false;
        GameManager.Instance.digCount--;
    }

    /*
     * TWO-STEPS ZONE DIG
     */

    override public void CheckInput() {
        sphereC.enabled = true;
        gameObject.layer = 14; // TESTING -> custom layer for SphereC

        if (player.IsZoneDigging) // If you already pressed [ZDIG] 2 times (activate -> valid start -> now)
            if (movingCircle && movingCircle.CanDig()) {
                player.drillGO.SetActive(true);
                AnimationManager.Anim_StarDigging(player.characterAnimator);
                if (player.CharacterPeriod == CharPeriod.ORIENTAL)

                    Invoke("HideDrillGO", AnimationManager.Anim_LenghtAnim(player.characterAnimator, "orientalDIG"));
                else
                    Invoke("HideDrillGO", AnimationManager.Anim_LenghtAnim(player.characterAnimator, "Dig And Plant Seeds"));
                StartCasting();
                player.IsCasting = true;
            }
            else {
                Cancel();
            }
        else if (player.VDig.isActiveAndEnabled) // If you already pressed [VDIG]
        {
            player.VDig.Cancel();
        }
        else if (CanDig()) {
            gameObject.SetActive(true);

            movingCirclePrefab.GetComponent<MovingCircle>().Setup(transform, player);
            movingCircle = Instantiate(movingCirclePrefab).GetComponent<MovingCircle>();
            player.IsZoneDigging = true;
        }
    }

    /*
     * THREE-STEPS ZONE DIG
     *
    override public void CheckInput()
    {
        sphereC.enabled = true;
        gameObject.layer = 14; // TESTING -> custom layer for SphereC

        if (player.IsZoneDigging) // If you already pressed [ZDIG] 2 times (activate -> valid start -> now)
            if (movingCircle && movingCircle.CanDig())
            {
                player.drillGO.SetActive(true);
                AnimationManager.Anim_StarDigging(player.characterAnimator);
                Invoke("HideDrillGO", AnimationManager.Anim_LenghtAnim(player.characterAnimator, "Dig And Plant Seeds"));
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
                movingCircle = Instantiate(movingCirclePrefab).GetComponent<MovingCircle>();
                movingCircle.Setup(transform, player);
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
    */

    public override void Cancel() {
        if (player.IsZoneDigging && movingCircle) {
            Destroy(movingCircle.gameObject);
            movingCircle = null;
            player.IsZoneDigging = false;
        }

        gameObject.SetActive(false);
        SwitchOffCollider();
    }

    private void SwitchOffCollider() {
        colliders = Physics.OverlapSphere(this.transform.position, radius);
        foreach (Collider collider in colliders)
            if (collider.GetComponent<Walls>())
                collider.GetComponent<Walls>().SwitchOff();
        sphereC.enabled = false;
    }
}