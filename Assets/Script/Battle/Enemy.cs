using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using DG.Tweening;
using Unity.VisualScripting;
public class Enemy : MonoBehaviour 
{
    public List<EnemyPath> move_line;
    [Header("敌人动画")]
    public List<EnemyAnimation> enemyAnimations;
    [HideInInspector]
    public EnemyState state;
    [Header("血量")]
    public float hp;
    private float hp_total;
    [Header("物抗")]
    public float def;
    [Header("法抗")]
    public float mdef;
    [Header("攻击")]
    public Damage damage;
    [Header("攻击间隔")]
    public float damageInterval;
    [Header("移动速度")]
    public float moveSpeed;
    [Range(0.0f,1.0f)]public float move_scale = 0.5f;
    [Header("敌人类型")]
    public bool is_fly = false;
    [Header("检查点阈值")]
    public float min_checkpoint_disance = 0.3f;
    [HideInInspector]
    public SkeletonAnimation skeletonAnimation;
    [HideInInspector]
    public EnemyMove enemyMove;
    [HideInInspector]
    public int move_index = 0;
    [HideInInspector]
    public List<Char> beAttackedTargets;
    [HideInInspector]
    public Char attackTarget;
    private float waitTime;
    private float attackWaitTime;
    private EnemyState lastState;
    [Header("血条")]
    public Transform hp_bar;
    public Transform hp_white;
    public GameObject hp_frame;
    [Header("是否攻击")]
    public bool is_attack = true;
    private Vector3 fly_point;
    private void Start() {
        hp_total = hp;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        enemyMove = GetComponent<EnemyMove>();
        enemyMove.UpdatePath(move_line[move_index].path);
        skeletonAnimation.state.Start += delegate{
            if(state == EnemyState.Attack){
                attackWaitTime = damageInterval;
            }
        };
        skeletonAnimation.state.Complete += delegate{
            switch(state){
                case EnemyState.Attack:
                    state = EnemyState.AttackInterval;
                    break;
                case EnemyState.Die:
                    Destroy(gameObject);
                    break;
            }
        };
        hp_frame.SetActive(false);
        if (is_fly)
        {
            enemyMove.enabled = false;
            enemyMove.seeker.enabled = false;
            fly_point = move_line[move_index].path;
        }
    }
    private void Update() {
        UpdateAnimation();
        EnemyBehaviorController();
    }
    private void UpdateAnimation(){
        EnemyAnimation anim = GetAnimationName(state);
        if(skeletonAnimation.AnimationName != anim.anim){
            skeletonAnimation.state.SetAnimation(0,anim.anim,anim.isLoop);
        }
    }
    public EnemyAnimation GetAnimationName(EnemyState estate){
        foreach(EnemyAnimation ea in enemyAnimations){
            if(ea.state == estate){
                return ea;
            }
        }
        Debug.LogWarning("Enemy Animation not found");
        return new EnemyAnimation(){
            state = estate,
            anim = "Default",
        };
    }
    public void EnemyMoveController(){
        if(Vector3.Distance(transform.position,move_line[move_index].path) < min_checkpoint_disance){
            move_index++;
            if(move_index >= move_line.Count){
                //扣血
                Destroy(gameObject);
                return;
            }
            switch(move_line[move_index].pointType){
                case PointType.normal:
                    if (!is_fly)
                    {
                        enemyMove.UpdatePath(move_line[move_index].path);
                    }
                    else
                    {
                        fly_point = move_line[move_index].path;
                    }
                        break;
                case PointType.wait:
                    state = EnemyState.Idle;
                    waitTime = move_line[move_index].waitTime;
                    break;
                case PointType.disappear:
                    GetComponent<MeshRenderer>().enabled = false;
                    move_index++;
                    break;
                case PointType.appear:
                    transform.position = move_line[move_index].path;
                    GetComponent<MeshRenderer>().enabled = true;
                    break;
            }
        }
        enemyMove.speed = moveSpeed * move_scale;
        if(!is_fly){
            //地面敌人
            enemyMove.NextTarget();
        }else{
            //飞行敌人
            Vector3 dir = (fly_point - transform.position).normalized;
            Vector3 velocity = dir * moveSpeed * move_scale;
            transform.position += velocity * Time.deltaTime;
            if (fly_point.x > transform.position.x+0.2f)
            {
                transform.eulerAngles = new Vector3(-30, 0, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(30, 180, 0);

            }
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetFloat("_angle", fly_point.x <= transform.position.x ? -30f : 60f);
            GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
        }
    }
    public void IdleController(){
        waitTime -= Time.deltaTime;
        if(waitTime < 0){
            state = EnemyState.Move;
        }
    }
    public void EnemyBehaviorController(){
        switch(state){
            case EnemyState.Move:
                EnemyMoveController();
                break;
            case EnemyState.Idle:
                IdleController();
                break;
            case EnemyState.Attack:
                AttackController();
                break;
            case EnemyState.AttackInterval:
                WaitForInterval();
                break;
        }
    }
    private void AttackController(){
        if(!ObjectIsAvailable(attackTarget)){
            state = lastState;
            try{
                attackTarget.beAttackedTargets.Remove(this);
            }catch{}
        }
    }
    private void WaitForInterval(){
        attackWaitTime -= Time.deltaTime;
        if(attackWaitTime < 0){
            state = EnemyState.Attack;
        }
    }
    public bool ObjectIsAvailable(Char c){
        return c != null && c.state != CharState.Default &&
         c.state != CharState.Start &&
          c.state != CharState.Die && c.hp > 0 &&
          state != EnemyState.Die;
    }
    public void AttackChar(){
        if(state != EnemyState.Attack) return;
        attackTarget.TakeDamage(damage);
    }
    private void OnTriggerStay(Collider other){
        if(other.gameObject.tag == "char" && ObjectIsAvailable(other.gameObject.GetComponent<Char>()) && attackTarget != other.gameObject.GetComponent<Char>() && is_attack){
            if(other.gameObject.GetComponent<Char>().can_def == false) return;
            attackTarget = other.gameObject.GetComponent<Char>();
            other.gameObject.GetComponent<Char>().beAttackedTargets.Add(this);
            other.gameObject.GetComponent<Char>().now_def++;
            lastState = state;
            state = EnemyState.Attack;
        }
    }
    private void UpdateHpBar(){
        if(hp <= 0) return;
        hp_frame.SetActive(true);
        hp_bar.localScale = new Vector3(hp/hp_total,1,1);
        hp_white.DOScaleX(hp/hp_total,0.15f);
    }
    public void TakeDamage(Damage damage){
        switch(damage.dt){
            case Damage.DamageType.Physics:
                hp -= Mathf.Max(0.05f*damage.damage,damage.damage-def);
                break;
            case Damage.DamageType.Magic:
                hp -= Mathf.Max(0.05f*damage.damage,damage.damage*(1-mdef*0.01f));
                break;
        }
        UpdateHpBar();
        if(hp < 0){
            state = EnemyState.Die;
            foreach(Char c in beAttackedTargets){
                c.attackTarget.Remove(this);
                c.now_def--;
            }
        }
    }
    private void OnDestroy()
    {
        BattleController.instance.killNum++;
    }
}