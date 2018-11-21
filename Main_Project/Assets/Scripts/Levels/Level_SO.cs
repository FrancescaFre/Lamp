using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Season{SPRING=0, SUMMER, FALL, WINTER }
[CreateAssetMenu]
public class Level_SO : ScriptableObject {

    public string LevelName;
    public Season levelSeason;
    public string Description;
    public int allyLamps;
    public int enemyLamps;
    [Header("Number of enemy of each type")]
    public int enemy_L1;
    public int enemy_L2;
    public int enemy_L3;
    [Header("GameObject of each enemy type")]
    public GameObject enemy_L1_GO;
    public GameObject enemy_L2_GO;
    public GameObject enemy_L3_GO;
    [Header("Challenges of the level")]
    public bool challenge_1;
    public string subQuest_1;
    public bool challenge_2;
    public string subQuest_2;
    public bool challenge_3;
    public string subQuest_3;




}
