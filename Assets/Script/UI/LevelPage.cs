using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class LevelPage : MonoBehaviour 
{
    public static LevelPage instance;
    [Header("容器")]
    public CanvasGroup frame;
    [Header("关卡编号")]
    public Text level_num;
    [Header("关卡名称")]
    public Text level_name;
    [Header("关卡推荐等级")]
    public Text level_level;
    [Header("关卡介绍")]
    public Text level_desc;
    private bool is_show = false;
    private void Start(){
        instance = this;
        frame.DOFade(0,0);
    }
    public void ShowLevelPage(string num,string name,string level,string desc){
        if(!is_show){
            frame.DOFade(1,0.3f);
            is_show = true;
        }
        level_num.text = num;
        level_name.text = name;
        level_level.text = level;
        level_desc.text = desc;
    }
    private void Update(){
        if(Input.GetMouseButtonDown(1)){
            if(is_show){
                frame.DOFade(0,0.3f);
                is_show = false;
            }
        }   
    }
}