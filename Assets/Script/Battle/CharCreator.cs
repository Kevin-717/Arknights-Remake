using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharCreator : MonoBehaviour 
{
    public GameObject CharPrefab;
    public int cost;
    public int waitTime;
    public GameObject respawnFrame;
    public Image respawnCircle;
    public Text respawnCircleText;
    private float wait = 0;
    public bool is_machine = false;
    public Text machine_count;
    public int machine_c = 10;
    public enum charType
    {
        lowLand,
        highLand
    }
    public charType type;
    public Image charTypeIcon;
    public Text charCostText;
    public Image charImage;
    public string charName;
    private void Start() {

    }
    public void CreateChar(){
        if(wait == 0 && BattleController.Instance.cost >= cost && BattleController.Instance.is_place == false){
            BattleController.Instance.cost -= cost;
            if(!is_machine){
                GameObject c = Instantiate(CharPrefab);
                Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 m_MousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, pos.z);
                c.transform.position = Camera.main.ScreenToWorldPoint(m_MousePos);   
                c.GetComponent<Char>().createBtn = gameObject;
                gameObject.SetActive(false);
                BattleController.Instance.is_place = true;
                BattleController.Instance.is_lowland = type == charType.lowLand;
            }else{
                if(machine_c > 0){
                    machine_c--;
                }else{
                    return;
                }
                GameObject m = Instantiate(CharPrefab);
                Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 m_MousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, pos.z);
                m.transform.position = Camera.main.ScreenToWorldPoint(m_MousePos);   
                machine_count.text = "x"+machine_c.ToString();
                BattleController.Instance.is_place = true;
                BattleController.Instance.is_lowland = true;
                Respawn();
            }
        }
    }
    private void Update() {
        if(wait > 0){
            respawnFrame.SetActive(true);
            respawnCircle.fillAmount = (waitTime-wait)/waitTime;
            respawnCircleText.text = string.Format("{0:N1}",wait); 
            wait -= Time.deltaTime;
        }else{
            respawnFrame.SetActive(false);
            wait = 0;
        }
    }
    public void Respawn(){
        wait = waitTime;
        gameObject.SetActive(true);
    }
}