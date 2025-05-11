using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class EnemyController : MonoBehaviour 
{
    public static EnemyController Instance;
    public string levelDataFile = "";
    public string enemyDataFile = "";
    public int enemyNum = 0;
    private JObject levelData;
    private JObject enemyData;
    public List<EnemyGroupData> enemyDatas = new List<EnemyGroupData>();
    private float globalTimer = 0;
    public GameObject toastFrame;
    public Image toastIcon;
    public Text toastTitle;
    public Text toastDescription;
    public bool is_toasting = false;
    public bool is_rogue_like = false;
    public bool flag = false;
    private void Start() {
        Instance = this;
        levelDataFile = Application.streamingAssetsPath+levelDataFile;
        enemyDataFile = Application.streamingAssetsPath+enemyDataFile;
        string data = ReadData(levelDataFile);
        levelData = JObject.Parse(data);
        Debug.Log("LoadFile -> "+levelDataFile);
        data = ReadData(enemyDataFile);
        enemyData = JObject.Parse(data);
        Debug.Log("LoadFile -> "+enemyDataFile);
        Debug.Log("Waves -> " + levelData["waves"].ToList().Count);
        InitEnemyData(levelData,enemyData);
    }
    private void InitEnemyData(JObject levelData,JObject enemyData){
        JArray waves = JArray.Parse(levelData["waves"].ToString());
        JArray routes = JArray.Parse(levelData["routes"].ToString());
        JArray enemiesData = JArray.Parse(enemyData["enemies"].ToString());
        float publicDelayTime = 0;
        foreach(var wave in waves){
            Debug.Log("Load Wave - "+wave.ToString());
            foreach(var fragment in wave["fragments"]){
                Debug.Log("Load fragment -> " + fragment.ToString());
                foreach(var enemy in fragment["actions"]){
                    Debug.Log("Load Enemy ->" + enemy.ToString());
                    try{
                        object at = (is_rogue_like||flag) ? enemy["actionType"].ToString() : (int)enemy["actionType"];
                        switch(at){
                            case "ACTIVATE_PREDEFINED":
                            case "SPAWN":
                            case 0:
                                if(enemy["key"].ToString() == "") continue;
                                string enemyName = enemy["key"].ToString();
                                string prefabName = "Enemy"+enemyName.Split("_")[1];
                                GameObject prefab = Resources.Load("Prefab/Battle/Enemy/"+prefabName) as GameObject;
                                Debug.Log("Load Prefab "+"Prefab/Battle/Enemy/"+prefabName + " -> " + prefab);
                                if(enemyName.Split("_").Length > 3){
                                    string level = enemyName.Split("_")[3];
                                    prefab = Resources.Load("Prefab/Battle/Enemy/"+prefabName+'_'+level) as GameObject;
                                }
                                if(prefab == null){
                                    Debug.Log("cannot find prefab " + prefabName);
                                    continue;
                                }
                                int route = (int)enemy["routeIndex"];
                                JObject routeData;
                                try{
                                    routeData = JObject.Parse(routes[route].ToString()); 
                                }catch{
                                    Debug.Log("cannot parse json ->");
                                    Debug.Log(routes[route].ToString());
                                    continue;
                                }
                                if(routeData.Property("startPosition") == null){
                                    Debug.Log(route.ToString() + " route error");
                                    continue;
                                }     
                                EnemyGroupData enemyGroupData = new EnemyGroupData();
                                enemyGroupData.enemyPrefab = prefab;
                                Vector3 pos = new Vector3((float)routeData["startPosition"]["col"],(float)routeData["startPosition"]["row"],0);
                                enemyGroupData.startPos = pos;
                                Vector3 p = pos;
                                foreach(var point in routeData["checkpoints"].ToList()){
                                    EnemyPath enemyPath = new EnemyPath();
                                    object pt = (is_rogue_like || flag) ? point["type"].ToString() : (int)point["type"];
                                    switch(pt){
                                        case "MOVE":
                                        case 0:
                                            //Move
                                            p = new Vector3((float)point["position"]["col"],(float)point["position"]["row"],0);
                                            Vector3 offset = new Vector3((float)point["reachOffset"]["x"],(float)point["reachOffset"]["y"],0);
                                            p += offset;
                                            enemyPath.path = p;
                                            enemyPath.pointType = PointType.normal;
                                            break;
                                        case 1: //WAIT_FOR_SECONDS
                                        case "WAIT_FOR_SECONDS":
                                        case 2: //WAIT_FOR_PLAY_TIME
                                        case "WAIT_FOR_PLAY_TIME":
                                        case 3: //WAIT_CURRENT_FRAGMENT_TIME
                                        case "WAIT_CURRENT_FRAGMENT_TIME":
                                        case 4: //WAIT_CURRENT_WAVE_TIME
                                        case "WAIT_CURRENT_WAVE_TIME":
                                            enemyPath.path = p;
                                            enemyPath.pointType = PointType.wait;
                                            enemyPath.waitTime = (float)point["time"];
                                            break;
                                        case "DISAPPEAR":
                                        case 5:
                                            //	DISAPPEAR
                                            enemyPath.path = p;
                                            enemyPath.pointType = PointType.disappear;
                                            break;
                                        case "APPEAR_AT_POS":
                                        case 6:
                                            // APPEAR AT POS
                                            p = new Vector3((float)point["position"]["col"],(float)point["position"]["row"],0);
                                            enemyPath.path = p;
                                            enemyPath.pointType = PointType.appear;
                                            break;
                                    }
                                    enemyGroupData.enemyPath.Add(enemyPath);
                                }
                                EnemyPath endp = new EnemyPath();
                                endp.path = new Vector3((float)routeData["endPosition"]["col"],(float)routeData["endPosition"]["row"],0);
                                enemyGroupData.enemyPath.Add(endp);
                                enemyGroupData.repeat = (int)enemy["count"];
                                enemyNum+=(int)enemy["count"];
                                enemyGroupData.interval = (float)enemy["interval"];
                                enemyGroupData.startTime = publicDelayTime + (float)enemy["preDelay"]+(float)fragment["preDelay"];
                                if(fragment["actions"][fragment["actions"].ToList().Count-1] == enemy){
                                    Debug.Log("wave last -> checkTime");
                                    publicDelayTime = publicDelayTime + (float)enemy["preDelay"]+(float)fragment["preDelay"];
                                }
                                enemyGroupData.type = 0;
                                enemyDatas.Add(enemyGroupData);
                                break;
                            case 1:
                                //Preview Cursor
                                break;
                            case 2:
                                //STORY
                                break;
                            case 3:
                                //???
                                break;
                            case 4:
                                //PLAY_BGM
                                break;
                            case 5:
                                //DISPLAY ENEMY INFO
                                EnemyGroupData enemyInfoData = new EnemyGroupData();
                                enemyInfoData.type = 5;
                                string key = enemy["key"].ToString();
                                foreach(var e in enemiesData){
                                    if(e["Key"].ToString() == key){
                                        string name = e["Value"][0]["enemyData"]["name"]["m_value"].ToString();
                                        string description = e["Value"][0]["enemyData"]["description"]["m_value"].ToString();
                                        enemyInfoData.name = name;
                                        enemyInfoData.startTime = publicDelayTime + (float)enemy["preDelay"]+(float)fragment["preDelay"];
                                        enemyInfoData.description = description.Replace("</>","</color>").Replace("<@eb.danger>","<color=red>").Replace("<@eb.key>","<color=\"#00F6FF\">");
                                        enemyInfoData.icon = Resources.Load<Sprite>("enemies_icon/"+key);
                                    }
                                }
                                enemyDatas.Add(enemyInfoData);
                                continue;
                        }
                    }catch(Exception e){
                        Debug.Log("Load ??? error -> " + e.Source + "\n" + e.Message);
                    }
                }
            }
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
    private void Update() {
        if (BattleController.instance.isRunning == false) Destroy(gameObject);
        globalTimer += Time.deltaTime;
        foreach(EnemyGroupData enemyGroupData in enemyDatas){
            if(globalTimer > enemyGroupData.startTime && (!enemyGroupData.created)){
                switch(enemyGroupData.type){
                    case 0:
                        GameObject g = Instantiate(Resources.Load("Prefab/Battle/CreateTemp") as GameObject);
                        g.GetComponent<CreateEnemyTemp>().enemy = enemyGroupData;
                        break;
                    case 5:
                        //DISPLAY ENEMY INFO
                        if(is_toasting){
                            enemyGroupData.startTime += 3.6f;
                            return;
                        }
                        is_toasting = true;
                        toastFrame.SetActive(true);
                        toastIcon.sprite = enemyGroupData.icon;
                        toastTitle.text = enemyGroupData.name;
                        toastDescription.text = enemyGroupData.description;
                        toastFrame.GetComponent<RectTransform>().DOLocalMoveX(714f,0.8f).OnComplete(()=>{
                            Invoke("HideToast",2f);
                        });
                        break;
                }
                enemyGroupData.created = true;
                Debug.Log("Do Enemy Action");
            }
        }
    }
    private void HideToast(){
        toastFrame.GetComponent<RectTransform>().DOLocalMoveX(1210f,0.8f).OnComplete(()=>{
            toastFrame.SetActive(false);
            is_toasting = false;
        });
    }
}