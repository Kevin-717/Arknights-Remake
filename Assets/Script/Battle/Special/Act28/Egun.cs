using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Spine.Unity;
public class Egun : MonoBehaviour 
{
    private Enemy enemyController;
    private SkeletonAnimation spineController;
    private float stt = 5;
    private float sp = 5;
    private float sp_max = 28;
    private float timer = 1;
    private float stimer = 1;
    private float sttimer = 5;
    private string es;
    private bool stigger = false;
    private void Start() {
        enemyController = GetComponent<Enemy>();
        spineController = GetComponent<SkeletonAnimation>();
    }
    private void Update() {
        if(sp < sp_max){
            timer -= Time.deltaTime;
            if(timer < 0){
                timer = 1;
                sp++;
            }
        }else{
            sp = 0;
            es = enemyController.state;
            Debug.Log(enemyController.state);
            enemyController.useSpecial = true;
            enemyController.state = "Skill";
            spineController.state.Complete += delegate {
                Debug.Log(es);
                if(enemyController.state != "Skill") return;
                enemyController.state = es;
            };
            stigger = true;
        }
        if(stigger){
            stimer -= Time.deltaTime;
            sttimer -= Time.deltaTime;
            if(sttimer < 0){
                sttimer = 5;
                stigger = false;
            }
            if(stimer < 0){
                stimer = 1;
                foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
                    if(enemy.gameObject.name == "Collider" && enemy.transform.parent.gameObject.GetComponent<OverloadController>() != null){
                        if(Vector3.Distance(enemy.transform.position,transform.position) <= 2.2f){
                            enemy.transform.parent.gameObject.GetComponent<OverloadController>().energy++;
                        }
                        
                    }
                }
            }
        }
    }
}