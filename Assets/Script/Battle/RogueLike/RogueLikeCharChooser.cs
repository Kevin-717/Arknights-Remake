using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class RogueLikeCharChooser : MonoBehaviour 
{
    public void Back(){
        MapCreator.instance.ReDraw();
    }
}