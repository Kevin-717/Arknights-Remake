using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class CharDetail : MonoBehaviour 
{
    public static CharDetail Instance;
    public Image charTypeIconImage;
    public Text charNameText;
    public Text charAtkText;
    public Text charPDefText;
    public Text charADefText;
    public Text charLifeText;
    [HideInInspector]
    public Char showCharObj;
    public GameObject frame;
    private void Start() {
        Instance = this;
    }
    public void Show(Char charObj){
        frame.SetActive(true);
        showCharObj = charObj;
    }
    public void Hide(){
        frame.SetActive(false);
        showCharObj = null;
    }
    private void Update() {
        if(showCharObj != null){
            charTypeIconImage.sprite = showCharObj.createBtn.GetComponent<CharCreator>().charTypeIcon.sprite;
            charNameText.text = showCharObj.createBtn.GetComponent<CharCreator>().charName;
            charAtkText.text = "攻击："+showCharObj.damage.ToString();
            charPDefText.text = "物理抗性："+showCharObj.def.ToString();
            charADefText.text = "法术抗性："+showCharObj.adef.ToString();
            charLifeText.text = "生命  "+showCharObj.hp.ToString()+'/'+showCharObj.totalHp.ToString();
        }
    }
} 