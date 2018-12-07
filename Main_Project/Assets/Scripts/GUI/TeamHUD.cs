using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamHUD : MonoBehaviour {

    public static TeamHUD Instance;
    public GameObject memberPrefab;
    public int teamCount;
    public Queue<Image> teamQueue=new Queue<Image>();
    public Color startColour=Color.white;
    public Color halfCurseColour=Color.magenta;
    public Color deadColour=Color.grey;

    private void Awake() {
        if (!Instance)
            Instance = this;
    }
    private void Start() {
        teamCount = GameManager.Instance.TeamList.Count;

        for (int i = 0; i < teamCount; i++) {
            CreateIconQueue();
        }
    }

    private void CreateIconQueue() {
        var iconGO = Instantiate<GameObject>(memberPrefab, this.transform);
        iconGO.transform.SetParent(this.transform);
        iconGO.transform.localScale = Vector3.one;
        iconGO.transform.localRotation = Quaternion.identity;
        Image icon = iconGO.GetComponent<Image>();
        icon.color = startColour;
        teamQueue.Enqueue(icon);
    }

    public void HalfCurse() {
        teamQueue.Peek().color = halfCurseColour;
    }

    public void Curse() {
        teamQueue.Peek().color = deadColour;
        teamQueue.Dequeue();
    }


}
