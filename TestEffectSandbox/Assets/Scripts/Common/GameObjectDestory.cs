using UnityEngine;
using System.Collections;

public class GameObjectDestory : MonoBehaviour 
{
    public float LifeTime;

    // Use this for initialization
    void Start ()
    {
        Destroy(gameObject, LifeTime);
    }
}
