using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BattleInfo : MonoBehaviour 
{
    public string scene_ind;
    public string scene_name;
    public string scene_num;
    public List<int> charInds = new List<int>();
    public static BattleInfo instance;
    private void Start() {
        instance = this;
    }
}