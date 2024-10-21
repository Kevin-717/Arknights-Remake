using UnityEngine;
using System.Collections;
using System.Collections.Generic;
class swllow : MonoBehaviour 
{
    private Char charController;
    [Header("二技能")]
    public string Attack_anim = "Skill_2_Loop";
    public string Idle_anim = "Skill_2_Idle";
    private string n_Attack_anim = "Attack_Loop";
    private string n_Idle_anim = "Idle";
    private void Start() {
        charController = GetComponent<Char>();
    }
    public void OnSkill2(){
        charController.damage *= 1.4f;
        charController.Attack_anim = Attack_anim;
        charController.Idle_anim = Idle_anim;
    }
    public void OnSkill2End(){
        charController.damage /= 1.4f;
        charController.Attack_anim = n_Attack_anim;
        charController.Idle_anim = n_Idle_anim;
    }
}