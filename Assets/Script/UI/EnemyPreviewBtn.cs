using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class EnemyPreviewBtn : MonoBehaviour
{
    public EnemyData ed;
    public void OnClicked()
    {
        EnemyPreview.instance.ShowEnemy(ed);
    }
}