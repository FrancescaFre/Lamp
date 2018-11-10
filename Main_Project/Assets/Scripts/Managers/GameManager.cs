using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;

    #region  GameObjects

    [Header("Prefabs of the Enemies")]
    public List<GameObject> enemies = new List<GameObject>(3);

    [Header("Prefabs of the Characters")]
    public List<GameObject> Characters = new List<GameObject>(3);
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
