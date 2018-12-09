using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRotate : MonoBehaviour
{

    public bool test1;
    public bool test2;
    public bool test3;

    // Update is called once per frame
    void Update()
    {

        if (test1)
            transform.Rotate(new Vector3(0, 360 / 5 * Time.deltaTime, 0), Space.Self);
        /*    if (test3)

                    transform.Rotate(new Vector3(0, 5/360 * Time.deltaTime, 0), Space.Self);
            if (test2)
                transform.Rotate(transform.up, 5/360 * Time.deltaTime *5);
        }*/
    }
}

