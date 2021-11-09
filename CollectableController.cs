using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    public string sceneToLoad;
    private int collectiblecounter = 0;
    private bool ran;

    public static CollectableController Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (collectiblecounter >= 4)
        {
            if (!ran)
            {
                MySceneManager.Instance.LoadAndUnloadScenes(sceneToLoad, MySceneManager.Instance.currentScene.sceneId);
                ran = true;
            }
        }
    }

    public void IncreaseItems()
    {
        collectiblecounter++;
    }
}
