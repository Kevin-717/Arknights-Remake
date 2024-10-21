using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Defdrn : MonoBehaviour 
{
    [Range(0,20)]public float range = 5;
    public float value = 100;
    private List<GameObject> enemyList = new List<GameObject>();
    private void Update() {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy")){
            if(g.GetComponent<Enemy>() == null) continue;
            if(Vector3.Distance(g.transform.position,transform.position) <= range && enemyList.IndexOf(g) == -1){
                enemyList.Add(g);
                g.GetComponent<Enemy>().def += value;
            }
            if(Vector3.Distance(g.transform.position,transform.position) > range && enemyList.IndexOf(g) != -1){
                enemyList.Remove(g);
                g.GetComponent<Enemy>().def -= value;
            }
        }
    }
    private void OnDestroy() {
        foreach(GameObject g in enemyList){
            g.GetComponent<Enemy>().def -= value;
        }
    }
}