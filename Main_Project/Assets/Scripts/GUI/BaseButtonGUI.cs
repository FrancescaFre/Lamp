using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BaseButtonGUI : MonoBehaviour, ICancelHandler, IPointerClickHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {

    [Header("Halo Effect")]
    public ParticleSystem haloParticle;
    public ParticleRendererGUI haloRenderer;
    public Material highlight;
    public Material selected;
    public Button thisButton;

    public virtual void Awake() {
        if (!haloParticle) {
            haloRenderer = GetComponentInChildren<ParticleRendererGUI>(includeInactive: true);
            haloParticle = haloRenderer.GetComponent<ParticleSystem>();
        }
        StopHalo();
        thisButton = GetComponent<Button>();
    }

    public void StartHalo() {
        haloParticle.Play();
    }

    public void StopHalo() {
        haloParticle.Stop();
    }

    public void SetSelected() {
        haloRenderer.material = selected;
    }

    public void SetHighlight() {
        haloRenderer.material = highlight;
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
        SetSelected();
    }

    public virtual void OnSubmit(BaseEventData eventData) {
        SetSelected();
    }

    public virtual void OnCancel(BaseEventData eventData) {
        SetHighlight();
    }

    #endregion Event Handlers
}