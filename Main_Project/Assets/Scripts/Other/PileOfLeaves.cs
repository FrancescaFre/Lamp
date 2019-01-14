using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileOfLeaves : MonoBehaviour
{
    private Collider[] colliders;
    public int radiusIsSneaking;
    public int radiusNotSneaking;
    public int radius;

    public float walkSpeedInWater;


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(Tags.Player))
        {

            radius = other.GetComponent<PlayerController>().isSneaking ? radiusIsSneaking : radiusNotSneaking;

            colliders = Physics.OverlapSphere(this.transform.position, radius, LayerMask.GetMask("Enemy"));

            foreach (Collider enemy in colliders)
            {
                float dstToTarget = Vector3.Distance(transform.position, enemy.transform.position);
                Vector3 dirToTarget = (enemy.transform.position - transform.position).normalized;
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, LayerMask.GetMask("Obstacles")))
                    enemy.GetComponent<Enemy>().Allert(other.GetComponent<PlayerController>());
            }
        }
    }
}
