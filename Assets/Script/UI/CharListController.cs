using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharListController : MonoBehaviour 
{
    public RectTransform frame;
    public GameObject btnPrefab;
    private void Start(){
        LoadChar();
    }
    private void LoadChar(){
        float w = 100;
        int n = 0;
        foreach(CharInfo ci in CharData.instance.charDatas){
            GameObject btn = Instantiate(btnPrefab,frame);
            CharChooseBtn bui = btn.GetComponent<CharChooseBtn>();
            bui.SetData(ci);
            n++;
            if(n % 2 == 0){
                w += 207.27f;
            }
        }
        if(n % 2 != 0){
            w += 207.27f;
        }
        w += 100;
        frame.sizeDelta = new Vector2(w,frame.sizeDelta.y);
    }
    private void Update(){
        
    }
    public void ReturnToChooser(){
        SceneManager.LoadScene("Scenes/UI/CharChooser");
    }
}