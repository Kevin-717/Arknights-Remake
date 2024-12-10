using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MapCreator : MonoBehaviour 
{
    public static MapCreator instance;
    [System.Serializable]
    public class Node
    {
        public enum NodeType {
            Battle,
            Store
        }
        [System.Serializable]
        public class NodeCol{
            [Header("单列节点")]
            public List<NodeType> nodesCol = new List<NodeType>();
        }
        [Header("单层节点")]
        public List<NodeCol> nodes = new List<NodeCol>();
        [System.Serializable]
        public class LevelDesc{
            [Header("关卡名")]
            public string name;
            [Header("关卡介绍")]
            public string description;
            public string sence;
        }
        [Header("战斗节点信息")]
        public List<LevelDesc> levelDescs = new List<LevelDesc>();
    }
    public List<Node> nodeLevels = new List<Node>();
    public class Map
    {
        public class NodeInfo
        {
            public Node.NodeType nodeType;
            public string levelSence;
            public string levelName;
            public string levelDesc;
            public List<int> connectIndex = new List<int>();
            public GameObject instance;
        }
        public List<List<NodeInfo>> nodes = new List<List<NodeInfo>>();
    }
    public List<Map> maps = new List<Map>();
    public Transform node_frame;
    public GameObject battleNodePrefab;
    public GameObject colPrefab;
    public GameObject linePrefab;
    public GameObject line2Prefab;
    public GameObject pointPrefab;
    public Material mat;
    private void CreateNode(){
        int level=0,r=0,c=0;
        foreach(Node nodeLevel in nodeLevels){
            Map map = new Map();
            List<Node.LevelDesc> levelDescs = new List<Node.LevelDesc>(nodeLevel.levelDescs);
            foreach(Node.NodeCol col in nodeLevel.nodes){
                List<Map.NodeInfo> nc = new List<Map.NodeInfo>();
                foreach(Node.NodeType nodeType in col.nodesCol){
                    Map.NodeInfo nodeInfo = new Map.NodeInfo();
                    nodeInfo.nodeType = nodeType;
                    switch(nodeType){
                        case Node.NodeType.Store://???
                        case Node.NodeType.Battle:
                            Node.LevelDesc l;
                            if(levelDescs.Count == 0){
                                Debug.Log(nodeLevel.levelDescs.Count);
                                l = nodeLevel.levelDescs[Random.Range(0,nodeLevel.levelDescs.Count)];
                            }else{
                                l = levelDescs[Random.Range(0,levelDescs.Count)];
                            }
                            levelDescs.Remove(l);
                            nodeInfo.levelSence = l.sence;
                            nodeInfo.levelName = l.name;
                            nodeInfo.levelDesc = l.description;
                            break;
                    }
                    nc.Add(nodeInfo);
                    r++;
                }
                map.nodes.Add(nc);
                c++;
            }
            maps.Add(map);
            level++;
        }
        int li=0;
        foreach(Map map in maps){
            int cl = 0;
            foreach(List<Map.NodeInfo> col in map.nodes){
                if(col != map.nodes[map.nodes.Count-1]){
                    if(col.Count == 1){
                        for(int t=0;t<maps[li].nodes[cl+1].Count;t++){
                            col[0].connectIndex.Add(t);
                        }
                    }else{
                        if(maps[li].nodes[cl+1].Count < col.Count){
                            int last = 0;
                            for(int t=0;t<maps[li].nodes[cl+1].Count;t++){
                                col[t].connectIndex.Add(t);
                                last++;
                            }
                            while(last < col.Count){
                                col[last].connectIndex.Add(Random.Range(0,maps[li].nodes[cl+1].Count));
                                last++;
                            }
                        }else if(maps[li].nodes[cl+1].Count == col.Count){
                            for(int t=0;t<maps[li].nodes[cl+1].Count;t++){
                                col[t].connectIndex.Add(t);
                            }
                        }else{
                            int last = 0;
                            for(int t=0;t<col.Count;t++){
                                col[t].connectIndex.Add(t);
                                last++;
                            }
                            while(last<maps[li].nodes[cl+1].Count){
                                col[Random.Range(0,col.Count)].connectIndex.Add(last);
                                last++;
                            }
                        }
                    }
                }
                cl++;
            }
            li++;
        }
    }
    private void DrawNode(Map m){
        Map map = m;
        Debug.Log(map);
        int x = 500;
        foreach(List<Map.NodeInfo> col in map.nodes){
            GameObject c = Instantiate(colPrefab,node_frame);
            c.transform.localPosition = new Vector3(x,-500,0);
            foreach(Map.NodeInfo row in col){
                GameObject n;
                            Debug.Log("drawing");
                switch(row.nodeType){
                    case Node.NodeType.Store:
                    case Node.NodeType.Battle:
                        n  = Instantiate(battleNodePrefab,c.transform);
                        n.name = "BattleNode-"+Random.Range(10000,99999).ToString();
                        n.GetComponent<NodeData>().nodeType = Node.NodeType.Battle;
                        n.GetComponent<NodeData>().levelName = row.levelName;
                        n.GetComponent<NodeData>().levelDesc = row.levelDesc;
                        n.GetComponent<NodeData>().levelSence = row.levelSence;
                        Debug.Log(n);
                        row.instance = n;
                        break;
                }
            }
            x+=800;
        }
        int i=0;
        foreach(List<Map.NodeInfo> col in map.nodes){
            foreach(Map.NodeInfo row in col){
                foreach(int ind in row.connectIndex){
                    GameObject line = Instantiate(linePrefab,row.instance.GetComponent<NodeData>().connect_frame);
                    GameObject lineEnd = Instantiate(line2Prefab,map.nodes[i+1][ind].instance.GetComponent<NodeData>().connect_frame_before);
                    GameObject t1t = Instantiate(pointPrefab,line.transform);
                    GameObject t2t = Instantiate(pointPrefab,line.transform);
                    GameObject t3t = Instantiate(pointPrefab,lineEnd.transform);
                    GameObject t4t = Instantiate(pointPrefab,lineEnd.transform);
                    t1t.transform.localPosition = new Vector3(0,0,0);
                    t2t.transform.localPosition = new Vector3(60,0,0);
                    t3t.transform.localPosition = new Vector3(-60,0,0);
                    t4t.transform.localPosition = new Vector3(-10,0,0);
                    Curve curve = line.GetComponent<Curve>();
                    curve.posList[0] = t1t.transform;
                    curve.posList[1] = t2t.transform;
                    curve.posList[2] = t3t.transform;
                    curve.posList[3] = t4t.transform;
                }
            }
            i++;
        }
    }
    private void Start() {
        instance = this;
        CreateNode();
        DrawNode(maps[0]);
    }
    public void ReDraw(){
        SceneManager.LoadScene("Scenes/Roguelike/RogueLevel");
        StartCoroutine(redraw());
    }
    IEnumerator redraw(){
        yield return null;
        Debug.Log(GameObject.FindGameObjectWithTag("RogueLikeFrame"));
        node_frame = GameObject.FindGameObjectWithTag("RogueLikeFrame").transform;
        Debug.Log(maps[0].nodes.Count);
        DrawNode(maps[0]);
    }
    public void Leave(){
        Destroy(RogueLikeData.Instance.gameObject);
        Destroy(gameObject);
        SceneManager.LoadScene("Scenes/Main");
    }
}