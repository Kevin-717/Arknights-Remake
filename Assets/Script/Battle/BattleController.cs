using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class BattleController : MonoBehaviour 
{
    public static BattleController instance;
    public string parentScene;
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
    public GameObject finFrame;
    public RectTransform finText;
    public CanvasGroup black;
    public bool isRunning = true;
    private void Start() {
        instance = this;
        costCountdown = costSpeed;
        black.gameObject.SetActive(false);
        black.GetComponent<Image>().DOFade(0,0);
    }
    private void Update() {
        if(is_placing || is_showing_range){
            Time.timeScale = 0.03f;
        }else{
            Time.timeScale = ts;
        }
        Cost();
        UITop();
        if(killNum == EnemyController.Instance.enemyNum)
        {
            finFrame.SetActive(true);
            finText.DOLocalMoveX(-245, 1f).OnComplete(() =>
            {
                Quit();
            });
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
    public void Quit()
    {
        isRunning = false;
        ts = 1;
        black.gameObject.SetActive(true);
        black.GetComponent<Image>().DOFade(1, 0.8f).OnComplete(() =>
        {
            SceneManager.LoadScene(parentScene);
        });
    }
}