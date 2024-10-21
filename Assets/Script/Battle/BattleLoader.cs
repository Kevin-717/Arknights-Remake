using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class BattleLoader : MonoBehaviour 
{
    public GameObject btnPrefab;
    private void Start() {
        List<CharData.Character> cs = new List<CharData.Character>();
        foreach(CharData.Character c in CharData.instance.charList){
            foreach(int i in BattleInfo.instance.charInds){
                if(i == c.id){
                    cs.Add(c);
                }
            }
        }
        foreach(CharData.Character character in cs){
            GameObject btn = Instantiate(btnPrefab);
            btn.GetComponent<RectTransform>().localScale = new Vector3(Screen.width/1920f,Screen.height/1080f,1);
            CharCreator ctr =  btn.GetComponent<CharCreator>();
            if(character.ct == CharData.Character.CharType.lowLand){
                ctr.type = CharCreator.charType.lowLand;
            }else{
                ctr.type = CharCreator.charType.highLand;
            }
            ctr.charName = character.charName;
            ctr.CharPrefab = character.charPrefab;
            ctr.cost = character.cost;
            ctr.waitTime = (int)character.waitTime;
            ctr.charImage.sprite = character.image;
            ctr.charCostText.text = character.cost.ToString();
            ctr.charTypeIcon.sprite = character.charIcon;
            ctr.transform.SetParent(transform);
        }
    }
}