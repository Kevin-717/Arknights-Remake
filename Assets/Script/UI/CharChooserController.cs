using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharChooserController : MonoBehaviour 
{
    private List<CharInfo> tci = new List<CharInfo>();
    public List<CharChooseBtn> ccbts;
    private void Start(){
    }

    private void Update(){
        UpdateList();
    }
    public void UpdateList(){
        if(tci != BattleInfo.instance.cis){
            int i = 0;
            foreach(CharChooseBtn b in ccbts){
                b.ShowEmpty();
            }
            foreach(CharInfo c in BattleInfo.instance.cis){
                ccbts[i].SetDataShow(c);
                i++;
            }
            tci = BattleInfo.instance.cis;
        }
    }
    public void EnterToBattle(){
        SceneManager.LoadScene(BattleInfo.instance.scene_string);
    }
}