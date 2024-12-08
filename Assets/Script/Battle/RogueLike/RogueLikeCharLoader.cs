using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class RogueLikeCharLoader : MonoBehaviour 
{
    public GameObject btnPrefab;
    public Transform frame;
    private RectTransform rectTransform;
    public List<Sprite> raities = new List<Sprite>();

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        int col = 0;
        if(RogueLikeData.Instance.rogueChars.Count % 3 == 0){
            col = RogueLikeData.Instance.rogueChars.Count / 3;
        }else{
            col = RogueLikeData.Instance.rogueChars.Count / 3 + 1;
        }
        rectTransform.sizeDelta = new Vector2(100+150+635.5f*col,1063);
        foreach(int ind in RogueLikeData.Instance.rogueChars){
            foreach(CharData.Character c in CharData.instance.charList){
                if(c.id == ind){
                    CharData.Character character = c;
                    GameObject b = Instantiate(btnPrefab);
                    b.transform.SetParent(frame);
                    b.GetComponent<RectTransform>().localScale = new Vector3(Screen.width/1920f,Screen.height/1080f,1);
                    b.GetComponent<RogueLikeBtn>().raity.sprite = raities[character.star-1];
                    b.GetComponent<RogueLikeBtn>().charName.text = character.charName;
                    b.GetComponent<RogueLikeBtn>().charIcon.sprite = character.image;
                    b.GetComponent<RogueLikeBtn>().charId = character.id;
                }
            }
        }
    }
}