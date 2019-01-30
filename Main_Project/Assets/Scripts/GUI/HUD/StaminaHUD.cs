using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaHUD : MonoBehaviour {
    public Slider staminaSlider;

    public float MaxStamina { get; set; }

    private void Start()
    {
        staminaSlider = GetComponent<Slider>();
    }
}
