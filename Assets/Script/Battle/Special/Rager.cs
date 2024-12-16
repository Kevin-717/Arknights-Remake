using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Rager : MonoBehaviour 
{
    public Enemy enemyController;
    public float damage;
    private void Start() {
        enemyController = GetComponent<Enemy>();
    }
    private void Update() {
        enemyController.TakeDamage(damage*Time.deltaTime,BattleController.damageType.Real);
    }
}