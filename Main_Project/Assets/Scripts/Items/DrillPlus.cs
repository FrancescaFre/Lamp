using UnityEngine;

public class DrillPlus : MonoBehaviour {

    private PlayerController _player;

    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        transform.position = transform.position + transform.up * Mathf.Sin(Time.time * 2f) * Time.deltaTime * 0.3f;
    }

    void OnCollisionEnter(Collision player)
    {
        if (player.collider.CompareTag("Player"))
        {
            _player.digCount++;
            Destroy(gameObject);
        }
    }
}
