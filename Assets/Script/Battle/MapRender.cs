using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapRender : MonoBehaviour 
{
    public LandType type;
    public bool can_place;
    [Header("显示组件")]
    public GameObject show_range;
    public GameObject show_place;
    private void Start(){
        
    }

    private void Update(){
        UpdateUI();
    }
    private void UpdateUI(){
        if(BattleController.instance.is_placing){
            show_place.SetActive(can_place && BattleController.instance.placingType == type);
            if(BattleController.instance.is_showing_range){
                //检测范围
                RaycastHit[] raycastHits = Physics.RaycastAll(transform.position+new Vector3(0,0,1),transform.forward*-1,100);
                Debug.DrawRay(transform.position+new Vector3(0,0,1),transform.forward*-1,Color.yellow);
                bool flag = false;
                foreach(RaycastHit hit in raycastHits){
                    if(hit.collider.tag == "attackRange" && hit.collider.transform.parent.parent.parent.GetComponent<Char>().is_placing){
                        flag = true;
                        break;
                    }
                }
                show_range.SetActive(flag);
            }else{
                show_range.SetActive(false);
            }
        }else{
            show_place.SetActive(false);
            show_range.SetActive(false);
        }
    }
}