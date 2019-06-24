using System.Collections.Generic;
using UnityEngine;

public class DiggableWallLighter : MonoBehaviour {

    //Walls lighter
    [Range(5f,15f)]
    public float overlapRadius = 10f;
    public SphereCollider overlapOnWalls;
    public List<Collider> colliders;

    // Use this for initialization
    void Start () {
        overlapOnWalls = this.gameObject.AddComponent<SphereCollider>();
        overlapOnWalls.isTrigger = true;
        overlapOnWalls.radius = overlapRadius;
        overlapOnWalls.enabled = false;
        colliders = new List<Collider>();

        DigBehaviour.instance.wallLighter=this;
    }

    public void EnlightWalls() {
        overlapOnWalls.enabled = true;
        gameObject.layer = 14;

    }

    public void TurnOffWallLights() {
        colliders.AddRange(Physics.OverlapSphere(this.transform.position, overlapRadius));
        foreach (Collider collider in colliders)
            if (collider.GetComponent<Walls>())
                collider.GetComponent<Walls>().SwitchOff();
        overlapOnWalls.enabled = false;
        colliders.Clear();

    }
}
