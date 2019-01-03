using UnityEngine;

public class TutorialStep : MonoBehaviour {

    public GameObject[] children;

    private int index, size;

    void Start()
    {
        index = 0;
        size = children.Length;
    }

    public bool Next()
    {
        children[index].SetActive(false);

        if (index == size - 1)
            return false;
            
        index++;
        children[index].SetActive(true);
        return true;
    }
}
