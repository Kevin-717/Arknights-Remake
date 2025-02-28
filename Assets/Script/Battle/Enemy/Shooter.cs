using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;

public class Shooter : MonoBehaviour 
{
    public Enemy enemyController;
    private float AttackInterval;
    private Char attackTarget;
    private EnemyState lastState;
    [Header("攻击范围半径")]
    [Header("!!! 在Enemy中设置Shoot -> 射击动画")]
    [Range(0.1f,20.0f)]public float attackRange = 2.0f;
    private void Start(){
        enemyController = GetComponent<Enemy>();
        enemyController.skeletonAnimation.state.Complete += delegate{
            if(enemyController.state == EnemyState.Shoot){
                enemyController.state = lastState;
            }
        };
    }

    private void Update(){
        AttackInterval -= Time.deltaTime;
        if(enemyController.attackTarget == null && AttackInterval < 0){
            AttackInterval = enemyController.damageInterval;
            foreach(GameObject c in GameObject.FindGameObjectsWithTag("char")){
                if(enemyController.ObjectIsAvailable(c.GetComponent<Char>()) && Vector3.Distance(c.transform.position,transform.position) <= attackRange){
                    lastState = CopyState();
                    enemyController.state = EnemyState.Shoot;
                    attackTarget = c.GetComponent<Char>();
                    break;
                }
            }
        }
    }
    public void ShootChar(){
        if(enemyController.state != EnemyState.Shoot) return;
        attackTarget.TakeDamage(enemyController.damage);
    }
    private EnemyState CopyState()
    {
        // 创建一个新的GameObject
        GameObject newGameObject = new GameObject("CopiedObject");

        // 添加MyMonoClass组件
        Enemy copy = newGameObject.AddComponent<Enemy>();

        // 获取所有公共字段
        FieldInfo[] fields = enemyController.GetType().GetFields(BindingFlags.Public);

        // 复制字段值
        foreach (FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(this));
        }
        
        Destroy(newGameObject);
        return copy.state;
    }
}