using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GalaxyButtonGUI : MonoBehaviour {
    

    [Header("Levels of the Galaxy")]
    public List<Level_SO> levels;
    public Queue<Level_SO> temp = new Queue<Level_SO>();
    private Button nextButton;

    private void Start() {
        nextButton = GetComponentInParent<GUIManager>().nextButton;
    }


    public void OnSelectGalaxy() {
        temp.Clear();
        for (int i = levels.Count-1; i >=0; i--) {
            
            temp.Enqueue(levels[i]);     //enqueue the levels from last to first 
                                         //so the First one is the one the player begins with
        }
        GameManager.Instance.levelsQueue = temp;
        Debug.Log(GameManager.Instance.levelsQueue.Count + " levels in "+ gameObject.name);
        nextButton.interactable = true;
       
    }
    


    public void FetchPlanets() {
        PlanetGUI[] temp = GetComponentsInChildren<PlanetGUI>();

        for (int i = 0; i < levels.Count; i++) {
            Debug.Log(temp[i].name + " " + i);
            temp[i].planetLevel = levels[i];
        }
    }
}
