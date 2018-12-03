using UnityEngine;
using UnityEngine.UI;

public class Lantern : MonoBehaviour
{
    [Range(300, 600)]
    public float lightDuration = 420f;

    public Image lanternGauge;
    public Image lanternProgress;

    private float _progress = 0f;
    private bool _picked;
    private PlayerController _player;

    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _picked = false;
        lanternGauge.gameObject.SetActive(true);
    }

    void Update()
    {
        _progress++;
        lanternProgress.fillAmount += 1.0f / lightDuration;

        if (_progress >= lightDuration)
        {
            lanternProgress.fillAmount = 0;
            lanternGauge.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision player)
    {
        if (!_picked && player.collider.CompareTag("Player"))
        {
            transform.SetParent(_player.transform);
            transform.localPosition = new Vector3(0.5f, -0.5f, 0.5f);
            transform.localScale = new Vector3(2, 2, 2);
            Destroy(gameObject);
        }
    }
}
