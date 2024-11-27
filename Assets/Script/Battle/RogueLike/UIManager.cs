using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIManager : MonoBehaviour 
{
    public static UIManager instance;
    [Header("战斗入口UI")]
    public Text battleTitle;
    public Text battleDescription;
    public GameObject battleFrame;
    public string currentSence;
    private void Start() {
        instance = this;
    }
    public void ShowBattleEntrance(string name,string description,string sence){
        battleFrame.SetActive(true);
        battleTitle.text = name;
        battleDescription.text = description;
        currentSence = sence;
    }
    private void Update() {
        if(Input.GetMouseButtonDown(1)){
            battleFrame.SetActive(false);
        }
    }
}