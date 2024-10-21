using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Messager : MonoBehaviour 
{
    private List<Enemy> enemies = new List<Enemy>();
    private void Update() {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            if(enemy.GetComponent<Enemy>() != null 
            && enemy.GetComponent<Enemy>().tags.IndexOf("youji") != -1
            && enemies.IndexOf(enemy.GetComponent<Enemy>()) == -1
            && enemy.GetComponent<Enemy>().state != enemy.GetComponent<Enemy>().Die_anim) {
                enemies.Add(enemy.GetComponent<Enemy>());
                enemy.GetComponent<Enemy>().def += 100;
                enemy.GetComponent<Enemy>().damage *= 1.1f;
            }
        }
        foreach(Enemy enemy in enemies){
            if(enemy.state == enemy.Die_anim){
                enemies.Remove(enemy);
            }
        }
    }
    private void OnDestroy() {
        foreach (Enemy enemy in enemies) {
            try{
                enemy.GetComponent<Enemy>().def -= 100;
                enemy.GetComponent<Enemy>().damage /= 1.1f;
            }catch{}
        }
    }
}