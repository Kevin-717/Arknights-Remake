using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
partial class CharLoader : MonoBehaviour 
{
    private GameObject battleInfo;
    public GameObject btnPrefab;
    private RectTransform rectTransform;
    public Transform content;
    private GridLayoutGroup m_grid;
    private List<GameObject> btns = new List<GameObject>();
    private void Start() {
        battleInfo = GameObject.Find("levelInfo");
        m_grid = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
        foreach(CharData.Character c in CharData.instance.charList) {
            GameObject btn = Instantiate(btnPrefab);
            btn.GetComponent<Image>().sprite = c.charHalf;
            btn.GetComponent<RectTransform>().localScale = new Vector3(Screen.width/1920f,Screen.height/1080f,1);
            btn.GetComponent<BtnInfo>().cid = c.id;
            btn.transform.SetParent(content);
            if(battleInfo.GetComponent<BattleInfo>().charInds.IndexOf(btn.GetComponent<BtnInfo>().cid) >= 0){
                btn.GetComponent<BtnInfo>().OnClicked();
            }
            btns.Add(btn);
        }
    }
    public void close(){
        SceneManager.LoadScene("Scenes/CharChooser");
    }
    public void finish(){
        SceneManager.LoadScene("Scenes/CharChooser");
        int i = 0;
        battleInfo.GetComponent<BattleInfo>().charInds.Clear();
        while(battleInfo.GetComponent<BattleInfo>().charInds.Count < 13 && i < btns.Count){
            if(btns[i].GetComponent<BtnInfo>().is_selected && battleInfo.GetComponent<BattleInfo>().charInds.IndexOf(btns[i].GetComponent<BtnInfo>().cid) < 0){
                battleInfo.GetComponent<BattleInfo>().charInds.Add(btns[i].GetComponent<BtnInfo>().cid);
            }
            i++;
        }
    }
}