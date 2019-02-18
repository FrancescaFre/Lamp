using System.Collections.Generic;
using UnityEngine;
using MEC;
public class EnemyEnabler : MonoBehaviour {
   
    [Tooltip("Half of the size of the box in each dimension.")]
    public Vector3 area;
    public float waitingTime = 3f;

    public Collider[] playerCollider;
    public List<Enemy> containedEnemies;

    private void Start() {

        containedEnemies = new List<Enemy>(GetComponentsInChildren<Enemy>());

        Timing.RunCoroutine(CheckPlayer());
    }

    //private IEnumerator<float> CheckPlayer() {
    private IEnumerator<float> CheckPlayer() {

        while (true) {

            playerCollider = Physics.OverlapBox(transform.position, area, Quaternion.identity, LayerMask.GetMask(Tags.Player));
            

            if (playerCollider.Length > 0) {
                for (int i = 0; i < containedEnemies.Count; i++) {
                    containedEnemies[i].gameObject.SetActive(true);
                    yield return Timing.WaitForOneFrame;
                }
            }
            else {
                for (int i = 0; i < containedEnemies.Count; i++) {

                    if (containedEnemies[i].currentStatus == EnemyStatus.WANDERING)
                        containedEnemies[i].gameObject.SetActive(false);
                    else
                        Timing.RunCoroutine(WaitEnemy(containedEnemies[i]));

                    yield return Timing.WaitForOneFrame;
                }
            }

            yield return Timing.WaitForSeconds(waitingTime);
            //yield return Timing.WaitForOneFrame;
            
        }

    }

    private IEnumerator<float> WaitEnemy(Enemy en) {

        do {

            yield return Timing.WaitForSeconds(1f);
            if (en.currentStatus == EnemyStatus.WANDERING)
                en.gameObject.SetActive(false);
        } while (en.currentStatus != EnemyStatus.WANDERING);
    }


}
