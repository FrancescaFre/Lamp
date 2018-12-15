
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LampHUD : MonoBehaviour {

    public GameObject IconPrefab;

    public Queue<Image> allyQueue = new Queue<Image>();

    public Queue<Image> enemyQueue = new Queue<Image>();

    private void Awake() {
        GetComponentInParent<PauseManagerGUI>().lampHUDPanel = this;
        GetComponentInParent<InGameHUD>().lampHUDPanel = this;
    }

    private void Start() {
        if (!GameManager.Instance.lampHUD) 
            GameManager.Instance.lampHUD = this;

        for (int i = 0; i < GameManager.Instance.levelLoaded.allyLamps; i++) {
            CreateIconQueue(allyQueue, Color.white);

        }

        for (int i = 0; i < GameManager.Instance.levelLoaded.enemyLamps; i++) {
            CreateIconQueue(enemyQueue, Color.magenta);

        }
        gameObject.SetActive(false);
        
    }

    private void CreateIconQueue(Queue<Image> queue, Color col) {
        var iconGO = Instantiate<GameObject>(IconPrefab,this.transform);
        iconGO.transform.SetParent(this.transform);
        iconGO.transform.localScale = Vector3.one;
        iconGO.transform.localRotation = Quaternion.identity;
        Image icon= iconGO.GetComponent<Image>();
        icon.color = col;
        queue.Enqueue(icon);
        

    }

    public  void DequeueAlly() {
        OnDequeue(allyQueue, Color.yellow);
    }
    public void DequeueEnemy() {
        OnDequeue(enemyQueue, Color.gray);
        
    }

    private void OnDequeue(Queue<Image> queue, Color col) {
        if (queue.Count == 0) return ;

        Image icon = queue.Dequeue();
        icon.color = col;
        
    }
}
