using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;
public class MeiFei : MonoBehaviour 
{
    [Range(1,100)]public float attackRange = 3;
    public string Attack_anim = "";
    public float interval = 1;
    private float countdown;
    public float damage = 50;
    private SkeletonAnimation skeletonAnimation;
    private Enemy enemyController;
    private string es;
    private List<GameObject> attackObjects = new List<GameObject>();
    public BattleController.damageType dt = BattleController.damageType.Revitalize;
    private void Start() {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        enemyController = GetComponent<Enemy>();
        countdown = interval;
    }
    private void Update(){
        if(enemyController.inhole) return;
        if(countdown <= 0 && enemyController.state != enemyController.Die_anim && GameObject.FindGameObjectsWithTag("Enemy").Length > 0){
            countdown = interval;
            int c = 0;
            foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
                if(enemy.GetComponent<Enemy>() == null) continue;
                if(enemy.GetComponent<Enemy>().hp < enemy.GetComponent<Enemy>().totalHp){
                    attackObjects.Add(enemy);
                    c++;
                }
            }  
            if(c < 3){
                if( GameObject.FindGameObjectsWithTag("Enemy").Length < 3){
                    attackObjects.Clear();
                    foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
                        if(enemy.GetComponent<Enemy>() == null) continue;
                        attackObjects.Add(enemy);
                    } 
                };
                foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
                    if(enemy.GetComponent<Enemy>() == null) continue;
                    if(attackObjects.IndexOf(enemy) == -1){
                        attackObjects.Add(enemy);
                    }
                }  
            }
            es = enemyController.state;
            enemyController.useSpecial = true;
            enemyController.state = enemyController.Attack_anim;
        }else{
            countdown -= Time.deltaTime;
        }
    }
    public void Damage(){
        foreach(GameObject enemy in attackObjects){
            if(enemy.IsDestroyed()) return;
            Debug.LogWarning(enemy.gameObject.name);
            enemy.GetComponent<Enemy>().TakeDamage(damage,BattleController.damageType.Revitalize);
        }
        attackObjects.Clear();
        skeletonAnimation.AnimationState.Complete += delegate {
            enemyController.state = es;
            enemyController.useSpecial = false;
        };
    }
}