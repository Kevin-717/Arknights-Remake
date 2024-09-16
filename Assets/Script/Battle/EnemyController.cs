using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Linq;
using Unity.VisualScripting;
using System.Threading.Tasks;
using System.Threading;
[System.Serializable]
public class EnemyPath{
    public Vector3 path;
    public PointType pointType;
    public float waitTime = 0;
}
[System.Serializable]
public class EnemyGroupData{
    public GameObject enemyPrefab;
    public Vector3 startPos;
    public List<EnemyPath> enemyPath = new List<EnemyPath>();
    public int repeat = 1;
    public float interval = 1;
    public float startTime;
    public bool created = false;
}
public class EnemyController : MonoBehaviour 
{
    public static EnemyController Instance;
    public string levelDataFile = "";
    public string enemyDataFile = "";
    private JObject levelData;
    private JObject enemyData;
    public List<EnemyGroupData> enemyDatas = new List<EnemyGroupData>();
    private float globalTimer = 0;

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
        InitEnemyData(levelData);
    }
    private void InitEnemyData(JObject levelData){
        JArray waves = JArray.Parse(levelData["waves"].ToString());
        JArray routes = JArray.Parse(levelData["routes"].ToString());
        float publicDelayTime = 0;
        foreach(var wave in waves){
            Debug.Log("Load Wave - "+wave.ToString());
            foreach(var fragment in wave["fragments"]){
                foreach(var enemy in fragment["actions"]){
                    try{
                        switch((int)enemy["actionType"]){
                            case 0:
                                string enemyName = enemy["key"].ToString();
                                string prefabName = "Enemy"+enemyName.Split("_")[1];
                                GameObject prefab = Resources.Load("Prefab/Battle/Enemy/"+prefabName) as GameObject;
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
                                    switch((int)point["type"]){
                                        case 0:
                                            //Move
                                            p = new Vector3((float)point["position"]["col"],(float)point["position"]["row"],0);
                                            enemyPath.path = p;
                                            enemyPath.pointType = PointType.normal;
                                            break;
                                        case 1: //WAIT_FOR_SECONDS
                                        case 2: //WAIT_FOR_PLAY_TIME
                                        case 3: //WAIT_CURRENT_FRAGMENT_TIME
                                        case 4: //WAIT_CURRENT_WAVE_TIME
                                            enemyPath.path = p;
                                            enemyPath.pointType = PointType.wait;
                                            enemyPath.waitTime = (float)point["time"];
                                            break;
                                        case 5:
                                            //	DISAPPEAR
                                            enemyPath.path = p;
                                            enemyPath.pointType = PointType.disappear;
                                            break;
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
                                enemyGroupData.interval = (float)enemy["interval"];
                                enemyGroupData.startTime = publicDelayTime + (float)enemy["preDelay"]+(float)fragment["preDelay"];
                                if(fragment["actions"][fragment["actions"].ToList().Count-1] == enemy){
                                    Debug.Log("wave last -> checkTime");
                                    publicDelayTime = publicDelayTime + (float)enemy["preDelay"]+(float)fragment["preDelay"];
                                }
                                enemyDatas.Add(enemyGroupData);
                                break;
                        }
                    }catch{
                        Debug.Log("Load ??? error");
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
        globalTimer += Time.deltaTime;
        foreach(EnemyGroupData enemyGroupData in enemyDatas){
            if(globalTimer > enemyGroupData.startTime && (!enemyGroupData.created)){
                GameObject g = Instantiate(Resources.Load("Prefab/Battle/CreateTemp") as GameObject);
                g.GetComponent<CreateEnemyTemp>().enemy = enemyGroupData;
                enemyGroupData.created = true;
                Debug.Log("Create Enemy");
            }
        }
    }
}