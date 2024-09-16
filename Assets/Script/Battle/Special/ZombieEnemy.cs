using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ZombieEnemy : MonoBehaviour 
{
    public float hpSpeed = 80;
    private float hpVal = 0.8f;
    private Enemy enemyController;
    private float lt;
    private void Start() {
        enemyController = GetComponent<Enemy>();
        lt = Time.time;
    }
    private void Update() {
        if(enemyController.inhole) return;
        if(Time.time - lt > 1){
            lt = Time.time;
            enemyController.TakeDamage(hpSpeed*hpVal,BattleController.damageType.Revitalize);
        }
    }
}