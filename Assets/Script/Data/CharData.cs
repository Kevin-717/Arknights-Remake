using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
public class CharData : MonoBehaviour 
{
    public List<CharInfo> charDatas;
    public static CharData instance;
    public string charDataJson;
    private JObject cdata;
    public List<CharProfession> cps;
    public List<Star_Sprite> sps;
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
        ParseCharData();
    }
    private void ParseCharData(){
        var e = cdata.GetEnumerator();
        while(e.MoveNext()){
            if(e.Current.Key.ToString().Split("_")[0] != "char"){
                continue;
            }
            string charPrefabName = "Char"+e.Current.Key.ToString().Split("_")[1];
            GameObject prefab = Resources.Load("Prefab/Battle/Char/"+charPrefabName) as GameObject;
            if(prefab == null){
                Debug.Log("Not Found prefab for -> " + e.Current.Key.ToString());
                continue;
            }else{
                Debug.Log("Found prefab for -> " + e.Current.Key.ToString());
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
            charDatas.Add(new CharInfo(){
                cid = int.Parse(e.Current.Key.ToString().Split("_")[1]),
                charType = e.Current.Value["position"].ToString() == "MELEE" ? CharType.LowLand : CharType.HighLand,
                charPrefab = prefab,
                cost = int.Parse(val["cost"].ToString()),
                max_hp = int.Parse(val["maxHp"].ToString()),
                def = int.Parse(val["def"].ToString()),
                atk = int.Parse(val["atk"].ToString()),
                mdef = int.Parse(val["magicResistance"].ToString()),
                replaceTime = int.Parse(val["respawnTime"].ToString()),
                name = e.Current.Value["name"].ToString(),
                name_en = e.Current.Value["appellation"].ToString(),
                cui = new CharUIData(){
                    icon_box = Resources.Load<Sprite>("CharHead/"+e.Current.Key.ToString()),
                    icon_half = Resources.Load<Sprite>("CharHalf/char_"+e.Current.Key.ToString().Split("_")[1]),
                    icon_type = pi,
                    sp = sps[int.Parse(e.Current.Value["rarity"].ToString().Split("_")[1])-1]
                }
            });
        }
    }
}