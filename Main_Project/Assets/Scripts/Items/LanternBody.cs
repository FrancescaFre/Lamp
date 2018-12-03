using UnityEngine;

public class LanternBody : MonoBehaviour
{
    private PlayerController _player;
    public GameObject lantern;

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
            // TODO: ADD ITEM TO INVENTORY
            lantern.SetActive(true); // REPLACE THIS
            Destroy(gameObject);
        }
    }
}
