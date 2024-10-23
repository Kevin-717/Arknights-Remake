using UnityEngine;
public class AstarInstance : MonoBehaviour 
{
    public static AstarInstance instance;
    private void Start() {
        instance = this;
    }
}