using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesInScene : MonoBehaviour
{
    public List<Enemy> enemiesInScene;

    public static EnemiesInScene Instance;

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
