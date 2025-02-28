using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class BattleController : MonoBehaviour 
{
    public static BattleController instance;
    [Header("资源Sprite")]
    public Sprite pause;
    public Sprite running;
    public List<Sprite> time_scales;
    private bool pause_flag = false;
    [Header("控件对象")]
    public Image pause_btn;
    public Image time_btn;
    public GameObject pause_frame;
    public float ts = 1;
    private float temp_ts;
    [HideInInspector]
    public bool is_placing = false;
    [HideInInspector]
    public bool is_showing_range = false;
    [Header("ui")]
    public Text enemy_num;
    public Text life_text;
    [HideInInspector]
    public LandType placingType;
    private void Start() {
        instance = this;
    }
    private void Update() {
        if(is_placing || is_showing_range){
            Time.timeScale = 0.03f;
        }else{
            Time.timeScale = ts;
        }

    }
    public void Pause(){
        if(pause_flag){
            ts = temp_ts;
            pause_btn.sprite = running;
        }else{
            temp_ts = ts;
            ts = 0;
            pause_btn.sprite = pause;
        }
        pause_flag = !pause_flag;
        pause_frame.SetActive(pause_flag);
    }
    public void SetSpeed(){
        if(ts > 2){
            ts = 1;
        }else{
            ts++;
        }
        time_btn.sprite = time_scales[(int)(ts-1)];
    }
}