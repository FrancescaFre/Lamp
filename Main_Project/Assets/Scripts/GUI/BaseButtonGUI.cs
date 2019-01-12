using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseButtonGUI : MonoBehaviour, ICancelHandler, IPointerClickHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {
    [Header("Halo Effect")]
    public GameObject haloParticle;
    public bool needLock = false;
    public bool lockFX = false;

    public virtual void Awake() {
        
        haloParticle.SetActive(false);
    }

    public void StartHalo() {
        haloParticle.SetActive(true);

    }
    public void StopHalo() {
        haloParticle.SetActive(false);

    }

    public void ForceRelease() {
        lockFX = false;
        StopHalo();
    }

    #region Event Handlers

    public virtual void OnPointerEnter(PointerEventData eventData) {
        StartHalo();
    }


    public virtual void OnSelect(BaseEventData eventData) {
        StartHalo();

    }
    public virtual void OnPointerExit(PointerEventData eventData) {
        if (needLock && lockFX) return;
        StopHalo();
    }



    public virtual void OnDeselect(BaseEventData eventData) {
        if (needLock && lockFX) return;
        StopHalo();
    }

    public virtual void OnPointerClick(PointerEventData eventData) {
        lockFX = true;
    }
    public virtual void OnSubmit(BaseEventData eventData) {
        lockFX = true;
    }
    public virtual void OnCancel(BaseEventData eventData) {
        lockFX = false;
    }
    #endregion

}
