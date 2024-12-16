using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Emp : MonoBehaviour 
{
    private Animator animator;
    private int boom = Animator.StringToHash("boom");
    private void Start() {
        animator = GetComponent<Animator>();
    }
    private void Update() {
        if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray,1000);
            foreach(RaycastHit hit in hits){
                if(hit.collider.gameObject == gameObject){
                    if(BattleController.Instance.cost >= 10){
                        BattleController.Instance.cost -= 10;
                        animator.SetTrigger(boom);
                    }
                }
            }
        }
    }
    public void RemoveSelf(){
        Destroy(gameObject);
    }
    public void Boom(){
        Debug.Log("Boom");
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy")){
            if(Vector3.Distance(g.transform.position,transform.position) > 2.1f){
                continue;
            }
            if(g.GetComponent<Enemy>()){
                if(g.GetComponent<Enemy>().state != "Start" && g.GetComponent<Enemy>().state != g.GetComponent<Enemy>().Die_anim){
                    Effect effect = new Effect(){
                        effectType = Effect.EffectType.Dizzy,
                        time = 7
                    };
                    g.GetComponent<Buff>().buffs.Add(effect);
                }
            }
        }
    }
}