using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Level_SO : ScriptableObject {

    public string levelName;
    public int allyLamps;
    public int enemyLamps;
    [Header("Number of enemy of each type")]
    public int enemy_L1;
    public int enemy_L2;
    public int enemy_L3;

}
