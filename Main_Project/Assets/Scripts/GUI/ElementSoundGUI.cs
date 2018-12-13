using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ElementSoundGUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,ISelectHandler,ISubmitHandler,ICancelHandler {
    [Header("RESOURCES")]
    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioClip cancelSound;


    [Header("SETTINGS")]
    public bool enableHoverSound = true;
    public bool enableClickSound = true;
    public bool isSelectable=true;


    private AudioSource HoverSource { get { return GetComponent<AudioSource>(); } }
    private AudioSource ClickSource { get { return GetComponent<AudioSource>(); } }
    private AudioSource CancelSource { get { return GetComponent<AudioSource>(); } }
    private Button isButton;

    void Start() {
        gameObject.AddComponent<AudioSource>(); //adds the audiosource at runtime
        isButton = GetComponent<Button>();  // get the button component (if any)
        HoverSource.clip = hoverSound;
        ClickSource.clip = clickSound;
        CancelSource.clip = clickSound;
        HoverSource.volume = .5f;
        HoverSource.playOnAwake = false;
        ClickSource.playOnAwake = false;
        CancelSource.playOnAwake = false;
    }


    #region With Mouse
  
    public void OnPointerEnter(PointerEventData eventData) {
        HoverSound();
    }

    
    public void OnPointerClick(PointerEventData eventData) {
        SelectSound();
    }
    #endregion

    #region With JoyPad

    public void OnSelect(BaseEventData eventData) {
        HoverSound();
    }
    public void OnSubmit(BaseEventData eventData) {
        SelectSound();
    }
    #endregion

    public void OnCancel(BaseEventData eventData) {
        CancelSound();
    }
    /// <summary>
    /// Play sound on hover/selection (if the object is a button it has to be interactable) 
    /// </summary>
    private void HoverSound() {
        if (!isButton) {
            if (enableHoverSound && HoverSource) {
                HoverSource.PlayOneShot(hoverSound);
            }
        }
        else {
            if (isButton.interactable && HoverSource) {
                HoverSource.PlayOneShot(hoverSound);
            }
        }
    }


    /// <summary>
    /// Play sound on click/submit (if the object is a button it has to be interactable) 
    /// </summary>
    /// <param name="eventData"></param>
    private void SelectSound() {
        if (!isSelectable) return;  //don't play any sound if the object is not selectable

        if (isButton) {

            if (isButton.interactable &&ClickSource)
                ClickSource.PlayOneShot(clickSound);
        }
    }

    private void CancelSound() {
        CancelSource.PlayOneShot(cancelSound);
    }

}
