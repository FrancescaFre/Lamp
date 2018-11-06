using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// The waiting bar that appears before digging
/// </summary>
public class Caster : MonoBehaviour
{

    public Image bar; // The bar that fills when casting
    public PlayerController player;  // The player. Needed to access its methods (we could use singleton pattern tho)

    [Range(1, 120)]
    public float castingTime; // Frames needed to charge (120 frames = 2 seconds)

    private float _progress; // Actual progress
    private Dig _digType; // Used to know which Dig to execute

    void Update()
    {
        _progress++;
        bar.fillAmount += 1.0f / castingTime;

        if (_progress >= castingTime)
        {
            if (_digType == Dig.LINEAR)
                player.LinearDig();
            else
                player.ZoneDig();

            Cancel();
        }
    }

    /// <summary>
    /// Awakens and sets the Caster for the actual dig
    /// </summary>
    /// <param name="digType"></param>
    public void StartCircle(Dig digType)
    {
        _progress = 0;
        _digType = digType;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Deactivates and resets the Caster after a dig
    /// </summary>
    public void Cancel()
    {
        _progress = 0;
        bar.fillAmount = 0;
        _digType = Dig.NONE;
        gameObject.SetActive(false);
    }
}
