﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;
    public Level_SO levelLoaded;
    public LampBehaviour LastAllyLamp = null; //last lamp turned on
    [Header("HUD of the lamps")]
    public LampHUD lampHUD;
    public int allyLamps;
    public int enemyLamps;

    #region  GameObjects

    [Header("Enemies")]
    public List<GameObject> enemiyGOList;
    public int howManySeeing = 0;
    public int howManyHearing = 0;


    [Header(" Characters")]
    public List<PlayerController> CharactersList = new List<PlayerController>();   //the gameobject that are present in the scene
    public PlayerController currentPC;
    public int currentCharacter = 0;
    public int nextChar = 1;    //the 0 is the starting player
    public List<CharPeriod> TeamList;
    public static Dictionary<CharPeriod, PlayerController> CharactersDict;
    #endregion


    public Dictionary<string, int> items;

    private void Awake() {
        if (!Instance) {
            Instance = this;
            DontDestroyOnLoad(Instance);
            DontDestroyOnLoad(EventSystem.current);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;  // add a delegate to be run everytime a scene is loaded
    }



    // Use this for initialization
    void Start() {
        items = new Dictionary<string, int>(6);






    }



    /// <summary>
    /// Sets as Active only one character at a time
    /// </summary>
    public void ActivatePlayerX() {

        for (int i = 0; i < TeamList.Count; i++) {
            CharactersDict[TeamList[i]].gameObject.SetActive(i == currentCharacter);

        }

        currentPC = CharactersDict[TeamList[currentCharacter]];

        nextChar = currentCharacter < TeamList.Count ? currentCharacter + 1 : -1;
        if (nextChar == -1) {
            Debug.Log("gameover");

        }


    }

    public void SpawnNewPlayer() {
        currentCharacter = nextChar;
        if (LastAllyLamp)

            CharactersDict[TeamList[currentCharacter]].transform.position = LastAllyLamp.transform.position/* + LastAllyLamp.transform.forward */+ CharactersDict[TeamList[currentCharacter]].transform.forward;
        else
            CharactersDict[TeamList[currentCharacter]].transform.position = levelLoaded.entryPoint;
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
    public void LoadGame() {
        //TODO fade into scene
        Debug.Log("GAME STARTED WITH SCENE: " + levelLoaded.levelSeason + " first player is " + TeamList[0]);
        SceneManager.LoadScene(levelLoaded.name);



    }

    public void StartGame() {//Prologue

        CharactersList = new List<PlayerController>(FindObjectsOfType<PlayerController>());
        CharactersDict = new Dictionary<CharPeriod, PlayerController>();

        this.allyLamps = levelLoaded.allyLamps;
        this.enemyLamps = levelLoaded.enemyLamps;
        for (int i = 0; i < CharactersList.Count; i++) {
            Debug.Log(CharactersList[i].CharacterPeriod.ToString());
            CharactersDict[CharactersList[i].CharacterPeriod] = CharactersList[i];
        }
        enemiyGOList = new List<GameObject>() {
            levelLoaded.enemy_L1_GO,
            levelLoaded.enemy_L2_GO,
            levelLoaded.enemy_L3_GO
        };
        /*
        for (int i = 0; i < levelLoaded.enemy_L1; i++) {
            Enemy x = levelLoaded.enemy_L1_GO.GetComponent<Enemy>();
            x.path = levelLoaded.PathList[i].transform;
            Instantiate(levelLoaded.enemy_L1_GO);
        }*/
        //lampGUI = FindObjectOfType<LampGUI>();
        ActivatePlayerX();
    }
    public void EndGame() {//epilogue
        currentPC = null;
        lampHUD = null;
        levelLoaded = null;
        TeamList = null;
        currentCharacter = 0;
        nextChar = 1;
        enemiyGOList.Clear();
        CharactersDict.Clear();
        CharactersList.Clear();
        SceneManager.LoadScene("1_GameMenu");//which has index 1


    }

    public void SpawnNewEnemy(int enemyLevel, Vector3 playerPosition, Transform enemyPath) {
        Debug.Log("create the enemy ");
        GameObject enemyGO = enemiyGOList[enemyLevel]; //the levels are [1,3]
        enemyGO.GetComponent<Rigidbody>().position = playerPosition;

        enemyGO.GetComponent<Enemy>().path = enemyPath;
        TeamHUD.Instance.Curse();
        SpawnNewPlayer(); //destroys the character
        Instantiate<GameObject>(enemyGO);//creates the enemy instead
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex < 2) return;
        Debug.Log("OnSceneLoaded: " + scene.name);
        StartGame();
    }

    #endregion
}
