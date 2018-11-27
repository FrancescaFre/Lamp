using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseButtonGUI : MonoBehaviour, ICancelHandler, IPointerClickHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {
    [Header("Halo Effect")]
    public GameObject haloParticle;
    public virtual void Awake() {
        haloParticle.SetActive(false);
    }

    public void StartHalo() {
        haloParticle.SetActive(true);
        Debug.Log("LUCE ATTIVA");
    }
    public void StopHalo() {
        haloParticle.SetActive(false);
        Debug.Log("LUCE SPENTA");
    }



    #region Event Handlers

    public virtual void OnPointerEnter(PointerEventData eventData) {
        StartHalo();
    }


    public virtual void OnSelect(BaseEventData eventData) {
        StartHalo();

    }
    public virtual void OnPointerExit(PointerEventData eventData) {
        StopHalo();
    }



    public virtual void OnDeselect(BaseEventData eventData) {
        StopHalo();
    }

    public virtual void OnPointerClick(PointerEventData eventData) {
      
    }
    public virtual void OnSubmit(BaseEventData eventData) {
        
    }
    public virtual void OnCancel(BaseEventData eventData) {
       
    }
    #endregion

}
