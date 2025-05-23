using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Spine.Unity;
using DG.Tweening;

public class Char : MonoBehaviour 
{
    [Header("角色动画")]
    public List<CharAnimation> charAnimations;
    [Header("血量")]
    public float hp;
    public float hp_total;
    [Header("物抗")]
    public float def;
    [Header("法抗")]
    public float mdef;
    [Header("攻击")]
    public Damage damage;
    [Header("攻击间隔")]
    public float damageInterval;
    public CharState state;
    private SkeletonAnimation skeletonAnimation;
    public enum PlacingState
    {
        SetPosition,
        SetDirection
    }
    private PlacingState pstate = PlacingState.SetPosition;
    [HideInInspector]
    public bool is_placing = true;
    [Header("地面/高台")]
    public LandType landType = LandType.lowLand;
    [Header("部署组件")]
    public GameObject darrow;
    public GameObject dframe;
    public GameObject getDFrame;
    public GameObject getDCursor;
    public GameObject attackRange;
    private string di = "left";
    [HideInInspector]
    public List<Enemy> beAttackedTargets;
    [HideInInspector]
    public List<Enemy> attackTarget;
    [Header("可攻击对象个数")]
    public int atk_num = 1;
    [Header("血条")]
    public Transform hp_bar;
    public Transform hp_white;
    private float attackWaitTime;
    [Header("阻挡")]
    public int obs_num = 2;
    public int now_def = 0;
    [HideInInspector]
    public bool can_def;
    private void Start(){
        hp_total = hp;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.Start += delegate{
            if(state == CharState.Attack){
                attackWaitTime = damageInterval;
            }
        };
        skeletonAnimation.state.Complete += delegate{
            switch(state){
                case CharState.Start:
                    state = CharState.Idle;
                    break;
                case CharState.Die:
                    Destroy(gameObject);
                    break;
                case CharState.Attack:
                    state = CharState.AttackInterval;
                    break;
            }
        };
    }

    private void Update(){
        UpdateAnimation();
        if(is_placing){
            HandlePlacing();
        }else{
            CharBehaviorController();
        }
    }
    private void CharBehaviorController(){
        switch(state){
            case CharState.Attack:
                AttackController();
                break;
            case CharState.AttackInterval:
                WaitForInterval();
                break;
        }
        can_def = now_def < obs_num;
    }
    private void AttackController(){
        if(attackTarget.Count == 0){
            state = CharState.Idle;
        }
    }
    private void WaitForInterval(){
        attackWaitTime -= Time.deltaTime;
        if(attackWaitTime < 0){
            state = CharState.Attack;
        }
    }
    private void HandlePlacing(){
        if(pstate == PlacingState.SetPosition){
            if(SetPosition() && Input.GetMouseButtonDown(0)){
                //显示ui
                getDFrame.SetActive(true);
                pstate = PlacingState.SetDirection;
                BattleController.instance.is_showing_range = true;
            }
        }else{
            if(SetDirection()){
                getDFrame.SetActive(false);
                state = CharState.Start;
                BattleController.instance.is_placing = false;
                MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
                propertyBlock.SetFloat("_angle",di=="right" ? -30f : 60f);
                GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
                is_placing = false;
                BattleController.instance.is_showing_range = false;
            }
        }
    }
    private bool SetPosition(){
        bool canPut = false;
        Vector3 mp = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mp);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray,100);
        foreach(RaycastHit hit in raycastHits){
            if(hit.collider.tag != "mapRender") continue;
            if(!hit.collider.gameObject.GetComponent<MapRender>().can_place) continue;
            if(hit.collider.gameObject.GetComponent<MapRender>().type == LandType.lowLand && landType == LandType.lowLand){
                transform.position = hit.collider.transform.position;
                canPut = true;
                break;
            }else if(hit.collider.gameObject.GetComponent<MapRender>().type == LandType.highLand && landType == LandType.highLand){
                transform.position = hit.collider.transform.position;
                canPut = true;
                break;
            }
        }
        if(!canPut){
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 m_MousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, pos.z);
            Vector3 wp = Camera.main.ScreenToWorldPoint(m_MousePos);
            transform.position = new Vector3(wp.x,wp.y,landType == LandType.highLand ? -0.3f : -0.01f);
        }
        return canPut;
    }
    private bool SetDirection(){
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 m_MousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, pos.z);
        Vector3 wp = Camera.main.ScreenToWorldPoint(m_MousePos);
        if(Vector3.Distance(new Vector3(wp.x,wp.y,getDCursor.transform.position.z),getDFrame.transform.position) < 1.1f){
            getDCursor.transform.position = new Vector3(wp.x,wp.y,getDCursor.transform.position.z);
        }
        Vector3 d = getDCursor.transform.position;
        Vector3 o = getDFrame.transform.position;
        int angle = 0;
        if(d.y > o.y && Mathf.Abs(d.y-o.y)>0.8f){
            di = "left";
            angle = 90;
        }else if(d.y < o.y && Mathf.Abs(d.y-o.y)>0.8f){
            di = "left";
            angle = 270;
        }else if(d.x < o.x){
            di = "right";
            angle = 180;
        }else if(d.x > o.x){
            di = "left";
            angle = 0;
        }
        darrow.transform.eulerAngles = new Vector3(0,0,angle);
        attackRange.transform.eulerAngles = new Vector3(0,0,angle);
        return Input.GetMouseButton(1) && Vector3.Distance(new Vector3(wp.x,wp.y,getDCursor.transform.position.z),getDFrame.transform.position) > 0.5f;
    }
    private void UpdateAnimation(){
        CharAnimation anim = GetAnimationName(state);
        if(skeletonAnimation.AnimationName != anim.anim){
            skeletonAnimation.state.SetAnimation(0,anim.anim,anim.isLoop);
        }
    }
    public CharAnimation GetAnimationName(CharState cstate){
        foreach(CharAnimation ca in charAnimations){
            if(ca.state == cstate){
                return ca;
            }
        }
        Debug.LogWarning("Char Animation not found");
        return new CharAnimation(){
            state = cstate,
            anim = "Default",
        };
    }
    private bool TargetIsAvailable(Enemy e){
        return e.state != EnemyState.Die && e.hp > 0 && attackTarget.IndexOf(e) == -1;
    }
    private void OnTriggerStay(Collider other) {
        if(state == CharState.Die || state == CharState.Default || state == CharState.Start) return;
        if(other.gameObject.tag == "Enemy" && TargetIsAvailable(other.gameObject.GetComponent<Enemy>())){
            attackTarget.Add(other.gameObject.GetComponent<Enemy>());
            state = CharState.Attack;
            other.gameObject.GetComponent<Enemy>().beAttackedTargets.Add(this);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(state == CharState.Die || state == CharState.Default || state == CharState.Start) return;
        if(other.gameObject.tag == "Enemy"){
            if(attackTarget.IndexOf(other.gameObject.GetComponent<Enemy>()) != -1){
                try{
                    attackTarget.Remove(other.gameObject.GetComponent<Enemy>());
                    other.gameObject.GetComponent<Enemy>().beAttackedTargets.Remove(this);
                }catch{

                }
            }
        }   
    }
    private void UpdateHpBar(){
        if(hp < 0) return;
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
            state = CharState.Die;
            attackTarget.Clear();
            foreach(Enemy e in beAttackedTargets){
                e.attackTarget = null;
            }
        }
    }
    public void AttackEnemy(){
        int n = 0;
        foreach(Enemy e in attackTarget){
            try{
                e.TakeDamage(damage);
                n++;
                if(n >= atk_num) return;
            }catch{
                Debug.LogWarning("Error Attack");
            }
        }
    }
}