using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ElementSoundGUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,ISelectHandler,ISubmitHandler,ICancelHandler {
    [Header("RESOURCES")]
    public AudioClip hoverSound;
    public AudioClip confirmSound;
    public AudioClip cancelSound;


    [Header("SETTINGS")]
    public bool enableHoverSound = true;
    public bool enableClickSound = true;
    public bool isSelectable=true;

    private Button isButton;

    void Start() {
       
        isButton = GetComponent<Button>();  // get the button component (if any)

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
            if (enableHoverSound) {
                AudioManager.Instance.SFXSource.PlayOneShot(hoverSound);
            }
        }
        else {
            if (isButton.interactable ) {
                AudioManager.Instance.SFXSource.PlayOneShot(hoverSound); 
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

            if (isButton.interactable )
                AudioManager.Instance.SFXSource.PlayOneShot(confirmSound);
        }
    }

    private void CancelSound() {
        AudioManager.Instance.SFXSource.PlayOneShot(cancelSound);
    }

}
