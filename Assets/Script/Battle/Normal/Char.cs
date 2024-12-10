using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine.Events;
public class Char : MonoBehaviour 
{
    private float hpScale = 1.0f;
    private float animScale = 1.0f;
    private SkeletonAnimation skeletonAnimation;
    [Header("动画")]
    public string Start_anim;
    public string Idle_anim;
    public string Attack_anim;
    public string Die_anim;
    [Header("基础数据和组件")]
    public float hp = 50;
    public GameObject HpAnim;
    public GameObject HpBar;
    [HideInInspector]
    public float totalHp;
    [HideInInspector]
    public string state;
    public float damage = 5f;
    private string first = "getPos";
    [HideInInspector]
    public bool can_put = false;
    public enum charType
    {
        lowLand,
        highLand
    }
    public charType ct = charType.lowLand;
    public GameObject getDFrame;
    public GameObject getD;
    public GameObject showD;
    public GameObject attackRange;
    public GameObject dCircle;
    public GameObject targetCollider;
    private string di = "left";
    public GameObject createBtn;
    private int debugId;
    private bool b = false;
    public float def = 50;
    public float adef = 50;
    public BattleController.damageType dt;
    [HideInInspector]
    public float dd;
    public bool is_range = false;
    public List<GameObject> enemyList = new List<GameObject>();
    [Header("技能")]
    public GameObject skillImage;
    public bool have_skill = false;
    public GameObject skillBar;
    private bool inSkill = false;
    public enum skillType{
        time,
        once,
        count
    }
    public skillType skt;
    public enum spType{
        time,
        attack,
        beAttacked,
    }
    public float skillRepeat = 0;
    public float skillCount = 0;
    public spType st;
    public int totalSP;
    public int sp;
    private float spAddTime = 0;
    [System.Serializable]
    public class Skill{
        public UnityEvent OnStart;
        public UnityEvent OnEnd;
    }
    public List<Skill> skillList = new List<Skill>();
    public Color sstartColor;
    public Color sendColor;
    void Start() {
        debugId = Random.Range(10000,99999);
        totalHp = hp;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        getDFrame.SetActive(false);
        state = "Default";
        CharDetail.Instance.Show(this);
        dd = def;
    }
    public void TakeDamage(float damage,BattleController.damageType dt){
        if(dt == BattleController.damageType.Revitalize){
            hp -= damage;
            if(hp >= totalHp){
                hp = totalHp;
            }
            hpScale = hp/totalHp;
            animScale = hp/totalHp;
            return;
        }
        float value = 0;
        if(dt == BattleController.damageType.Physics){
            value = Mathf.Max(0.05f*damage,damage-def);
        }else if(dt == BattleController.damageType.Abillity){
            value = Mathf.Max(0.05f*damage,damage*(1-adef*0.01f));
        }else if(dt == BattleController.damageType.Real){
            value = damage;
        }
        hp -= value;
        if(hp <= 0){
            hp = 0;
            state = Die_anim;
            skeletonAnimation.state.Complete += delegate{
                Destroy(gameObject);
            };
            Destroy(gameObject,2f);
        }
        hpScale = hp/totalHp;
        animScale = hp/totalHp;
    }
    private void UpdateHpBar(){
        HpBar.transform.localScale = new Vector3(hpScale,1,1);
        if(have_skill){
            if(!inSkill){
                skillBar.GetComponent<Image>().color = sstartColor;
                skillBar.transform.localScale = new Vector3((float)sp/totalSP,1,1);
            }else{
                skillBar.GetComponent<Image>().color = sendColor;
                skillBar.transform.localScale = new Vector3(skillCount/skillRepeat,1,1);
            }
        }
        if(HpAnim.transform.localScale.x > animScale){
            HpAnim.transform.localScale -= new Vector3(1,0,0)*Time.deltaTime;
        }else{
            HpAnim.transform.localScale = new Vector3(animScale,1,1);
        }
    }
    public void StartSkill(){
        if(inSkill) return;
        inSkill = true;
        skillList[0].OnStart.Invoke();
        skillCount = skillRepeat;
    }
    private void SkillController(){
        if(!have_skill) return;
        if(st == spType.time && state != "Default"){
            spAddTime += Time.deltaTime;
            if(spAddTime >= 1){
                spAddTime = 0;
                sp++;
                if(sp > totalSP){
                    sp = totalSP;
                }
            }
        }
        if(inSkill){
            if(skt == skillType.time){
                skillCount -= Time.deltaTime;
            }
            if(skillCount <= 0){
                inSkill = false;
                sp = 0;
                skillList[0].OnEnd.Invoke();
            }
        }
    }
    void Update(){
        UpdateHpBar();
        SkillController();
        if(skeletonAnimation.AnimationName != state){
            if(state == Idle_anim || state == Attack_anim){
                skeletonAnimation.state.SetAnimation(0,state,true);
            }else{
                skeletonAnimation.state.SetAnimation(0,state,false);
            }
        }
        if(di == "right" && state != "Default" && (!b)){
            b = true;
            transform.eulerAngles = new Vector3(30,180,0);
            getDFrame.transform.localEulerAngles = new Vector3(30,180,45);
            dCircle.transform.eulerAngles = new Vector3(0,0,0);
            attackRange.transform.localEulerAngles = new Vector3(-60,attackRange.transform.localEulerAngles.y,attackRange.transform.localEulerAngles.z-180);
            targetCollider.transform.localEulerAngles = new Vector3(-30,0,0);
        }
        if(state == "Default"){
            OnStart();
        }
        else if(state == Idle_anim){
            if(enemyList.Count > 0){
                state = Attack_anim;
                return;
            }

        }else if(state == Attack_anim){
            if(enemyList.Count == 0){
                state = Idle_anim;
                return;
            }
            if(dt == BattleController.damageType.Revitalize){
                for(int i = 0;i<enemyList.Count;i++){
                    try{
                        GameObject target = enemyList[i];
                        if(target.GetComponentInParent<Char>().hp >= target.GetComponentInParent<Char>().totalHp){
                            target.GetComponentInParent<Char>().hp = target.GetComponentInParent<Char>().totalHp;
                            enemyList.Remove(target);
                        }
                    }catch{
                        enemyList.Remove(enemyList[i]);
                    }
                }
            }else{
                for(int i = 0;i<enemyList.Count;i++){
                    try{
                        GameObject target = enemyList[i];
                        if(target.GetComponentInParent<Enemy>().hp <= 0){
                            enemyList.Remove(target);
                        }
                    }catch{
                        enemyList.Remove(enemyList[i]);
                    }
                }
            }
        }else if(state == Die_anim){

        }else if(state == Start_anim){

        }
    }
    public void showDetial(){
        getDFrame.SetActive(true);
        getD.SetActive(false);
        if(have_skill){
            skillImage.SetActive(true);
        }
    }
    public void hideDetail(){
        getDFrame.SetActive(false);
        if(have_skill){
            skillImage.SetActive(false);
        }
    }
    private void OnStart(){
        if(first == "getPos"){
            if(!can_put){
                Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 m_MousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, pos.z);
                Vector3 wp = Camera.main.ScreenToWorldPoint(m_MousePos);
                transform.position = new Vector3(wp.x,wp.y,ct == charType.highLand ? -0.3f : -0.01f);
            }
            RaycastHit[] hitInfos;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hitInfos = Physics.RaycastAll(ray,1000);
            if(hitInfos.Length==0){can_put = false;return;}
            foreach(RaycastHit hitInfo in hitInfos)
            {   
                Vector3 p;
                if(hitInfo.collider.gameObject.GetComponent<MapRender>() && hitInfo.collider.gameObject.GetComponent<MapRender>().all){
                    can_put = true;
                    p = hitInfo.collider.gameObject.transform.position;
                    transform.position = p;
                    return;
                }
                if(ct == charType.lowLand && hitInfo.collider.gameObject.tag == "lowland" && hitInfo.collider.gameObject.GetComponent<MapRender>().can_place){
                    can_put = true;
                    p = hitInfo.collider.gameObject.transform.position;
                    transform.position = p;
                    return;
                }else if(ct == charType.highLand && hitInfo.collider.gameObject.tag == "highland" && hitInfo.collider.gameObject.GetComponent<MapRender>().can_place){
                    can_put = true;
                    p = hitInfo.collider.gameObject.transform.position;
                    transform.position = p;
                    return;
                }else{
                    can_put = false;
                }
            }
        }else{
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 m_MousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, pos.z);
            Vector3 wp = Camera.main.ScreenToWorldPoint(m_MousePos);
            if(Vector3.Distance(new Vector3(wp.x,wp.y,getD.transform.position.z),getDFrame.transform.position) < 1.1f){
                getD.transform.position = new Vector3(wp.x,wp.y,getD.transform.position.z);
            }
            GetD();
        }
    }
    void GetD(){
        Vector3 d = getD.transform.position;
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
        showD.transform.eulerAngles = new Vector3(0,0,angle);
        attackRange.transform.eulerAngles = new Vector3(0,0,angle);
    }
    private void FixedUpdate() {
        if(state == "Default"){
            //Debug.Log(debugId.ToString()+" - a");
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 m_MousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, pos.z);
            Vector3 wp = Camera.main.ScreenToWorldPoint(m_MousePos);
            if(Input.GetMouseButton(0) && can_put && first == "getPos"){
                first = "getD";
                getDFrame.SetActive(true);
            }
            if(Input.GetMouseButton(1) && first == "getD" && Vector3.Distance(new Vector3(wp.x,wp.y,getD.transform.position.z),getDFrame.transform.position) > 0.5f){
                getDFrame.SetActive(false);
                CharDetail.Instance.Hide();
                state = Start_anim;
                BattleController.Instance.is_place = false;
                Material material = new Material(GetComponent<MeshRenderer>().material);
                MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
                propertyBlock.SetFloat("_angle",di=="right" ? -30f : 60f);
                GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
                skeletonAnimation.state.Complete += delegate {
                    state = Idle_anim;

                };
            }
        }
    }
    public void Damage(){
        if(st == spType.attack){
            sp++;
            if(sp > totalSP){
                sp = totalSP;
            }
        }
        if(is_range == false){
            try{
                if(dt == BattleController.damageType.Revitalize){
                    enemyList[0].GetComponentInParent<Char>().TakeDamage(-1*damage,dt);
                }else{
                    enemyList[0].GetComponentInParent<Enemy>().TakeDamage(damage,dt);
                }
            }catch{}
            return;
        }
        for(int i = 0;i<enemyList.Count;i++){
            GameObject attackObject = enemyList[i];
            try{
                if(attackObject != null){
                    if(dt == BattleController.damageType.Revitalize){
                        attackObject.GetComponentInParent<Char>().TakeDamage(-1*damage,dt);
                    }else{
                        attackObject.GetComponentInParent<Enemy>().TakeDamage(damage,dt);
                    }
                }
            }catch{}
        }
    }
    private void OnTriggerStay(Collider other) {
        if(dt == BattleController.damageType.Revitalize){
            if(other.gameObject.tag == "attackTarget" &&  state != Die_anim && state != Start_anim && state != "Default"){
                if(other.gameObject.GetComponentInParent<Char>().hp > 0 &&
                 other.gameObject.GetComponentInParent<Char>().hp < other.gameObject.GetComponentInParent<Char>().totalHp &&
                 other.gameObject.GetComponentInParent<Char>().state != "Default" && other.gameObject.GetComponentInParent<Char>().state != "Die" &&
                 enemyList.IndexOf(other.gameObject) == -1){
                    enemyList.Add(other.gameObject);
                }
            }
        }
        else{
            if(other.gameObject.tag == "Enemy" && state != Die_anim && state != Start_anim && state != "Default" && enemyList.IndexOf(other.gameObject) == -1){
                if(other.gameObject.GetComponentInParent<Enemy>().hp > 0){
                    if(ct == charType.lowLand && other.gameObject.GetComponentInParent<Enemy>().enemyType == Enemy.EnemyType.Fly){
                        return;
                    }
                    enemyList.Add(other.gameObject);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Enemy" && enemyList.IndexOf(other.gameObject) != -1 && state != Die_anim && state != Start_anim){
            enemyList.Remove(other.gameObject);
        }  
    }
    private void OnDestroy() {
        if(createBtn != null){
            createBtn.GetComponent<CharCreator>().Respawn();
        }
    }
    private void defadd(){
        def = dd;
    }
    public void defdown(float p,float t){
        def *= 1-p;
        Invoke("defadd",t);
    }
}