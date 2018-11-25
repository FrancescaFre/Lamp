using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class GalaxyButtonGUI : MonoBehaviour,ICancelHandler,IPointerClickHandler,ISubmitHandler {
    

    [Header("Levels of the Galaxy")]
    public List<Level_SO> levels;
    public Queue<Level_SO> temp = new Queue<Level_SO>();
    



    public void OnSubmit(BaseEventData eventData) {
        SelectGalaxy();
    }

    public void OnPointerClick(PointerEventData eventData) {
        SelectGalaxy();
    }
    public void SelectGalaxy() {
        temp.Clear();
        for (int i = 0; i < levels.Count; i++) {     
            temp.Enqueue(levels[i]);      // the First one is the one the player begins with
        }


        GameManager.Instance.levelsQueue = temp;
        Debug.Log(GameManager.Instance.levelsQueue.Count + " levels in "+ gameObject.name);
        GUIManager.GUIInstance.nextButton.interactable = true;
        
    }
    


    public void FetchPlanets() {
        PlanetGUI[] temp = GetComponentsInChildren<PlanetGUI>();

        for (int i = 0; i < levels.Count; i++) {
            Debug.Log(temp[i].name + " " + i);
            temp[i].planetLevel = levels[i];
        }
    }

    public void OnCancel(BaseEventData eventData) {
        GameManager.Instance.levelsQueue = null;
        GUIManager.GUIInstance.nextButton.interactable = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

}
