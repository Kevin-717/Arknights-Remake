using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SmephiMove : MonoBehaviour 
{
    private Enemy enemyController;
    public List<float> waitTime = new List<float>();
    public float hideTime;
    private float timer;
    private int index = 0;
    private void Start() {
        enemyController = GetComponent<Enemy>();
        timer = waitTime[index];
    }
    private void Update() {
        timer -= Time.deltaTime;
        if (timer < 0) {
            if(index == waitTime.Count-1) {
                BattleController.Instance.life--;
                Destroy(gameObject);
                return;
            }
            index++;
            GetComponent<MeshRenderer>().enabled = false;
            transform.position = enemyController.move_line[enemyController.move_index].path;
            Invoke("Show",hideTime);
            timer = waitTime[index];
        }
    }
    private void Show(){
        GetComponent<MeshRenderer>().enabled = true;
    }
}