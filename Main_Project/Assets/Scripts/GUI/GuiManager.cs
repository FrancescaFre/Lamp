using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GuiManager : MonoBehaviour {
    public static GuiManager GUIInstance;

    [Header("Menu list")]
    public GameObject Galaxies;
    public GameObject Characters;

    [Header("First selected object in each menu")]
    public GameObject firstSelectedGalaxy;
    public GameObject firstSelectedCharacter;

    [Header("Buttons to the next menu")]
    public Button nextButton;
    public Button PlayButton;

    [Header("Items SFX")]
    public AudioClip HoverSound;
    public AudioClip ConfirmSound;
    public AudioClip AbortSound;

    private void Awake() {
        if (!GUIInstance)
            GUIInstance = this;
        else {
            Destroy(gameObject);
            return;
        }


        
    }
    private void Start() {

        Galaxies.SetActive(true);
        Characters.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(firstSelectedGalaxy, null);

        var x = GetComponentsInChildren<ElementSoundGUI>(includeInactive:true);

        foreach(var el in x) {
            el.hoverSound = HoverSound;
            el.confirmSound = ConfirmSound;
            el.cancelSound = AbortSound;
        }
    }

    /// <summary>
    /// Change the panel to the team selection
    /// </summary>
    public void CheckSelectedLevel() {
        if (GameManager.Instance.levelLoaded == null) {
            nextButton.interactable = false;
            nextButton.transform.GetChild(0).gameObject.SetActive(nextButton.interactable);
            return;
        }

        
        Galaxies.SetActive(false);
        Characters.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelectedCharacter, null);
        AudioManager.Instance.SFXSource.PlayOneShot(ConfirmSound);

    }
    


    public void CheckSelectedTeam() {
        if (GameManager.Instance.TeamList != null && GameManager.Instance.TeamList.Count == 3 && GameManager.Instance.levelLoaded)
            GameManager.Instance.LoadGame();
        else {
            PlayButton.interactable = false;
           // PlayButton.transform.GetChild(0).gameObject.SetActive(PlayButton.interactable);
            PlayButton.GetComponent<BaseButtonGUI>().StopHalo(); 
        }
    }

    //to turn back once reached the team selection
    public void BackToGalaxySelect() {
        
        Characters.SetActive(false);
        Galaxies.SetActive(true);
        EventSystem.current.SetSelectedGameObject(nextButton.gameObject, null);
        AudioManager.Instance.SFXSource.PlayOneShot(AbortSound);
    }
}
