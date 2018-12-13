using UnityEngine;

public class WallBuilder : MonoBehaviour {

    public GameObject prefabToBuild;
    public Transform parent;

    [Tooltip("Maximum Size = 8")]
    public GameObject[] buildingBlocks;

    private Vector3 lastStep;
    private float distance;
    private float stepLength;
    private bool continuousBuild = true;

    //-------------------------------------------------------------------------

    void Start()
    {
        stepLength = prefabToBuild.transform.localScale.x;
        lastStep = gameObject.transform.position;
        distance = 0;
    }

    void FixedUpdate ()
    {
        if (continuousBuild)
        {
            distance = Vector3.Distance(gameObject.transform.position, lastStep);
            //if (Input.GetKeyDown(KeyCode.E))
            if (distance >= stepLength)
            {
                Instantiate(prefabToBuild, gameObject.transform.position, gameObject.transform.rotation, parent);
                lastStep = gameObject.transform.position;
            }
        }
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            continuousBuild = false;

        BuildBlock(Input.inputString);        
    }

    //-------------------------------------------------------------------------

    private void BuildBlock(string input)
    {
        switch (input)
        {
            case "1":
                InstantiateOnGround(buildingBlocks[0], gameObject.transform.position, gameObject.transform.rotation);
                break;
            case "2":
                InstantiateOnGround(buildingBlocks[1], gameObject.transform.position, gameObject.transform.rotation);
                break;
            case "3":
                InstantiateOnGround(buildingBlocks[2], gameObject.transform.position, gameObject.transform.rotation);
                break;
            case "4":
                InstantiateOnGround(buildingBlocks[3], gameObject.transform.position, gameObject.transform.rotation);
                break;
            case "5":
                InstantiateOnGround(buildingBlocks[4], gameObject.transform.position, gameObject.transform.rotation);
                break;
            case "6":
                InstantiateOnGround(buildingBlocks[5], gameObject.transform.position, gameObject.transform.rotation);
                break;
            case "7":
                InstantiateOnGround(buildingBlocks[6], gameObject.transform.position, gameObject.transform.rotation);
                break;
            case "8":
                InstantiateOnGround(buildingBlocks[7], gameObject.transform.position, gameObject.transform.rotation);
                break;
        }
    }

    private void InstantiateOnGround(GameObject go, Vector3 position, Quaternion rotation)
    {
        var obj = Instantiate(go, position, rotation);
        obj.transform.Translate(new Vector3(0,-.25f,0), Space.Self);
    }
}
