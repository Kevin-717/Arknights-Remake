using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharListUI : MonoBehaviour 
{
    [Header("角色按钮预制体")]
    public GameObject CharBtnPrefab;
    [Header("角色列表Transform")]
    public Transform cBtnParent;
    public List<CharInfo> chars;
    public static CharListUI instance;
    private void Start(){
        instance = this;
        LoadChar();
        CreateCharBtn();
    }
    private void CreateCharBtn(){
        foreach(CharInfo c in chars){
            GameObject btn = Instantiate(CharBtnPrefab,cBtnParent);
            CharBtnUI cui = btn.GetComponent<CharBtnUI>();
            cui.ci = c;
        }
    }
    private void LoadChar(){
        foreach(CharInfo c in BattleInfo.instance.cis){
            chars.Add(c);
        }
        GetComponent<RectTransform>().localScale = new Vector3(Screen.width*1.0f/1920,Screen.height*1.0f/1080,1);
    }
    private void Update(){
        
    }
}