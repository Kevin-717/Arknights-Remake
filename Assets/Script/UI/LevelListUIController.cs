using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelListUIController : MonoBehaviour 
{
    public Transform backgroundUI;
    public GameObject background2;
    public RectTransform content;
    public float scale = 0.2f;
    private bool flag = false;
    private void Start(){
        background2.GetComponent<Image>().DOFade(0,0);
    }

    private void Update(){
        
    }
    public void OnValueChanged(Vector2 pos){
        float movement = pos.x*content.rect.width * scale;
        if(pos.x >= 0.5){
            background2.GetComponent<Image>().DOFade(1,0.3f);
            backgroundUI.localPosition = new Vector3(-movement - 0.25f*1960,backgroundUI.localPosition.y,backgroundUI.localPosition.z);
            flag = true;
        }else{
            if(flag) background2.GetComponent<Image>().DOFade(0,0.3f);
            backgroundUI.localPosition = new Vector3(-movement - 0.5f*1960,backgroundUI.localPosition.y,backgroundUI.localPosition.z);
        }
    }
    public void EnterToChooser(){
        BattleInfo.instance.cis.Clear();
        SceneManager.LoadScene("Scenes/UI/CharChooser");
    }
}