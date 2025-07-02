using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Spine.Unity;
using UnityEditor;
[System.Serializable]
public class EnemyAnimation
{
    public EnemyState state;
    [SpineAnimation] public string anim;
    public bool isLoop = true;
}
[System.Serializable]
public class CharAnimation
{
    public CharState state;
    [SpineAnimation] public string anim;
    public bool isLoop = true;
}
public enum EnemyState
{
    Move,
    Idle,
    Attack,
    AttackInterval,
    Die,
    ReBorn,
    Default,
    Shoot
}
public enum CharState
{
    Idle,
    Attack,
    AttackInterval,
    Die,
    Default,
    Start
}
public enum BuffType
{
    Originium,
    Freeze,
    Dizzy,
}
[System.Serializable]
public class Damage
{
    public enum DamageType
    {
        Physics,
        Magic,
        Real,
        Recovery
    }
    public DamageType dt;
    public float damage;
}
public enum PointType
{
    normal,
    wait,
    disappear,
    appear,
}
[System.Serializable]
public class EnemyPath{
    public Vector3 path;
    public PointType pointType;
    public float waitTime = 0;
}
[System.Serializable]
public class EnemyGroupData{
    public int type = 0;
    public GameObject enemyPrefab;
    public Vector3 startPos;
    public List<EnemyPath> enemyPath = new List<EnemyPath>();
    public int repeat = 1;
    public float interval = 1;
    public float startTime;
    public bool created = false;
    public string name;
    public string description;
    public Sprite icon;
}
public enum CharType
{
    HighLand,
    LowLand
}
[System.Serializable]
public class CharInfo
{
    public int cid;
    public CharType charType;
    public GameObject charPrefab;
    public int cost;
    public float replaceTime;
    public float max_hp;
    public float atk;
    public float def;
    public float mdef;
    public string name;
    public string name_en;
    public int def_num;
    public CharUIData cui;
}
[System.Serializable]
public class CharUIData
{
    public Sprite icon_box;
    public Sprite icon_half;
    public Sprite icon_type;
    public Star_Sprite sp;
}
public enum LandType
{
    lowLand,
    highLand
}
[System.Serializable]
public class LevelDescText
{
    [Header("关卡编号")]
    public string level_num;
    [Header("关卡名")]
    public string level_name;
    [Header("关卡场景名")]
    public string scene_name;
    [Header("关卡介绍")]
    [Multiline(5)]
    public string level_desc;
    [Header("关卡推荐等级")]
    public string level_c;
    [Header("关卡json文件")]
    public string json_path;
}
[System.Serializable]
public class CharProfession
{
    public string profession;
    public Sprite icon;
}
[System.Serializable]
public class Star_Sprite
{
    public Sprite star;
    public Sprite bloom;
    public Sprite bottom;
    public Sprite bkg;
}
[System.Serializable]
public class EnemyData
{
    public string key;
    public Sprite icon;
    public string name;
    public string introduction;
    public string def;
    public string atk;
    public string mdef;
    public string life;
    public string group;
}
[System.Serializable]
public class Buff
{
    public BuffType type;
    public float lastTime;
}