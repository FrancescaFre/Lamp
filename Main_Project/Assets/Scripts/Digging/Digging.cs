using UnityEngine;
using UnityEngine.UI;

public abstract class Digging : MonoBehaviour {

    public Material digYes;
    public Material digNo;
    public PlayerController player;

    [Range(1, 120)]
    public float castingTime; 
    protected PlayerMovement _pm;
    protected float _progress; // Actual casting progress  

    /*#region Trigger Interaction

    protected void OnTriggerEnter(Collider terrain)
    {
        if (terrain.gameObject.CompareTag("Solid") ||
            terrain.gameObject.CompareTag("Water") ||
            terrain.gameObject.CompareTag("Ice") ||
            terrain.gameObject.CompareTag("Leaves"))
        {
            canDig = false;
        }
    }

    protected void OnTriggerStay(Collider terrain)
    {
        if (terrain.gameObject.CompareTag("Solid") ||
            terrain.gameObject.CompareTag("Water") ||
            terrain.gameObject.CompareTag("Ice") ||
            terrain.gameObject.CompareTag("Leaves"))
        {
            canDig = false;
        }
    }

    protected void OnTriggerExit(Collider terrain)
    {
        if (terrain.gameObject.CompareTag("Solid") ||
            terrain.gameObject.CompareTag("Water") ||
            terrain.gameObject.CompareTag("Ice") ||
            terrain.gameObject.CompareTag("Leaves"))
        {
            canDig = true;
        }
    }
    #endregion*/

    protected virtual void Start() {

        _pm = player.GetComponent<PlayerMovement>();

        gameObject.SetActive(false);

        if (player.CharacterPeriod == CharPeriod.VICTORIAN || player.CharacterPeriod == CharPeriod.PREHISTORY)
            castingTime = AnimationManager.Anim_LenghtAnim(player.characterAnimator, "Dig And Plant Seeds");
        if (player.CharacterPeriod == CharPeriod.ORIENTAL)
            castingTime = AnimationManager.Anim_LenghtAnim(player.characterAnimator, "orientalDIG");

        player.caster.maxValue = castingTime;

}

    protected virtual void Update() {
        //if (!caster.isActiveAndEnabled)
        if (!player.caster) return;
        if (!player.caster.isActiveAndEnabled)
            ChangeColor();
        else
            CastDig();
    }



    /// <summary>
    /// Performs the digging action (called by caster)
    /// </summary>
    public virtual void Dig() {
        if (player.digEffect) {
            player.digEffect.gameObject.SetActive(true);
            player.digEffect.Play();
            player.GetComponent<PlayerSFXEmitter>().DigEffect();//plays dig audio
        }
    }

    /// <summary>
    /// Enables the digging circle
    /// </summary>
    public abstract void CheckInput();

    /// <summary>
    /// Switches the circle's color between green and red
    /// </summary>
    protected virtual void ChangeColor() {
        if (CanDig())
            GetComponent<MeshRenderer>().material = digYes;
        else
            GetComponent<MeshRenderer>().material = digNo;
    }

    /// <summary>
    /// Checks if the terrain under the player is ok to be dug
    /// </summary>
    /// <returns></returns>
    public virtual bool CanDig() {
        return !( _pm.OnWater || _pm.OnSolidFloor);
    }

    /// <summary>
    /// Casts the dig during the casting update
    /// </summary>
    protected void CastDig() {
        _progress+=Time.deltaTime;
        player.caster.value=_progress;

        if (_progress >= castingTime) {
            Dig();
            StopCasting();
            Cancel();
        }
    }

    /// <summary>
    /// Pops up and starts the casting bar
    /// </summary>
    protected void StartCasting() {
        _progress = 0;
        player.caster.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides and resets the casting bar
    /// </summary>
    protected void StopCasting() {
        _progress = 0;
        player.caster.value = 0;
        player.caster.gameObject.SetActive(false);
    }

    /// <summary>
    /// Hides the dig circle
    /// </summary>
    public virtual void Cancel() {
        gameObject.SetActive(false);
    }

    protected void HideDrillGO(){
        player.drillGO.SetActive(false);
    }
}
