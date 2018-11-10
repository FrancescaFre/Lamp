using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;
    public Level_SO levelLoaded;
    public LampBehaviour LastAllyLamp = null; //last lamp turned on


    #region  GameObjects

    [Header("Prefabs of the Enemies")]
    public List<GameObject> enemies = new List<GameObject>(3);

    [Header("Prefabs of the Characters")]
    public List<GameObject> Characters;
    public int nextChar = 1;    //the 0 is the starting player
    #endregion

  
    public Dictionary<string, int> items;

    private void Awake() {
        if (!Instance) {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }



    // Use this for initialization
    void Start () {
        items = new Dictionary<string, int>(6);
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnNewPlayer(PlayerController character) {
        while(!Characters[nextChar] && nextChar < Characters.Count) {
            nextChar++;
        }
        GameObject newCharacter = Characters[nextChar];
        Debug.Log(newCharacter.name);



        //newCharacter.transform.position = LastAllyLamp.transform.position + LastAllyLamp.transform.forward + newCharacter.transform.forward;

        Destroy(character.gameObject);
        Instantiate<GameObject>(newCharacter);

    }

    /// <summary>
    /// Use an item from the inventory if any
    /// </summary>
    /// <param name="itemKey">Item to be used</param>
    public void UseItem(string itemKey) {
        if (items[itemKey] > 0) {
            ManageItem(itemKey, -1);
            Debug.Log("used item " + itemKey);  //TODO: add use of the item
        }
        else {
            Debug.Log("not enough item " + itemKey);
        }
    }
    /// <summary>
    /// Manage an item in the inventory
    /// </summary>
    /// <param name="itemKey">Managed item</param>
    /// <param name="use">Plus 1 if gathered; Minus 1 if used</param>
    public void ManageItem(string itemKey, int use) {
        items[itemKey] += use;
    }

}
