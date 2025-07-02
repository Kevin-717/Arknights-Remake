using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CharData : MonoBehaviour 
{
    public List<CharInfo> charDatas;
    public static CharData instance;
    public string charDataJson;
    private JObject cdata;
    public List<CharProfession> cps;
    public List<Star_Sprite> sps;
    public string enemyDataPath = "/levels/enemydata/enemy_database.json";
    public string handBookPath = "/enemy_handbook_table.json";
    public List<EnemyData> eds;
    public JObject enemyHandbook;
    private JArray enemies;
    private int total;
    private int current;
    public List<Transform> progressBars;
    public CanvasGroup btn;
    public RectTransform ball;
    public CanvasGroup info;
    public Text prog;
    public Image black;
    private bool isLoading = true;
    private void Awake() {
        if(GameObject.FindGameObjectsWithTag("battleInfo").Length > 1){
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
        }
    }
    public string ReadData(string path){
        string readData;
        string fileUrl = path;
        using (StreamReader sr = File.OpenText(fileUrl)){
            readData = sr.ReadToEnd();
            sr.Close();
        }
        return readData;
    }
    private void Start() {
        instance = this;
        cdata = JObject.Parse(ReadData(Application.streamingAssetsPath+charDataJson));
        enemies = JArray.Parse(JObject.Parse(ReadData(Application.streamingAssetsPath + enemyDataPath))["enemies"].ToString());
        //ParseCharData();
        //ParseEnemyData()
        total = cdata.Count + enemies.Count;
        info.DOFade(0, 0);
        black.DOFade(0, 0);
        Debug.Log(total);
    }
    public void StartInit()
    {
        ball.DOLocalMoveY(179.73f, 0.6f);
        btn.DOFade(0, 0.8f).OnComplete(() =>
        {
            info.gameObject.SetActive(true);
            info.DOFade(1, 0.5f).OnComplete(() =>
            {
                StartCoroutine(ParseCharData());
                StartCoroutine(ParseEnemyData());
            });
        });
    }
    private void Update()
    {
        if(current == total && isLoading)
        {
            isLoading = false;
            black.gameObject.SetActive(true);
            black.DOFade(1, 0.8f).OnComplete(() =>
            {
                SceneManager.LoadScene("Scenes/UI/LevelList0");
            });
        }
    }
    private void UpdateScale()
    {
        float scale = (float)current / (float)total;
        progressBars[0].localScale = new Vector3(scale, 1, 1);
        progressBars[1].localScale = new Vector3(scale, 1, 1);
        prog.text = (Mathf.RoundToInt(((float)current/total)*100)).ToString() + "%";
    }
    private IEnumerator ParseCharData(){
        var e = cdata.GetEnumerator();
        while(e.MoveNext()){
            if(e.Current.Key.ToString().Split("_")[0] != "char"){
                current++;
                UpdateScale();
                continue;
            }
            string charPrefabName = "Char"+e.Current.Key.ToString().Split("_")[1];
            GameObject prefab = Resources.Load("Prefab/Battle/Char/"+charPrefabName) as GameObject;
            if(prefab == null){
                //Debug.Log("Not Found prefab for -> " + e.Current.Key.ToString());
                current++;
                UpdateScale();
                continue;
            }else{
                //Debug.Log("Found prefab for -> " + e.Current.Key.ToString());
            }
            int l = e.Current.Value["phases"].ToList().Count;
            var attr = e.Current.Value["phases"][l-1]["attributesKeyFrames"].ToList();
            JObject val = JObject.Parse(attr[attr.Count-1]["data"].ToString());
            string profession = e.Current.Value["profession"].ToString();
            Sprite pi = null;
            foreach(CharProfession cp in cps){
                if(cp.profession == profession){
                    pi = cp.icon;
                }
            }
            charDatas.Add(new CharInfo() {
                cid = int.Parse(e.Current.Key.ToString().Split("_")[1]),
                charType = e.Current.Value["position"].ToString() == "MELEE" ? CharType.LowLand : CharType.HighLand,
                charPrefab = prefab,
                cost = int.Parse(val["cost"].ToString()),
                max_hp = int.Parse(val["maxHp"].ToString()),
                def = int.Parse(val["def"].ToString()),
                atk = int.Parse(val["atk"].ToString()),
                mdef = int.Parse(val["magicResistance"].ToString()),
                replaceTime = int.Parse(val["respawnTime"].ToString()),
                def_num = int.Parse(val["blockCnt"].ToString()),
                name = e.Current.Value["name"].ToString(),
                name_en = e.Current.Value["appellation"].ToString(),
                cui = new CharUIData(){
                    icon_box = Resources.Load<Sprite>("CharHead/"+e.Current.Key.ToString()),
                    icon_half = Resources.Load<Sprite>("CharHalf/char_"+e.Current.Key.ToString().Split("_")[1]),
                    icon_type = pi,
                    sp = sps[int.Parse(e.Current.Value["rarity"].ToString().Split("_")[1])-1]
                }
            });
            current++;
            UpdateScale();
            yield return null;
        }
    }
    public string getLevel(string type,float value)
    {
        JArray infoList = JArray.Parse(enemyHandbook["levelInfoList"].ToString());
        foreach (JObject obj in infoList)
        {
            if (float.Parse(obj[type]["min"].ToString()) <= value && float.Parse(obj[type]["max"].ToString()) >= value)
            {
                return obj["classLevel"].ToString();
            }
        }
        return "";
    }
    private IEnumerator ParseEnemyData()
    {
        enemyHandbook = JObject.Parse(ReadData(Application.streamingAssetsPath + handBookPath));
        foreach (JObject enemy in enemies) { 
            //Debug.Log("enemy -> " + enemy["Key"].ToString());
            try
            {
                EnemyData ed = new EnemyData()
                {
                    key = enemy["Key"].ToString(),
                    icon = Resources.Load<Sprite>("enemies_icon/" + enemy["Key"].ToString()),
                    name = enemy["Value"][0]["enemyData"]["name"]["m_value"].ToString(),
                    introduction = enemyHandbook["enemyData"][enemy["Key"].ToString()]["description"].ToString(),
                    atk = getLevel("attack", float.Parse(enemy["Value"][0]["enemyData"]["attributes"]["atk"]["m_value"].ToString())),
                    life = getLevel("maxHP", float.Parse(enemy["Value"][0]["enemyData"]["attributes"]["maxHp"]["m_value"].ToString())),
                    mdef = getLevel("magicRes", float.Parse(enemy["Value"][0]["enemyData"]["attributes"]["magicResistance"]["m_value"].ToString())),
                    def = getLevel("def", float.Parse(enemy["Value"][0]["enemyData"]["attributes"]["def"]["m_value"].ToString())),
                    group = enemyHandbook["enemyData"][enemy["Key"].ToString()]["enemyIndex"].ToString(),

                };
                eds.Add(ed);
            }
            catch
            {
                Debug.Log("Error");
            }
            current++;
            UpdateScale();
            yield return null;
        }
    }
}