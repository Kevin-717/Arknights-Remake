using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharData : MonoBehaviour 
{
    [System.Serializable]
    public class Character
    {
        public int id;
        public Sprite image;
        public Sprite type;
        public GameObject charPrefab;
        public int cost;
        public float waitTime;
        public int star = 5;
        public enum CharType{
            lowLand,
            highLand,
        }
        public CharType ct;
        public Sprite charIcon;
        public Sprite charHalf;
        public string charName;
    }
    public List<Character> charList;
    public static CharData instance;
    private void Start() {
        instance = this;
    }
}