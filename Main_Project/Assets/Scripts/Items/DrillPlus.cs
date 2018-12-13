using UnityEngine;

public class DrillPlus : MonoBehaviour {

    void Update()
    {
        transform.position = transform.position + transform.up * Mathf.Sin(Time.time * 2f) * Time.deltaTime * 0.3f;
    }

    void OnCollisionEnter(Collision player)
    {
        if (player.collider.CompareTag("Player"))
        {
            GameManager.Instance.currentPC.digCount++;
            Destroy(gameObject);
        }
    }
}
