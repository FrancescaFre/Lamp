using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CastingCircle : MonoBehaviour {

    public Image bar; // The bar that fills when casting

    [Range(1, 120)]
    public float castingTime; // Frames needed to charge
    private float _progress; // Actual progress

    private bool _isCasting; // The update runs only if it's true

    void Start()
    {
        _progress = 0;
    }

    // Update is called once per frame
    void Update ()
    {
        if (_isCasting)
            if (_progress <= castingTime)
            {
                _progress++;
                bar.fillAmount += 1 / castingTime;
            }
	}

    public void Cancel()
    {
        _progress = 0;
        bar.fillAmount = 0;
        gameObject.SetActive(false);
    }
}
