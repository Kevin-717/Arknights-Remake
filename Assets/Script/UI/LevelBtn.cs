using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class LevelBtn : MonoBehaviour 
{
    public LevelDescText ldt;
    private void Start(){
        
    }
    public void OnClicked(){
        LevelPage.instance.ShowLevelPage(ldt.level_num,ldt.level_name,ldt.level_c,ldt.level_desc);
        BattleInfo.instance.scene_string = ldt.scene_name;
    }
    private void Update(){
        
    }
}