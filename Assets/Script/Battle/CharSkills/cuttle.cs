using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class cuttle : MonoBehaviour 
{
    private Char charController;
    private void Start() {
        charController = GetComponent<Char>();
    }
    public void skill1Start(){
        charController.damage *= 2;
    }
    public void skill1End(){
        charController.damage /= 2;
    }
}