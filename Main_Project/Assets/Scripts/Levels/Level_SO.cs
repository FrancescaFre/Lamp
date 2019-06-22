using System.Collections.Generic;
using UnityEngine;

public enum Season { SPRING = 0, SUMMER, FALL, WINTER }
public enum Quests {ENEMY_GT = 0, ENEMY_ET, ENEMY_LT,
                    SKILL_GT, SKILL_ET, SKILL_LT,
                    ITEM_GT, ITEM_ET, ITEM_LT,
                    CURSE_GT, CURSE_ET, CURSE_LT
                    }

[CreateAssetMenu]
public class Level_SO : ScriptableObject {

    [Header("Level Details")]
    public string LevelName;

    public Season levelSeason;
    public CharPeriod epoch;
    public string Description;
    public bool isCompleted = false;

    [Header("Music clips")]
    public List<AudioClip> levelMusic;

    [Header("SFX clips")]
    public AudioClip lampSwitchSFX;

    public AudioClip keySFX;
    public AudioClip drillSFX;
    public AudioClip missingSFX;
    public List<AudioClip> footStepsSFX;

    [Header("Lamp properties")]
    public int allyLamps;

    public Color allyColor = Color.yellow;
    public int enemyLamps;
    public Color enemyColor = Color.magenta;
    public Vector3 entryPoint = new Vector3(0f, 21f, 0);

    [Header("Number of enemy of each type")]
    public int enemy_L1;
    public int enemy_L2;
    public int enemy_L3;
    public List<GameObject> PathList;

    [Header("GameObject of each enemy type")]
    public GameObject enemy_L1_GO;
    public GameObject enemy_L2_GO;
    public GameObject enemy_L3_GO;

    [Header("Challenges of the level")]
    public string[] subQuests;
    public bool[] questCompletion;
    public Quests[] quest_code;
    public int[] quest_value;
    

    public void Reset() {
        isCompleted = false;
      
        questCompletion =new bool[]{ false,false,false };
    }

    public void SetFree() {
        isCompleted = true;
    }
}
