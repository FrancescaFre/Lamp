using UnityEngine;

public class WorldEnlighter : MonoBehaviour {

    private Light _worldLight;
    private float _lerpStep;
    private VolumetricLight _fog;
    [Tooltip("TRUE if you only want to change the color of the fog.")]
    public bool changeColor = false;
    public Color endColor = Color.white;

    // Use this for initialization
    void Start() {
        _worldLight = GetComponent<Light>();
        GameManager.Instance.worldLight = this;
        _fog = GetComponent<VolumetricLight>();
        _lerpStep = 1f / GameManager.Instance.levelLoaded.allyLamps;
        _fog.ScatteringCoef = GameManager.Instance.levelLoaded.allyLamps / 10f;
    }

    public void Enlight() {
        if (changeColor)
            _worldLight.color = Color.Lerp(_worldLight.color, endColor, _lerpStep);
        else
            _fog.ScatteringCoef -= .1f;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T))
            Enlight();
    }
}
