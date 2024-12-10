using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Hole : MonoBehaviour 
{
    private void OnTriggerStay(Collider other){
        if(other.transform.parent.gameObject && other.gameObject.tag == "Enemy"){
            if(other.transform.parent.gameObject.GetComponent<Enemy>().state != other.transform.parent.gameObject.GetComponent<Enemy>().Die_anim&&
            other.transform.parent.gameObject.GetComponent<Enemy>().state != "Start"){
                other.transform.parent.gameObject.GetComponent<Enemy>().TakeDamage(999999,BattleController.damageType.Real);
            }
        }
    }
}