using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
public class MapController : MonoBehaviour 
{
    private AstarPath astarPath;
    public static MapController mapLoader;
    [Header("节点预制体")]
    public GameObject nodePrefab;
    [Header("障碍物预制体")]
    public GameObject obstaclePrefab;
    [Header("寻路父节点Transform")]
    public Transform rootTransform;
    [Header("障碍物父节点Transform")]
    public Transform obstacleTransform;
    [Header("部署点Prefab")]
    public GameObject placePointPrefab;
    [Header("部署点父节点Transform")]
    public Transform placePointsTransform;
    [Header("地面部署点偏移")]
    public Vector3 poffset;
    [Header("高台部署点偏移")]
    public Vector3 hpoffset;
    public string dataFile;
    private void Start() {
        mapLoader = this;
        astarPath = GetComponent<AstarPath>();
        LoadPath();
    }
    public void LoadPath(){
        JObject mapData = JObject.Parse(JObject.Parse(ReadData(Application.streamingAssetsPath+dataFile))["mapData"].ToString());
        int y = JArray.Parse(mapData["map"].ToString()).Count-1;
        int x = 0;
        foreach(JArray row in mapData["map"]){
            foreach(int col in row){
                Vector3 position = new Vector3(x,y,0);
                int nodeIndex = col;
                JObject nodeData = JObject.Parse(mapData["tiles"][nodeIndex].ToString());
                if(nodeData["passableMask"].ToString() == "ALL")
                {
                    GameObject n = Instantiate(nodePrefab,position,Quaternion.identity);
                    n.transform.parent = rootTransform;
                }else{
                    GameObject o = Instantiate(obstaclePrefab,position,Quaternion.identity);
                    o.transform.parent = obstacleTransform;
                }
                bool can_place = false;
                LandType landType = LandType.lowLand;
                Vector3 offset = new Vector3(0,0,0);
                switch(nodeData["buildableType"].ToString()){
                    case "None":
                        can_place = false;
                        if(nodeData["heightType"].ToString() == "HIGHLAND")
                        {
                            offset = poffset;
                        }else{
                            offset = hpoffset;
                        }
                        break;
                    case "MELEE":
                        can_place = true;
                        offset = poffset;
                        landType = LandType.lowLand;
                        break;
                    case "RANGED":
                        can_place = true;
                        offset = hpoffset;
                        landType = LandType.highLand;
                        break;
                }
                GameObject p = Instantiate(placePointPrefab,position+offset,Quaternion.identity);
                p.transform.parent = placePointsTransform;
                p.GetComponent<MapRender>().can_place = can_place;
                p.GetComponent<MapRender>().type = landType;
                x++;
            }
            x = 0;
            y--;
        }
        astarPath.Scan();
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
}