using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PreviewLine: MonoBehaviour 
{
    [HideInInspector]
    public List<GameObject> move_line;
    public int move_index = 0;
    public float speed = 8;
    private AIPath aIPath;
    private AIDestinationSetter aIDestinationSetter;
    private TrailRenderer trailRenderer;
    private bool isMove = true;
    void Start(){
        aIPath = GetComponent<AIPath>();
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        aIPath.maxSpeed = speed;
        aIDestinationSetter.target = move_line[move_index].transform;
        trailRenderer = GetComponent<TrailRenderer>();
    }
    private void tp(){
        trailRenderer.enabled = false;
        move_index++;
        transform.position = move_line[move_index].transform.position;
        move_index++;
        trailRenderer.enabled = true;
        isMove = true;
    }
    private void Move(){
        if(move_index == move_line.Count){
            Destroy(gameObject);
            return;
        }
        if(move_line[move_index].GetComponent<PointInfo>().isWait == PointType.hole){
            isMove = false;
            Invoke("tp",0.4f);
            return;
        }
        if(Vector3.Distance(move_line[move_index].transform.position,transform.position) <= 0.6f){
            aIPath.maxSpeed = 0;
            move_index++;
        }else{
            aIPath.maxSpeed = speed;
            aIDestinationSetter.target = move_line[move_index].transform;
        }
    }
    void FixedUpdate(){
        if(!isMove) return;
        Move();
    }
}