using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
public class BuffController : MonoBehaviour
{
    public List<Buff> buffs = new List<Buff>();
    private List<Buff> removedBuff = new List<Buff>();
    public bool isEnemy = true;
    private void Start()
    {

    }
    public void OnBuffLoaded(Buff buff)
    {
        if (isEnemy)
        {
            Enemy e = GetComponent<Enemy>();
            switch (buff.type)
            {
                case BuffType.Originium:
                    e.atkScale += 0.5f;
                    break;
            }
        }
        else
        {
            switch (buff.type)
            {

            }
        }
        buffs.Add(buff);
    }
    private void OnBuffRemain()
    {
        foreach (Buff buff in buffs)
        {
            if (isEnemy)
            {
                Enemy e = GetComponent<Enemy>();
                switch (buff.type)
                {
                    case BuffType.Originium:
                        e.TakeDamage(new Damage()
                        {
                            dt = Damage.DamageType.Real,
                            damage = 180 * Time.deltaTime
                        });
                        break;
                }
            }
            else
            {
                switch (buff.type) {

                }
            }
        }
        foreach (Buff buff in buffs)
        {
            buff.lastTime -= Time.deltaTime;
        }
    }
    private void OnBuffRemoved()
    {
        removedBuff.Clear();
        foreach (Buff buff in buffs)
        {
            if(buff.lastTime <= 0)
            {
                if (isEnemy)
                {
                    Enemy e = GetComponent<Enemy>();
                    switch (buff.type)
                    {
                        case BuffType.Originium:
                            e.atkScale -= 0.5f;
                            break;
                    }
                }
                else
                {
                    switch (buff.type)
                    {
                    }
                }
                    removedBuff.Add(buff);
            }
        }
        foreach(Buff buff in removedBuff)
        {
            buffs.Remove(buff);
        }
    }
    private void Update()
    {
        OnBuffRemain();
        OnBuffRemoved();
    }
}