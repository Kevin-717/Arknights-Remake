using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using UnityEngine.UI;
public class EnemyPreview : MonoBehaviour
{
    public CanvasGroup frame;
    public static EnemyPreview instance;
    public RectTransform listFrame;
    public GameObject prevBtnPrefab;
    public Text group;
    public Text eName;
    public Text desc;
    public Image icon;
    public Text def;
    public Text mdef;
    public Text life;
    public Text atk;
    private List<GameObject> btns = new List<GameObject>();

    private void Start()
    {
        instance = this;
        frame.DOFade(0, 0);
        frame.gameObject.SetActive(false);
    }
    public void OnShowPrev()
    {
        frame.DOFade(1, 0.3f);
        frame.gameObject.SetActive(true);
    }
    public void OnHide()
    {
        frame.DOFade(0, 0.3f).OnComplete(() => {
            frame.gameObject.SetActive(false);
        });
    }
    public void LoadEnemyData(string path)
    {
        foreach(GameObject btn in btns)
        {
            Destroy(btn.gameObject);
        }
        btns.Clear();
        Debug.Log("Load -> " + path);
        JArray enemyData = JArray.Parse(JObject.Parse(ReadData(Application.streamingAssetsPath + path))["enemyDbRefs"].ToString());
        float h = 50+158;
        int i = 0;
        foreach (JObject enemy in enemyData)
        {
            foreach(EnemyData ed in CharData.instance.eds)
            {
                if(ed.key == enemy["id"].ToString())
                {
                    GameObject btn = Instantiate(prevBtnPrefab, listFrame);
                    btn.GetComponent<Image>().sprite = ed.icon;
                    btn.GetComponent<EnemyPreviewBtn>().ed = ed;
                    i++;
                    if(i % 4 == 0)
                    {
                        h += 45 + 158;
                    }
                    btns.Add(btn);
                }
            }
            h += 50;
            listFrame.sizeDelta = new Vector2(listFrame.sizeDelta.x, h);
        }
    }
    public string ReadData(string path)
    {
        string readData;
        string fileUrl = path;
        using (StreamReader sr = File.OpenText(fileUrl))
        {
            readData = sr.ReadToEnd();
            sr.Close();
        }
        return readData;
    }
    public void ShowEnemy(EnemyData e)
    {
        group.text = e.group;
        eName.text = e.name;
        desc.text = e.introduction;
        atk.text = e.atk;
        def.text = e.def;
        mdef.text = e.mdef;
        life.text = e.life;
        icon.sprite = e.icon;
    }
}