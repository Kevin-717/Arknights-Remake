using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.InputField;

public class Box : MonoBehaviour
{
    private bool placing = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (placing)
        {
            if(SetPosition() && Input.GetMouseButtonDown(0))
            {
                placing = false;
                BattleController.instance.is_showing_range = false;
                BattleController.instance.is_placing = false;
                GameObject.FindGameObjectWithTag("path").GetComponent<AstarPath>().Scan();
                foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<Enemy>().UpdatePath();
                }
            }
        }
    }
    private bool SetPosition()
    {
        bool canPut = false;
        Vector3 mp = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mp);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray, 100);
        foreach (RaycastHit hit in raycastHits)
        {
            if (hit.collider.tag != "mapRender") continue;
            if (!hit.collider.gameObject.GetComponent<MapRender>().can_place) continue;
            if (hit.collider.gameObject.GetComponent<MapRender>().type == LandType.lowLand)
            {
                transform.position = hit.collider.transform.position;
                canPut = true;
                break;
            }
        }
        if (!canPut)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 m_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, pos.z);
            Vector3 wp = Camera.main.ScreenToWorldPoint(m_MousePos);
            transform.position = new Vector3(wp.x, wp.y, -0.01f);
        }
        return canPut;
    }
}
