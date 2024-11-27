using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CurseDevice : MonoBehaviour 
{
    private List<GameObject> chars = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();
    private float total_sp = 7;
    private float sp = 0;
    public Transform progress;
    public ParticleSystem particleSystem;
    private void Update() {
        foreach(GameObject chara in GameObject.FindGameObjectsWithTag("char")){
            if(chara.GetComponent<Char>().state != "Default" &&
               chara.GetComponent<Char>().state != "Die" &&
               chars.IndexOf(chara) == -1 &&
               Vector3.Distance(chara.transform.position,transform.position) <= 1.8f){
                chars.Add(chara);
            }
        }
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
            if(enemy.GetComponent<Enemy>() != null &&
             enemy.GetComponent<Enemy>().state != "Die" &&
             enemies.IndexOf(enemy) == -1 &&
             Vector3.Distance(enemy.transform.position,transform.position) <= 1.8f){
                enemies.Add(enemy);
            }
        }
        sp += Time.deltaTime;
        if(sp >= total_sp){
            sp = 0;
            particleSystem.Play();
            foreach(GameObject cTarget in chars){
                cTarget.GetComponent<Char>().TakeDamage(500,BattleController.damageType.Real);
            }
            foreach(GameObject eTarget in enemies){
                eTarget.GetComponent<Enemy>().TakeDamage(500,BattleController.damageType.Real);
            }
        }
        progress.transform.localScale = new Vector3(sp/total_sp,1,1);
        List<GameObject> t = new List<GameObject>();
        foreach(GameObject c in chars){
            if(c == null || Vector3.Distance(c.transform.position,transform.position) > 1.8f){
                t.Add(c);
            }
        }
        foreach(GameObject d in t) {chars.Remove(d);}
        List<GameObject> t2 = new List<GameObject>();
        foreach(GameObject e in enemies){
            if(e == null || Vector3.Distance(e.transform.position,transform.position) > 1.8f){
                t2.Add(e);
            }
        }
        foreach(GameObject d2 in t2){enemies.Remove(d2);}
    }
}