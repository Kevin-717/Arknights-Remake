using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class OverloadController : MonoBehaviour 
{
    private Enemy enemyController;
    public Transform energyProgress;
    public Color energyColor;
    public Color overloadColor;
    public int energy;
    public int energyTotal = 20;
    private bool is_overload = false;
    public float overloadTime = 10;
    private float overloadTimer;
    private float damageInit;
    private float defInit;
    private void Start() {
        enemyController = GetComponent<Enemy>();
        damageInit = enemyController.damage;
        defInit = enemyController.def;
    }
    private void Update() {
        enemyController.damage = damageInit * (0.1f*energy+1f);
        enemyController.def = defInit * (0.2f*energy+1f);
        if(is_overload){
            energyProgress.localScale = new Vector3(overloadTimer/overloadTime,1,1);
            energyProgress.gameObject.GetComponent<Image>().color = overloadColor;
            overloadTimer -= Time.deltaTime;
            if(overloadTimer < 0){
                enemyController.atkSpeedScale = 1;
                enemyController.adef -= 30;
                energy = 0;
                is_overload = false;
            }
        }else{
            energyProgress.localScale = new Vector3((float)energy/energyTotal,1,1);
            energyProgress.gameObject.GetComponent<Image>().color = energyColor;
            if(energy >= energyTotal){
                enemyController.atkSpeedScale = 2.2f;
                enemyController.adef += 30;
                overloadTimer = overloadTime;
                is_overload = true;
            }
        }
    }
}