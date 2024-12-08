using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class RogueLikeBtn : MonoBehaviour 
{
    public Image raity;
    public Image charIcon;
    public Text charName;
    public int charId;
    public GameObject mask;
    private bool flag = false;
    public void OnSelect(){
        flag = !flag;
        mask.SetActive(flag);
        if(flag){
            BattleInfo.instance.charInds.Add(charId);
        }else{
            BattleInfo.instance.charInds.Remove(charId);
        }
    }
}