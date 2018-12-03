using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LampGUI : MonoBehaviour {

    public GameObject IconPrefab;
    [Header("Good lamp Icons")]
    public int allyLamp;
    public Queue<Image> allyQueue = new Queue<Image>();
    [Header("Bad lamp Icons")]
    public int enemyLamp;
    public Queue<Image> enemyQueue = new Queue<Image>();

    private void Start() {
        if (allyLamp == 0)// to testi in edit mode
            allyLamp = GameManager.Instance.levelLoaded.allyLamps;
        if (enemyLamp == 0)
            enemyLamp = GameManager.Instance.levelLoaded.enemyLamps;
        for (int i = 0; i < allyLamp; i++) {
           CreateIconQueue(allyQueue, Color.white);
          
        }

        for (int i = 0; i < enemyLamp; i++) {
             CreateIconQueue(enemyQueue, Color.magenta);
          
        }
        
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
    private void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.M))
            DequeueAlly();
        if (Input.GetKeyDown(KeyCode.N))
            DequeueEnemy();
        
    }
    public  void DequeueAlly() {
        OnDequeue(allyQueue, Color.yellow);
    }
    public void DequeueEnemy() {
        OnDequeue(enemyQueue, Color.gray);
        //Destroy(icon.gameObject);
    }

    private void OnDequeue(Queue<Image> queue, Color col) {
        if (queue.Count == 0) return ;

        Image icon = queue.Dequeue();
        icon.color = col;
        
    }
}
