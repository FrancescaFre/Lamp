using UnityEngine;

public class WallBuilder : MonoBehaviour {

    public GameObject prefabToBuild;
    public Transform parent;

    [Tooltip("Maximum Size = 8")]
    public GameObject[] buildingBlocks;

    private Vector3 lastStep;
    private float distance;
    private float stepLength;

    //-------------------------------------------------------------------------

    void Start()
    {
        stepLength = prefabToBuild.transform.localScale.x;
        lastStep = gameObject.transform.position;
        distance = 0;
    }

    void FixedUpdate ()
    {
        distance = Vector3.Distance(gameObject.transform.position, lastStep);
        //if (Input.GetKeyDown(KeyCode.E))
        if (distance >= stepLength)
        {
            Instantiate(prefabToBuild, gameObject.transform.position, gameObject.transform.rotation, parent);
            lastStep = gameObject.transform.position;
        }
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            enabled = false;

        BuildBlock(Input.inputString);        
    }

    //-------------------------------------------------------------------------

    private void BuildBlock(string input)
    {
        switch (input)
        {
            case "1":
                Instantiate(buildingBlocks[0], gameObject.transform.position, gameObject.transform.rotation);
                break;
            case "2":
                Instantiate(buildingBlocks[1], gameObject.transform.position, gameObject.transform.rotation);
                break;
            case "3":
                Instantiate(buildingBlocks[2], gameObject.transform.position, gameObject.transform.rotation);
                break;
            case "4":
                Instantiate(buildingBlocks[3], gameObject.transform.position, gameObject.transform.rotation);
                break;
            case "5":
                Instantiate(buildingBlocks[4], gameObject.transform.position, gameObject.transform.rotation);
                break;
            case "6":
                Instantiate(buildingBlocks[5], gameObject.transform.position, gameObject.transform.rotation);
                break;
            case "7":
                Instantiate(buildingBlocks[6], gameObject.transform.position, gameObject.transform.rotation);
                break;
            case "8":
                Instantiate(buildingBlocks[7], gameObject.transform.position, gameObject.transform.rotation);
                break;
        }
    }
}
