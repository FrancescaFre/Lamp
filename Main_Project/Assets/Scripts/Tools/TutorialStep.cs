using UnityEngine;
using UnityEngine.UI;

public class TutorialStep : MonoBehaviour
{

    public GameObject[] children;
    public Button menuEntry;

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

    public void Refresh()
    {
        children[index].SetActive(false);
        index = 0;
        children[index].SetActive(true);
    }

    public void AddMenuEntry()
    {
        if (menuEntry)
            menuEntry.interactable = true;
    }
}
