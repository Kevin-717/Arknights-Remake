using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Pathfinding;
using System;

public class Enemy : MonoBehaviour{
    private SkeletonAnimation spine_anim;
    // private AIDestinationSetter aIDestinationSetter;
    // private AIPath aIPath;
    private EnemyMove enemyMove;
    public float speedT = 3;
    public float speed = 3;
    private GameObject attackObject;
    //[HideInInspector]
    public List<EnemyPath> move_line;
    public int move_index = 0;
    private float waitTime = 0;
    public float hp = 50f;
    public Image HpAnim;
    public Image HpBar;
    [HideInInspector]
    public float totalHp;
    public float damage = 5f;
    private float hpScale = 1.0f;
    private float animScale = 1.0f;
    public string Move_anim;
    public string Idle_anim;
    public string Attack_anim;
    public string Die_anim;
    public string HpAddAnim;
    [HideInInspector]
    public string state;
    [HideInInspector]
    public bool useSpecial = false;
    public float def = 50;
    public float adef = 50;
    public bool haveStart = false;
    public bool haveNormalAttack = true;
    public bool haveSecondLife = false;
    private bool flag = false;
    private bool isDieBind = false;
    public BattleController.damageType dt;
    public enum EnemyType
    {
        Ground,
        Fly
    }
    public EnemyType enemyType;
    private Rigidbody rb;
    private GameObject point;
    [HideInInspector]
    public bool inhole = false;
    public float addTime = 5;
    public List<string> tags = new List<string>();
    public float atkSpeedScale = 1.0f;
    public float moveSpeedScale = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        speedT = speed;
        spine_anim = GetComponent<SkeletonAnimation>();
        // aIDestinationSetter = GetComponent<AIDestinationSetter>();
        // aIPath = GetComponent<AIPath>();
        enemyMove = GetComponent<EnemyMove>();
        // aIPath.maxSpeed = 0;
        // point = Instantiate(Resources.Load("Prefab/Battle/Point") as GameObject,transform.position,Quaternion.identity);
        // aIDestinationSetter.target = point.transform;
        // aIDestinationSetter.target.position = move_line[move_index].path;
        enemyMove.seeker = GetComponent<Seeker>();
        Debug.Log(enemyMove.seeker);
        totalHp = hp;
        if(!haveStart){
            state = Move_anim;
        }
        rb = GetComponent<Rigidbody>();
        checkPoint();
        HpBar.transform.parent.gameObject.SetActive(false);
    }
    public void checkPoint(){
        if(move_index >= move_line.Count){
            BattleController.Instance.life--;
            Destroy(gameObject);
            return;
        }
        if(move_line[move_index].pointType == PointType.wait){
            waitTime = move_line[move_index].waitTime;
            state = Idle_anim;
            move_index--;
            return;
        }else if(move_line[move_index].pointType == PointType.disappear){
            GetComponent<MeshRenderer>().enabled = false;
            HpBar.transform.parent.gameObject.SetActive(false);
            inhole = true;
            return;
        }else if(move_line[move_index].pointType == PointType.appear){
            transform.position = move_line[move_index].path;
            HpBar.transform.parent.gameObject.SetActive(true);
            inhole = false;
            GetComponent<MeshRenderer>().enabled = true;
            move_index++;
            return;
        }
        enemyMove.UpdatePath(move_line[move_index<0?0:move_index].path);
    }
    private void Move(){
        if(Vector3.Distance(move_line[move_index<0?0:move_index].path,transform.position) <= 0.3f){
            move_index++;
            checkPoint();
        }else{
            if(enemyType == EnemyType.Fly){
                float x = transform.position.x;
                float y = transform.position.y;
                float z = transform.position.z;
                float dx = move_line[move_index<0?0:move_index].path.x;
                float dy = move_line[move_index<0?0:move_index].path.y;
                float dz = move_line[move_index<0?0:move_index].path.z;
                float mx = (Mathf.Abs(x-dx)<=0.1f)?0:(x>dx?-1:1);
                float my = (Mathf.Abs(y-dy)<=0.1f)?0:(y>dy?-1:1);
                //float mz = (Mathf.Abs(z-dz)<=0.1f)?0:(z>dz?-1:1);
                if(mx < 0){
                    transform.eulerAngles = new Vector3(30,180,0);
                    mx=-mx;
                }else{
                    transform.eulerAngles = new Vector3(-30,0,0);
                }
                Material material = new Material(GetComponent<MeshRenderer>().material);
                MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
                propertyBlock.SetFloat("_angle",mx>0 ? -30f : 60f);
                GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
                Vector3 movement = new Vector3(mx,my,0);
                rb.transform.Translate(movement*speed*Time.deltaTime);
                rb.transform.position = new Vector3(rb.transform.position.x,rb.transform.position.y,0);
            }else{
                enemyMove.NextTarget();
                // float x = transform.position.x;
                // float dx = move_line[move_index].path.x;
                // float mx = (Mathf.Abs(x-dx)<=0.1f)?0:(x>dx?-1:1);
                // if(mx < 0){
                //     transform.eulerAngles = new Vector3(30,180,0);
                // }else{
                //     transform.eulerAngles = new Vector3(-30,0,0);
                // }
            }
            // Vector3 movement = new Vector3(mx,my,mz);
            // transform.Translate(movement*speed*Time.deltaTime);
        }
    }
    private void Delay(){
        waitTime -= Time.deltaTime;
        if(waitTime < 0){
            state = Move_anim;
            Debug.Log("Wait finish");
            move_index ++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        speed = speedT * moveSpeedScale;
        spine_anim.timeScale = atkSpeedScale;
        UpdateHpBar();
        if(spine_anim.AnimationName != state){
            if(state != Die_anim){
                spine_anim.state.SetAnimation(0,state,true);
            }else{
                spine_anim.state.SetAnimation(0,state,false);
            }
        }
        if(state == HpAddAnim){
            addTime -= Time.deltaTime;
            if(addTime <= 0){
                state = Move_anim;
                flag = true;
            }
            return;
        }
        else if(state == Move_anim){
            if(enemyType == EnemyType.Ground){
                enemyMove.speed = speed;
            }
            Move();
        }else if(state == Idle_anim){
            enemyMove.speed = 0;
            Delay();
        }else if(state == Attack_anim && !useSpecial){
            enemyMove.speed = 0;
            if(attackObject == null || attackObject.GetComponentInParent<Char>().hp <= 0){
                attackObject = null;
                state = Move_anim;
            }
        }else{
            enemyMove.speed = 0;
        }
    }
    public void TakeDamage(float damage,BattleController.damageType dt){
        HpBar.transform.parent.gameObject.SetActive(true);
        if(state == HpAddAnim) return;
        float value = 0;
        if(dt == BattleController.damageType.Physics){
            value = Mathf.Max(0.05f*damage,damage-def);
        }else if(dt == BattleController.damageType.Abillity){
            value = Mathf.Max(0.05f*damage,damage*(1-adef*0.01f));
        }else if(dt == BattleController.damageType.Revitalize){
            hp += damage;
            if(hp > totalHp){
                hp = totalHp;
            }
            hpScale = hp/totalHp;
            animScale = hp/totalHp;
            return;
        }else if(dt == BattleController.damageType.Real){
            value = damage;
        }
        hp -= value;
        if(hp <= 0){
            if(!haveSecondLife || flag){
                if(isDieBind) return;
                hp = 0;
                state = Die_anim;
                enemyMove.speed = 0;
                isDieBind = true;
                spine_anim.state.Complete += delegate{
                    if(state == "Die"){
                        Destroy(gameObject);
                    }
                    Destroy(gameObject,1f);
                };
            }else{
                hp = totalHp;
                state = HpAddAnim;
                return;
            }
        }
        hpScale = hp/totalHp;
        animScale = hp/totalHp;
    }
    private void OnDestroy() {
        BattleController.Instance.enemyNum++;
        Destroy(point);
    }
    public void Damage(){
        try{
            if(attackObject != null && (!useSpecial)){
                attackObject.GetComponentInParent<Char>().TakeDamage(damage,dt);
            }
        }catch{}
    }
    private void UpdateHpBar(){
        HpBar.transform.localScale = new Vector3(hpScale,1,1);
        if(HpAnim.transform.localScale.x > animScale){
            HpAnim.transform.localScale -= new Vector3(1,0,0)*Time.deltaTime;
        }else{
            HpAnim.transform.localScale = new Vector3(animScale,1,1);
        }
    }
    private void OnTriggerStay(Collider other) {
        if((!haveNormalAttack) || inhole || state == HpAddAnim) return;
        if(enemyType == EnemyType.Fly){
            return;
        }
        if(state != Die_anim && state != Attack_anim){
            if(other.gameObject.tag == "attackTarget"){
                if(other.gameObject.GetComponentInParent<Char>().hp > 0 && 
                other.gameObject.GetComponentInParent<Char>().state != "Default"){
                    state = Attack_anim;
                    attackObject = other.gameObject;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if(!haveNormalAttack || state == HpAddAnim) return;
        if (enemyType == EnemyType.Fly) return;
        if(state != Die_anim && other.gameObject == attackObject){
            state = Move_anim;
            attackObject = null;
        }
    }
}