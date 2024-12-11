using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class ChooserController : MonoBehaviour 
{
    public List<GameObject> btns;
    private GameObject battleInfo;
    public Sprite empty;
    public List<Sprite> char_images = new List<Sprite>();
    public GameObject loadingFrame;
    public GameObject level_text;
    public GameObject level_num;
    public GameObject hide_frame;
    private void Start() {
        battleInfo = GameObject.FindGameObjectWithTag("data");
        foreach(CharData.Character character in CharData.instance.charList){
            char_images.Add(character.charHalf);
        }
        foreach(GameObject btn in btns) {
            btn.GetComponent<Image>().sprite = empty;
        }
        int i = 0;
        foreach(int ind in battleInfo.GetComponent<BattleInfo>().charInds){
            btns[i].GetComponent<Image>().sprite = char_images[ind];
            i++;
        }
        level_num.GetComponent<Text>().DOFade(0,0.0f);
        level_text.GetComponent<Text>().DOFade(0,0.0f);
        loadingFrame.GetComponent<Image>().DOFade(0,0f);
        hide_frame.GetComponent<Image>().DOFade(0,0f);
    }
    public void EnterToList(){
        SceneManager.LoadScene("Scenes/CharList");
    }
    public void EnterToBattle(){
        loadingFrame.SetActive(true);
        loadingFrame.GetComponent<Image>().DOFade(1,1.5f);
        level_text.GetComponent<Text>().text = BattleInfo.instance.scene_name;
        level_text.GetComponent<Text>().DOFade(1,1.8f);
        level_num.GetComponent<Text>().text = BattleInfo.instance.scene_num;
        level_num.GetComponent<Text>().DOFade(1,1.8f);
        Invoke("j",2);
    }
    private void j(){
        hide_frame.SetActive(true);
        hide_frame.GetComponent<Image>().DOFade(1,1f).OnComplete(()=>{
            SceneManager.LoadScene(BattleInfo.instance.scene_ind);
        });
    }
}