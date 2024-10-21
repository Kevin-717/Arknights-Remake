using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;
public class Faust : MonoBehaviour 
{
    [Range(1,30)]public float attackRange = 3;
    public string Attack_anim = "";
    public string atkAnim = "";
    public string skill1 = "";
    public string skill2 = "";
    public float interval = 1;
    private float countdown;
    public float damage = 50;
    private string es = "";
    private SkeletonAnimation skeletonAnimation;
    private Enemy enemyController;
    private GameObject attackObject;
    public BattleController.damageType dt;
    private int skillType = 0;
    private int count = 0;
    private float skill2Interval = 15;
    private float skill2LastTime;
    private void Start() {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        enemyController = GetComponent<Enemy>();
        countdown = interval;
        atkAnim = Attack_anim;
        skill2LastTime = Time.time;
    }
    private void Update(){
        if(enemyController.inhole) return;
        if(countdown <= 0 && attackObject == null && enemyController.state != enemyController.Die_anim && enemyController.state != skill2){
            foreach (GameObject character in GameObject.FindGameObjectsWithTag("char")){
                if(Vector3.Distance(character.transform.position,transform.position)<=attackRange
                && character.GetComponent<Char>().state != character.GetComponent<Char>().Die_anim
                && character.GetComponent<Char>().state != "Default"
                && character.GetComponent<Char>().state != character.GetComponent<Char>().Start_anim){
                    attackObject = character;
                    countdown = interval;
                    es = enemyController.state;
                    if(count >= 3){
                        skillType = 1;
                        atkAnim = skill1;
                        enemyController.state = atkAnim;
                        return;
                    }
                    enemyController.state = atkAnim;
                    enemyController.useSpecial = true;
                }
            }    
            if(attackObject != null){
                if(attackObject.GetComponent<Char>().hp <= 0){
                    enemyController.state = es;
                    enemyController.useSpecial = false;
                    attackObject = null;
                }
            }
        }else{
            countdown -= Time.deltaTime;
        }
        if(Time.time - skill2LastTime >= skill2Interval){
            if(enemyController.state == Attack_anim || enemyController.state == skill1){
                return;
            }
            skill2LastTime = Time.time;
            es = enemyController.state;
            enemyController.state = skill2;
            skillType = 2;
            enemyController.useSpecial = true;
        }
    }
    public void Damage(){
        switch(skillType){
            case 0:
                if(attackObject != null  && attackObject.GetComponent<Char>().hp > 0){
                    attackObject.GetComponent<Char>().TakeDamage(damage,dt);
                    count++;
                }else{
                    enemyController.state = es;
                    enemyController.useSpecial = false;
                    attackObject = null;
                    return;
                }
                enemyController.state = es;
                enemyController.useSpecial = false;
                attackObject = null;
                atkAnim = Attack_anim;
                break;
            case 1:
                if(attackObject != null  && attackObject.GetComponent<Char>().hp > 0){
                    attackObject.GetComponent<Char>().TakeDamage(damage*2,dt);
                    count = 0;
                }else{
                    enemyController.state = es;
                    enemyController.useSpecial = false;
                    attackObject = null;
                    return;
                }
                enemyController.state = es;
                enemyController.useSpecial = false;
                attackObject = null;
                atkAnim = Attack_anim;
                skillType = 0;
                break;
            case 2:
                BattleController.Instance.ActiveMachine();
                enemyController.state = es;
                enemyController.useSpecial = false;
                atkAnim = Attack_anim;
                skillType = 0;
                break;
        }
    }
}