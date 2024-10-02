using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ActiveOriginium : MonoBehaviour 
{
    private void Update() {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
            if(enemy.GetComponent<Enemy>() != null &&
             Vector3.Distance(enemy.transform.position,transform.position) < 0.7f){
                bool flag = true;
                foreach(Effect eff in enemy.GetComponent<Buff>().buffs){
                    if(eff.effectType == Effect.EffectType.ActiveOriginium){
                        flag = false;
                        break;
                    }
                }
                if(!flag){
                    continue;
                }
                Effect effect = new Effect(){
                    effectType = Effect.EffectType.ActiveOriginium,
                    time = 60*5
                };
                enemy.GetComponent<Buff>().buffs.Add(effect);
            }
        }
        foreach(GameObject chara in GameObject.FindGameObjectsWithTag("char")){
            if(chara.GetComponent<Char>() != null && chara.GetComponent<Char>().state != "Default" &&
             Vector3.Distance(chara.transform.position,transform.position) < 0.7f){
                bool flag = true;
                foreach(Effect eff in chara.GetComponent<Buff>().buffs){
                    if(eff.effectType == Effect.EffectType.ActiveOriginium){
                        flag = false;
                        break;
                    }
                }
                if(!flag){
                    continue;
                }
                Effect effect = new Effect(){
                    effectType = Effect.EffectType.ActiveOriginium,
                    time = 60*5
                };
                chara.GetComponent<Buff>().buffs.Add(effect);
                Debug.Log("Add buff to -> " + chara.gameObject.name);
            }
        }
    }
    private void OnDestroy() {
    }
}