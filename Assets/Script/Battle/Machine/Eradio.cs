using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
public class Eradio : MonoBehaviour 
{
    public Transform sp_progress_image;
    public Image sp_progress;
    public int sp = 0;
    private int sp_max = 50;
    private float st = 0;
    private float st_max = 10;
    public Color sp_color;
    public Color sp_skill_color;
    private float timer = 1;
    private float stimer = 1;
    private List<Enemy> enemies = new List<Enemy>();
    private void Start() {
        st = st_max;
    }
    private void Update() {
        if(sp < sp_max){
            timer -= Time.deltaTime;
            if(timer < 0){
                sp++;
                timer = 1;
                return;
            }
            sp_progress_image.localScale = new Vector3((float)sp/sp_max,1,1);
            sp_progress.color = sp_color;
        }else{
            foreach(Enemy enemy in enemies){
                enemy.moveSpeedScale = 0.3f;
            }
            if(stimer < 0){
                stimer = 1;
                foreach(Enemy enemy in enemies){
                    enemy.TakeDamage(200,BattleController.damageType.Abillity);
                    if(enemy.tags.IndexOf("act28") != -1){
                        enemy.GetComponent<OverloadController>().energy++;
                    }
                }
            }
            st -= Time.deltaTime;
            stimer -= Time.deltaTime;
            if(st <= 0){
                st = st_max; 
                sp = 0;
                foreach(Enemy enemy in enemies){
                    enemy.moveSpeedScale = 1;
                }
                return;
            }
            sp_progress_image.localScale = new Vector3(st/st_max,1,1);
            sp_progress.color = sp_skill_color;
        }
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.transform.parent.gameObject.name);
        if(other.gameObject.tag == "Enemy"){
            if(enemies.IndexOf(other.transform.parent.gameObject.GetComponent<Enemy>()) == -1){
                enemies.Add(other.transform.parent.gameObject.GetComponent<Enemy>());
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Enemy"){
            if(enemies.IndexOf(other.transform.parent.gameObject.GetComponent<Enemy>()) != -1){
                enemies.Remove(other.transform.parent.gameObject.GetComponent<Enemy>());
                other.transform.parent.gameObject.GetComponent<Enemy>().moveSpeedScale = 1;
            }
        }
    }
}