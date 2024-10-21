using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;
public class FlyBoom : MonoBehaviour 
{
    [Range(1,10)]public float attackRange = 3;
    public string Attack_anim = "";
    public float interval = 1;
    private float countdown;
    public float damage = 50;
    private string es = "";
    private SkeletonAnimation skeletonAnimation;
    private Enemy enemyController;
    private GameObject attackObject;
    public BattleController.damageType dt;
    public string Move_anim = "";
    public string Idle_anim = "";
    public string Die_anim = "";
    private void Start() {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        enemyController = GetComponent<Enemy>();
        countdown = interval;
    }
    private void Update(){
        if(countdown <= 0 && attackObject == null && enemyController.state != enemyController.Die_anim){
            foreach (GameObject character in GameObject.FindGameObjectsWithTag("char")){
                if(Vector3.Distance(character.transform.position,transform.position)<=attackRange
                && character.GetComponent<Char>().state != character.GetComponent<Char>().Die_anim
                && character.GetComponent<Char>().state != "Default"
                && character.GetComponent<Char>().state != character.GetComponent<Char>().Start_anim){
                    countdown = interval;
                    es = enemyController.state;
                    enemyController.state = Attack_anim;
                    enemyController.useSpecial = true;
                    attackObject = character;
                }
            }     
        }else{
            countdown -= Time.deltaTime;
        }
        if(attackObject != null){
            if(attackObject.GetComponent<Char>().hp <= 0){
                enemyController.state = enemyController.Move_anim;
                enemyController.useSpecial = false;
                attackObject = null;
            }
        }
    }
    public void Damage(){
        if(attackObject != null && attackObject.GetComponent<Char>().hp > 0){
            attackObject.GetComponent<Char>().TakeDamage(damage,dt);
            enemyController.Move_anim = Move_anim;
            enemyController.Idle_anim = Idle_anim;
            enemyController.Die_anim = Die_anim;
            enemyController.speed *= 2;
            Destroy(this);
        }else{
            enemyController.state = enemyController.Move_anim;
            enemyController.useSpecial = false;
            attackObject = null;
            return;
        }
        enemyController.state = enemyController.Move_anim;
        enemyController.useSpecial = false;
        attackObject = null;
    }
}