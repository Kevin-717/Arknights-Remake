using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PaoDan : MonoBehaviour 
{
    public float damage = 0;
    public BattleController.damageType damageType = BattleController.damageType.Physics;
    public GameObject target;
    [Range(0,1)]public float def_down = 0;
    public float t = 5;
    private void OnDestroy() {
        foreach(GameObject chara in GameObject.FindGameObjectsWithTag("char")){
            if(Vector3.Distance(chara.transform.position,target.transform.position) <= 1.5){
                chara.GetComponent<Char>().def = target.GetComponent<Char>().def*(1.0f-def_down);
                chara.GetComponent<Char>().TakeDamage(damage,damageType);
                chara.GetComponent<Char>().defdown(def_down,t);
            }
        }
    }
}