using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// The waiting bar that appears before digging
/// </summary>
public class Caster : MonoBehaviour
{

    public Image bar; // The bar that fills when casting
    public Digging digging;

    [Range(1, 120)]
    public float castingTime; // Frames needed to charge (120 frames = 2 seconds)

    private float _progress; // Actual progress

    void Update()
    {
        _progress++;
        bar.fillAmount += 1.0f / castingTime;

        if (_progress >= castingTime)
        {
            digging.Dig();
            Cancel();
        }
    }

    /// <summary>
    /// Awakens and sets the Caster for the actual dig
    /// </summary>
    /// <param name="digType"></param>
    public void StartCircle(DigType digType)
    {
        _progress = 0;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Deactivates and resets the Caster after a dig
    /// </summary>
    public void Cancel()
    {
        _progress = 0;
        bar.fillAmount = 0;
        gameObject.SetActive(false);
    }
}
