using UnityEngine;

public class Autodestruct : MonoBehaviour {

    [Tooltip("Life Span of the object measured in seconds. Leave zero if you want the autodestruction to come from outside")]
    public int lifespan; // measured in seconds

    [Tooltip("Next object to pop up after the autodestruction")]
    public GameObject nextOne;
    public bool DieNow { get; set; }
    private int counter;

    private void Start()
    {
        DieNow = false;
        counter = 0;
    }

    private void Update()
    {
        counter++;
        if (DieNow || (lifespan != 0 && counter == lifespan * 60))
        {
            gameObject.SetActive(false);
            if (nextOne)
                nextOne.SetActive(true);
        }
    }
}
