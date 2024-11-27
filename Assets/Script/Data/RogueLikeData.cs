using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RogueLikeData : MonoBehaviour 
{
    public static RogueLikeData Instance;
    public List<int> rogueChars = new List<int>();
    private void Start() {
        Instance = this;
    }
}