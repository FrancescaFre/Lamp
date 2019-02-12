
using UnityEngine;

public class ItemsSFXEmitter : SFXEmitter {

    private void OnCollisionEnter(Collision collision) {
        Collider other = collision.collider;
        if (other.CompareTag(Tags.Player)) {
            PlayOneShot();

        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.Player)) {
            PlayOneShot();

        }
    }
}
