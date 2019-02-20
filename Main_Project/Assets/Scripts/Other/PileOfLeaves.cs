using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileOfLeaves : MonoBehaviour
{
    public Collider[] colliders;
    public int radiusIsSneaking;
    public int radiusNotSneaking;
    public int radius;

    public float walkSpeedInWater;


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(Tags.Player))
        {

            radius = other.GetComponent<PlayerController>().isSneaking ? radiusIsSneaking : radiusNotSneaking;
            Debug.Log(radius + " RAGGIO ");


            colliders = Physics.OverlapSphere(this.transform.position, radius, LayerMask.GetMask("Enemy"));
       
            foreach (Collider enemy in colliders)
            {
                Debug.Log(enemy.transform.name);
                float dstToTarget = Vector3.Distance(transform.position, enemy.transform.position);
                Vector3 dirToTarget = (enemy.transform.position - transform.position).normalized;
                Debug.DrawRay(transform.position, dirToTarget, Color.yellow);
                if (!Physics.Raycast(other.transform.position, dirToTarget, dstToTarget, LayerMask.GetMask("Obstacles")) && (!Physics.Raycast(other.transform.position, dirToTarget, dstToTarget, LayerMask.GetMask("UnDiggable"))) )
                    enemy.GetComponent<Enemy>().Allert(other.GetComponent<PlayerController>());
            }
        }
    }
}
