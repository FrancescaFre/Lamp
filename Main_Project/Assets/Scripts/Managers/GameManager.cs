using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;
    public Level_SO levelLoaded;
    public LampBehaviour LastAllyLamp = null; //last lamp turned on
    public Vector3 startingPosition;      //the starting position if no lamp has been turned on yet
    #region  GameObjects

    [Header("Prefabs of the Enemies")]
    public List<GameObject> enemies = new List<GameObject>(3);

    [Header("Prefabs of the Characters")]
    public List<PlayerController> CharactersList;   //the gameobject that are present in the scene
    public int currentCharacter = 0;
    public int nextChar = 1;    //the 0 is the starting player
    public List<CharPeriod> TeamList;
    public static Dictionary<CharPeriod, PlayerController> Characters;
    #endregion

  
    public Dictionary<string, int> items;

    private void Awake() {
        if (!Instance) {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;  // add a delegate to be run everytime a scene is loaded
    }



    // Use this for initialization
    void Start () {
        items = new Dictionary<string, int>(6);
        Characters = new Dictionary<CharPeriod, PlayerController>();

       /* for (int i = 0; i < CharactersList.Count; i++) {// fill the dictionary with the characters
            
            Characters[CharactersList[i].CharacterPeriod] = CharactersList[i];
        }*/

        
        //ActivatePlayerX(); //called when the scene has been loaded

    }
	


    /// <summary>
    /// Sets as Active only one character at a time
    /// </summary>
    public void ActivatePlayerX() {

        for (int i = 0; i < TeamList.Count; i++)
            Characters[TeamList[i]].gameObject.SetActive(i == currentCharacter );
       
        

        nextChar = currentCharacter  < TeamList.Count ? currentCharacter + 1:-1;
        if (nextChar == -1)
            Debug.Log("gameover");
        

    }

    public void SpawnNewPlayer() {
        currentCharacter = nextChar;
        if (LastAllyLamp)
            // CharactersList[currentCharacter].transform.position = LastAllyLamp.transform.position/* + LastAllyLamp.transform.forward */+ CharactersList[currentCharacter].transform.forward;
            Characters[TeamList[currentCharacter]].transform.position = LastAllyLamp.transform.position/* + LastAllyLamp.transform.forward */+ Characters[TeamList [currentCharacter]].transform.forward;
        else
            Characters[TeamList[currentCharacter]].transform.position = startingPosition;
        ActivatePlayerX();


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


    #region Scene Management
    /// <summary>
    /// Start the selected level
    /// </summary>
    public void StartGame() {
        //TODO fade nto scene
        Debug.Log("GAME STARTED WITH SCENE: " + levelLoaded.levelSeason+" first player is "+TeamList[0]);
        SceneManager.LoadScene(levelLoaded.name);
    }

    public void EndGame() {


    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }

    #endregion
}
