using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
public class MapLoader : MonoBehaviour 
{
    private AstarPath astarPath;
    public static MapLoader mapLoader;
    public GameObject nodePrefab;
    public GameObject obstaclePrefab;
    public Transform rootTransform;
    public Transform obstacleTransform;
    public GameObject placePointPrefab;
    public Transform placePointsTransform;
    public Vector3 poffset;
    public Vector3 hpoffset;
    public string dataFile;
    private void Start() {
        mapLoader = this;
        astarPath = GetComponent<AstarPath>();
        LoadPath();
    }
    public void LoadPath(){
        JObject mapData = JObject.Parse(JObject.Parse(ReadData(Application.streamingAssetsPath+dataFile))["mapData"].ToString());
        int y = ((int)mapData["height"])-1;
        int x = 0;
        foreach(JArray row in mapData["map"]){
            foreach(int col in row){
                Vector3 position = new Vector3(x,y,0);
                int nodeIndex = col;
                JObject nodeData = JObject.Parse(mapData["tiles"][nodeIndex].ToString());
                if((int)nodeData["passableMask"] == 3){
                    GameObject n = Instantiate(nodePrefab,position,Quaternion.identity);
                    n.transform.parent = rootTransform;
                }else{
                    GameObject o = Instantiate(obstaclePrefab,position,Quaternion.identity);
                    o.transform.parent = obstacleTransform;
                }
                bool can_place = false;
                MapRender.LandType landType = MapRender.LandType.lowLand;
                Vector3 offset = new Vector3(0,0,0);
                switch((int)nodeData["buildableType"]){
                    case 0:
                        can_place = false;
                        if((int)nodeData["heightType"] == 0){
                            offset = poffset;
                        }else{
                            offset = hpoffset;
                        }
                        break;
                    case 1:
                        can_place = true;
                        offset = poffset;
                        landType = MapRender.LandType.lowLand;
                        break;
                    case 2:
                        can_place = true;
                        offset = hpoffset;
                        landType = MapRender.LandType.highLand;
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