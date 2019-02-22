using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class TutorialGUI : MonoBehaviour
{

    public GameObject firstSelected;
    public GameObject panel;
    public Button[] entries;

    private TutorialStep selected; // The selected tutorial block
    private bool insideTutorial; // True when a tutorial is active on the screen

    // Use this for initialization
    void Start()
    {
        insideTutorial = false;
        gameObject.SetActive(false);

        if (FindObjectOfType<TutorialManager>())
            foreach (Button entry in entries)
                entry.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (insideTutorial)
            if (Input.GetKeyDown(KeyCode.Tab))
                if (!selected.Next())
                    Back();
                else if (Input.GetKeyDown(KeyCode.Escape))
                    Back();
    }

    public void ReadTutorial(TutorialStep tutorial)
    {
        selected = tutorial;
        insideTutorial = true;
        InGameHUD.Instance.tabTutorial.gameObject.SetActive(true);
        selected.transform.SetParent(transform);
        panel.SetActive(false);
        selected.gameObject.SetActive(true);
        selected.Refresh();
    }

    public void Back()
    {
        selected.gameObject.SetActive(false);
        panel.SetActive(true);
        selected.transform.SetParent(panel.transform);
        selected.Refresh();
        insideTutorial = false;
        InGameHUD.Instance.tabTutorial.gameObject.SetActive(false);
        selected = null;
        EventSystem.current.SetSelectedGameObject(firstSelected, null);
    }

    public void Escape()
    {
        if (insideTutorial)
            Back();
        gameObject.SetActive(false);
    }
}
