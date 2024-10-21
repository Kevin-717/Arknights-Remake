using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Effect
{
    public enum EffectType{
        ActiveOriginium
    }
    public EffectType effectType;
    public float time;
    public bool flag = false;
    public float ea = 0;
}
public class Buff : MonoBehaviour 
{
    public List<Effect> buffs;
    public bool isEnemy = true;
    private Effect temp;
    private void Update() {
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
                if(isEnemy){
                    GetComponent<Enemy>().damage = effect.ea;
                }else{
                    GetComponent<Char>().damage = effect.ea;
                }
                temp = effect;
            }
        }
        if(temp != null){
            buffs.Remove(temp);
            temp = null;
        }
    }
}