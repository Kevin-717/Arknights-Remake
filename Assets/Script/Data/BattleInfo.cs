using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleInfo : MonoBehaviour 
{
    public List<int> charIds;
    public LevelDescText ldt;
    public static BattleInfo instance;
    public string scene_string;
    public List<CharInfo> cis;
    private void Awake() {
        if(GameObject.FindGameObjectsWithTag("battleInfo").Length > 1){
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start(){
        instance = this;
    }

    private void Update(){
        
    }
}