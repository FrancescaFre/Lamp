using UnityEngine;
using UnityEngine.UI;

public class StaminaHUD : MonoBehaviour {
    public Slider staminaSlider;

 

    private void Start()
    {
        staminaSlider = GetComponent<Slider>();
    }
}
