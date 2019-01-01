using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GUIManager : MonoBehaviour {
    public static GUIManager GUIInstance;

    [Header("Menu list")]
    public GameObject Galaxies;
    public GameObject Characters;

    [Header("First selected object in each menu")]
    public GameObject firstSelectedGalaxy;
    public GameObject firstSelectedCharacter;

    [Header("Buttons to the next menu")]
    public Button nextButton;
    public Button PlayButton;

    [Header("Confirm")]
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

    }

    /// <summary>
    /// Change the panel to the team selection
    /// </summary>
    public void CheckSelectedLevel() {
        if (GameManager.Instance.levelLoaded == null) {
            nextButton.interactable = false;
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
        else
            PlayButton.interactable = false;

    }

    //to turn back once reached the team selection
    public void BackToGalaxySelect() {
        
        Characters.SetActive(false);
        Galaxies.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelectedGalaxy, null);
        AudioManager.Instance.SFXSource.PlayOneShot(AbortSound);
    }
}
