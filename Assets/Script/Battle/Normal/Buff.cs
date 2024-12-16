using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
[System.Serializable]
public class Effect
{
    public enum EffectType{
        ActiveOriginium,
        Cold,
        Freeze,
        Dizzy
    }
    public EffectType effectType;
    public float time;
    public bool flag = false;
    public float ea = 0;
}
public class Buff : MonoBehaviour 
{
    public List<Effect> buffs;
    public GameObject buffIconPrefab;
    public bool isEnemy = true;
    private Effect temp;
    private GameObject bi;
    private GameObject effectObj;
    public Sprite blood;
    public Sprite cold;
    private void Start() {
        if(isEnemy){
            bi = Instantiate(buffIconPrefab,gameObject.transform);
            bi.transform.localPosition = new Vector3(0,3.85f,0);
            effectObj = bi.transform.GetChild(0).GetChild(0).gameObject;
            effectObj.SetActive(false);
        }
    }
    private void Update() {
        if(bi) bi.transform.localEulerAngles = transform.localEulerAngles;
        foreach(Effect effect in buffs) {
            if(isEnemy){
                Enemy enemy = GetComponent<Enemy>();
                switch(effect.effectType){
                    case Effect.EffectType.ActiveOriginium:
                        if(!effect.flag){
                            effect.flag = true;
                            effect.ea = enemy.damage;
                            enemy.damage *= 1.5f;
                        }
                        enemy.TakeDamage(180.0f*Time.deltaTime,BattleController.damageType.Real);
                        break;
                    case Effect.EffectType.Dizzy:
                        if(!effect.flag){
                            effect.flag = true;
                            enemy.TakeDamage(1000,BattleController.damageType.Abillity);
                        }
                        enemy.atkSpeedScale = 0;
                        enemy.moveSpeedScale = 0;
                        break;
                }
            }else{
                Char chara = GetComponent<Char>();
                switch(effect.effectType){
                    case Effect.EffectType.ActiveOriginium:
                        if(!effect.flag){
                            effect.flag = true;
                            effect.ea = chara.damage;
                            chara.damage *= 1.5f;
                        }
                        chara.TakeDamage(180.0f*Time.deltaTime,BattleController.damageType.Real);
                        break;
                }
            }
            effect.time -= Time.deltaTime;
            if(effect.time < 0){
                switch(effect.effectType){
                    case Effect.EffectType.ActiveOriginium:
                        if(isEnemy){
                            GetComponent<Enemy>().damage = effect.ea;
                        }else{
                            GetComponent<Char>().damage = effect.ea;
                        }
                        break;
                    case Effect.EffectType.Dizzy:
                        if(isEnemy){
                            GetComponent<Enemy>().atkSpeedScale = 1;
                            GetComponent<Enemy>().moveSpeedScale = 1;
                        }
                        break;
                }
                temp = effect;
            }
        }
        if(temp != null){
            buffs.Remove(temp);
            temp = null;
        }
        if(isEnemy){
            if(buffs.Count > 0){
                effectObj.SetActive(true);
                Effect b = buffs[0];
                effectObj.GetComponent<Image>().enabled = true;
                switch(b.effectType){
                    case Effect.EffectType.ActiveOriginium:
                        effectObj.GetComponent<Image>().sprite = blood;
                        break;
                    case Effect.EffectType.Cold:
                        effectObj.GetComponent<Image>().sprite = cold;
                        break;
                    default:
                        effectObj.GetComponent<Image>().enabled = false;
                        break;
                }
            }else{
                effectObj.SetActive(false);
            }
        }
    }
}