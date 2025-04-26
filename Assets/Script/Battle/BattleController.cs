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
    public int cost = 0;
    public float costSpeed = 1;
    private float costCountdown;
    public Transform costProgress;
    public Text costText;
    [HideInInspector]
    public int killNum = 0;
    public int life = 3;
    private void Start() {
        instance = this;
        costCountdown = costSpeed;
    }
    private void Update() {
        if(is_placing || is_showing_range){
            Time.timeScale = 0.03f;
        }else{
            Time.timeScale = ts;
        }
        Cost();
        UITop();
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
    private void Cost()
    {
        costCountdown -= Time.deltaTime;
        if (costCountdown <= 0)
        {
            costCountdown = costSpeed;
            cost += 1;
        }
        costProgress.localScale = new Vector3((costSpeed-costCountdown)/costSpeed, 1, 1);
        costText.text = cost.ToString();
    }
    private void UITop()
    {
        enemy_num.text = killNum.ToString()+" / "+EnemyController.Instance.enemyNum.ToString();
        life_text.text = life.ToString();

    }
}