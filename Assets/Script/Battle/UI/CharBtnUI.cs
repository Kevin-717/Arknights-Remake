using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharBtnUI : MonoBehaviour 
{
    [Header("角色按钮组件")]
    [Header("头像")]
    public Image head_image;
    [Header("职业")]
    public Image type_image;
    [Header("费用")]
    public Text cost_text;
    public CharInfo ci;
    private void Start(){
        SetUI();
    }
    public void SetUI(){
        head_image.sprite = ci.cui.icon_box;
        type_image.sprite = ci.cui.icon_type;
        cost_text.text = ci.cost.ToString();
    }

    private void Update(){
        
    }
    public void OnClick(){
        if(BattleController.instance.cost >= ci.cost) { 
            GameObject c = Instantiate(ci.charPrefab);
            Char ch = c.GetComponent<Char>();
            ch.def = ci.def;
            ch.damage.damage = ci.atk;
            ch.mdef = ci.mdef;
            ch.hp = ci.max_hp;
            ch.hp_total = ci.max_hp;
            ch.obs_num = ci.def_num;
            BattleController.instance.is_placing = true;
            BattleController.instance.placingType = ci.charPrefab.GetComponent<Char>().landType;
            gameObject.SetActive(false);
        }
    }
}