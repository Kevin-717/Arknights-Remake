using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class NodeData : MonoBehaviour 
{
    public Transform connect_frame;
    public Transform connect_frame_before;
    public MapCreator.Node.NodeType nodeType;
    public string levelName = "";
    public string levelDesc = "";
    public string levelSence = "";
    public void OnClick(){
        UIManager.instance.ShowBattleEntrance(levelName,levelDesc,levelSence);
        BattleInfo.instance.scene_ind = levelSence;
    }
}