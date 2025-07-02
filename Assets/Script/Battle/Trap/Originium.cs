using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Originium : MonoBehaviour
{
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null) {
            other.GetComponent<BuffController>().OnBuffLoaded(new Buff()
            {
                type = BuffType.Originium,
                lastTime = 5 * 60
            });
        }
    }
}