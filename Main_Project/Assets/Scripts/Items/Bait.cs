using UnityEngine;
using UnityEngine.UI;

public class Bait : MonoBehaviour {

    [Range(300, 600)]
    public float baitDuration = 420f;

    public Image baitGauge;
    public Image baitProgress;

    private bool _picked;
    private bool _placed;

    private float _progress;

    void Start()
    {
        _picked = false;
        _placed = false;

        _progress = 0f;
    }

    // Update is called once per frame
    void Update () {
        if (!_picked)
            transform.position = transform.position + transform.up * Mathf.Sin(Time.time * 2f) * Time.deltaTime * 0.3f;

        if (_placed)
            CountDown();
    }

    void OnCollisionEnter(Collision player)
    {
        if (player.collider.CompareTag(Tags.Player))
        {
            _picked = true;
            Destroy(gameObject);
        }
    }

    public void Place()
    {
        _placed = true;
        transform.SetPositionAndRotation(GameManager.Instance.currentPC.transform.position, GameManager.Instance.currentPC.transform.rotation);
        gameObject.SetActive(true);
        LureEnemy();
    }

    private void CountDown()
    {
        _progress++;
        baitProgress.fillAmount += 1.0f / baitDuration;

        if (_progress >= baitDuration)
        {
            baitProgress.fillAmount = 0;
            baitGauge.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void LureEnemy()
    {
        //TODO
    }
}
