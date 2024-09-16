using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CreateEnemyTemp : MonoBehaviour {
    public EnemyGroupData enemy;
    private int count;
    private float createTime;
    private void Start() {
        createTime = enemy.interval;
    }
    private void Update() {
        if(enemy.repeat == 1){
            GameObject e = Instantiate(enemy.enemyPrefab);
            e.transform.position = enemy.startPos;
            e.GetComponent<Enemy>().move_line = enemy.enemyPath;
            Destroy(gameObject);
            return;
        }
        if(count >= enemy.repeat){
            Destroy(gameObject);
        }
        if(createTime >= enemy.interval){
            GameObject e = Instantiate(enemy.enemyPrefab);
            e.transform.position = enemy.startPos;
            e.GetComponent<Enemy>().move_line = enemy.enemyPath;
            createTime = 0;
            count++;
        }
        createTime += Time.deltaTime;
    }
}