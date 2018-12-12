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

        lanternGauge = InGameHUD.Instance.InGameHUDPanel.transform.Find("Gauge Panel").Find("Lantern").GetComponent<Image>();
        lanternProgress = lanternGauge.transform.GetChild(0).GetComponent<Image>();
        lanternGauge.gameObject.SetActive(false);
        gameObject.SetActive(false);
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
            PickUp();
    }

    private void PickUp()
    {
        _picked = true;
        _player.Lantern = this;
        gameObject.SetActive(false);
        Destroy(GetComponentInChildren<ParticleSystem>().gameObject);
    }

    public void Use()
    {
        transform.SetParent(_player.transform);
        transform.localPosition = new Vector3(0.5f, -0.5f, 0.5f);
        transform.rotation = _player.transform.rotation;
        transform.localScale = new Vector3(2, 2, 2);

        GetComponentInChildren<SphereCollider>().enabled = true;
        GetComponentInChildren<Light>(includeInactive: true).gameObject.SetActive(true);
        lanternGauge.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }
}
