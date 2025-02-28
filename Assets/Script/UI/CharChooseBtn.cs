using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharChooseBtn : MonoBehaviour 
{
    [Header("角色半身像")]
    public Image char_img;
    [Header("角色星标")]
    public Image char_star;
    [Header("角色角标")]
    public Image char_bottom;
    [Header("角色角标泛光")]
    public Image char_bottom_bloom;
    [Header("角色职业图标")]
    public Image char_profession_icon;
    [Header("角色英文")]
    public Text char_en;
    [Header("角色名")]
    public Text char_ch;
    [Header("选择框")]
    public GameObject selected_frame;
    public int cid;
    [Header("信息容器")]
    public GameObject info_frame;
    [Header("空容器")]
    public GameObject empty_frame;
    private bool flag = false;
    public bool is_shower = false;
    private void Start(){
        if(is_shower){
            info_frame.SetActive(false);
            empty_frame.SetActive(true);
            GetComponent<Image>().enabled = false;
        }else{
            foreach(CharInfo ci in BattleInfo.instance.cis){
                if(ci.cid == cid){
                    flag = true;
                    selected_frame.SetActive(true);
                }
            }
        }
    }
    private void Update(){
        
    }
    public void SetData(CharInfo c){
        CharUIData cui = c.cui;
        cid = c.cid;
        char_img.sprite = cui.icon_half;
        char_bottom.sprite = cui.sp.bottom;
        GetComponent<Image>().sprite = cui.sp.bkg;
        char_profession_icon.sprite = cui.icon_type;
        char_star.sprite = cui.sp.star;
        char_bottom_bloom.sprite = cui.sp.bloom;
        char_en.text = c.name_en;
        char_ch.text = c.name;
    }
    public void SetDataShow(CharInfo c){
        info_frame.SetActive(true);
        empty_frame.SetActive(false);
        GetComponent<Image>().enabled = true;
        SetData(c);
    }
    private CharInfo getChar(int cid){
        foreach(CharInfo ci in CharData.instance.charDatas){
            if(ci.cid == cid){
                return ci;
            }
        }
        return null;
    }
    public void ShowEmpty(){
        info_frame.SetActive(false);
        GetComponent<Image>().enabled = false;
        empty_frame.SetActive(true);
    }
    public void Choosed(){
        if(is_shower){
            SceneManager.LoadScene("Scenes/UI/CharList");
        }else{
            flag = !flag;
            if(flag){
                BattleInfo.instance.cis.Add(getChar(cid));
            }else{
                BattleInfo.instance.cis.Remove(getChar(cid));
            }
            selected_frame.SetActive(flag);
        }
    }
}