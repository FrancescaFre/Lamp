using UnityEngine;
using UnityEngine.UI;

public abstract class Digging : MonoBehaviour {

    public Material digYes;
    public Material digNo;
    public PlayerController player;

    [Range(1, 120)]
    public int castingTime; // Frames needed to charge (120 frames = 2 seconds)
    public Image caster; // The caster to pop up
    public Image bar; // The bar that fills when casting

    protected bool canDig = true;

    protected int _progress; // Actual casting progress  
   
    #region Trigger Interaction

    protected void OnTriggerEnter(Collider obstacle)
    {
        if (obstacle.gameObject.CompareTag("Solid") ||
            obstacle.gameObject.CompareTag("Water") ||
            obstacle.gameObject.CompareTag("Ice") ||
            obstacle.gameObject.CompareTag("Leaves"))
        {
            canDig = false;
        }
    }

    protected void OnTriggerExit(Collider obstacle)
    {
        if (obstacle.gameObject.CompareTag("Solid") ||
            obstacle.gameObject.CompareTag("Water") ||
            obstacle.gameObject.CompareTag("Ice") ||
            obstacle.gameObject.CompareTag("Leaves"))
        {
            canDig = true;
        }
    }
    #endregion

    private void Start() {
        caster = InGameHUD.Instance.InGameHUDPanel.transform.Find("Gauge Panel").Find("Caster").GetComponent<Image>();
        bar = caster.transform.GetChild(0).GetComponent<Image>();
        caster.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (!caster.isActiveAndEnabled)
            ChangeColor();
        else
            CastDig();
    }

    /// <summary>
    /// Performs the digging action (called by caster)
    /// </summary>
    public virtual void Dig() {
        player.digEffect.gameObject.SetActive(true);
        player.digEffect.Play();
    }

    /// <summary>
    /// Enables the digging circle
    /// </summary>
    public abstract void CheckInput();

    /// <summary>
    /// Switches the circle's color between green and red
    /// </summary>
    protected void ChangeColor()
    {
        if (canDig)
            GetComponent<MeshRenderer>().material = digYes;
        else
            GetComponent<MeshRenderer>().material = digNo;
    }

    protected void CastDig()
    {
        _progress++;
        bar.fillAmount += 1.0f / castingTime;

        if (_progress >= castingTime)
        {
            Dig();
            StopCasting();
            Cancel();
        }
    }

    /// <summary>
    /// Pops up and starts the casting bar
    /// </summary>
    protected void StartCasting()
    {
        _progress = 0;
        caster.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides and resets the casting bar
    /// </summary>
    protected void StopCasting()
    {
        _progress = 0;
        bar.fillAmount = 0;
        caster.gameObject.SetActive(false);
    }

    /// <summary>
    /// Hides the dig circle
    /// </summary>
    public virtual void Cancel()
    {
        gameObject.SetActive(false);
    }
}
