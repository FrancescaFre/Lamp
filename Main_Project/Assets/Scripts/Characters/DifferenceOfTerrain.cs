using UnityEngine;

public class DifferenceOfTerrain : MonoBehaviour {

    public Transform modelTransform;
    private Vector3 originPos;
    private bool onWater;
    [Tooltip("How much up/down the model has to go to match the height level of the terrain.")]
    [Range(-3f, 3f)]
    public float height = 0;


    
    void Awake() {
        foreach (Transform child in transform)
            if (child.CompareTag(Tags.PlayerModel))
                modelTransform = child;

        originPos  = modelTransform.localPosition;
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.Water) && !onWater) {

            Vector3 temp = modelTransform.localPosition;
            temp.y += height;
            modelTransform.localPosition = temp;
            onWater = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag(Tags.Water) && !onWater) {

            Vector3 temp = modelTransform.localPosition;
            temp.y += height;
            modelTransform.localPosition = temp;
            onWater = true;
        }

    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(Tags.Water) && onWater) {

            modelTransform.localPosition = originPos;
            onWater = false;
        }
    }

}
