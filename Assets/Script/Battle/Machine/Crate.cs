using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Crate : MonoBehaviour 
{
    private bool is_placing = true;
    private bool can_put = false;
    private void Update() {
        if (is_placing){
            if(!can_put){
                Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 m_MousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, pos.z);
                Vector3 wp = Camera.main.ScreenToWorldPoint(m_MousePos);
                transform.position = new Vector3(wp.x,wp.y,0);
            }else{
                if(Input.GetMouseButtonDown(0)){
                    is_placing = false;
                    AstarInstance.instance.GetComponent<AstarPath>().Scan();
                    BattleController.Instance.is_place = false;
                }
            }
            RaycastHit[] hitInfos;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hitInfos = Physics.RaycastAll(ray,1000);
            if(hitInfos.Length==0){can_put = false;return;}
            foreach(RaycastHit hitInfo in hitInfos){
                if(hitInfo.collider.gameObject.tag == "lowland" && hitInfo.collider.gameObject.GetComponent<MapRender>().can_place){
                    can_put = true;
                    transform.position = hitInfo.collider.gameObject.transform.position;
                    return;
                }
            }
        }
    }
}