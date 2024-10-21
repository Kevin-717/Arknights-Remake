using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BattleController : MonoBehaviour 
{
    public enum damageType{
        Physics,
        Abillity,
        Real,
        Revitalize
    }
    public static BattleController Instance;
    [HideInInspector]
    public bool is_place = false;
    [HideInInspector]
    public bool is_lowland = true;
    public float ts = 1.0f;
    private bool is_pause = false;
    public GameObject pauseFrame;
    public Button pauseBtn;
    public Button speedBtn;
    [Range(0.1f,1)]public float costSpeed = 0.6f;
    private float costTime = 0; 
    public Transform costImage;
    public Text costText;
    public int cost = 10;
    public Text enemyText;
    public Text lifeText;
    public int life = 5;
    [HideInInspector]
    public int enemyNum = 0;
    public string parent_scene = "";
    [Header("资源Sprite填入")]
    public Sprite runImage;
    public Sprite pauseImage;
    public Sprite _1x;
    public Sprite _2x;
    public Sprite _3x;
    [Header("射线检测")]
    public LayerMask charLayerMask;
    [Header("装置设置(弩炮)")]
    public List<GameObject> machines = new List<GameObject>();
    private int machine_index = 0;
    private Char charObj;
    private bool onSelect = false;
    private void Start() {
        Instance = this;
        pauseFrame.SetActive(false);
    }
    public void Pause(){
        is_pause = !is_pause;
        if(is_pause){
            pauseFrame.SetActive(true);
            pauseBtn.GetComponent<Image>().sprite = pauseImage;
        }else{
            pauseFrame.SetActive(false);
            pauseBtn.GetComponent<Image>().sprite = runImage;
        }
    }
    public void SetSpeed(){
        if((!is_place) && (!is_pause)){
            ts += 1.0f;
            if(ts > 3.0f){
                ts = 1.0f;
            }
            Sprite[] imgs = {_1x,_2x,_3x};
            speedBtn.GetComponent<Image>().sprite = imgs[Int32.Parse((ts-1).ToString())];
        }
    }
    private void handleCost(){
        costTime += Time.deltaTime;
        costImage.localScale = new Vector3(costTime/costSpeed,1,1);
        if(costTime >= costSpeed){
            cost++;
            costTime = 0;
        }
        costText.text = cost.ToString();
    }
    private void UpdateText(){
        enemyText.text = enemyNum.ToString() + " / " + EnemyController.Instance.enemyNum.ToString();
        lifeText.text = life.ToString();
    }
    private void Update() {
        UpdateText();
        handleCost();
        handleClick();
        timeScaleController();
    }
    public void timeScaleController(){
        if(is_place){
            Time.timeScale = 0.1f;
        }else{
            if(!is_pause){
                Time.timeScale = ts;
            }else{
                Time.timeScale = 0;
            }
        }
    }
    public void handleClick(){
        if(Input.GetKeyDown(KeyCode.Q)){
            CharDetail.Instance.Hide();
            charObj.hideDetail();
            is_place = false;
            onSelect = false;
            
        }
        if(onSelect || is_place) return;
        if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray,100,charLayerMask);
            if(hits.Length > 0){
                RaycastHit hit = hits[0];
                if(hit.collider.gameObject.GetComponentInParent<Char>().state != "Default"){
                    charObj = hit.collider.gameObject.GetComponentInParent<Char>();
                    charObj.showDetial();
                    CharDetail.Instance.Show(charObj);
                    is_place = true;
                    onSelect = true;
                }

            }
        }
    }
    public void QuitBattle(){
        SceneManager.LoadScene(parent_scene);
    }
    public void ActiveMachine(){
        machines[machine_index].SetActive(true);
        if(machine_index < machines.Count-1){
            machine_index++;
        }
    }
}