using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollManager : MonoBehaviour
{
    [Header("Doll")]
    public List<Doll> dolls;
    public List<Doll> activeDolls;
    public Transform[] spawnPositions;
    public int dollLimit; //The maximum amount of dolls the player can queue at once.
    public int spawnEachOfDoll;
    public GameObject meleeDollPrefab;
    public GameObject longRangeDollPrefab;
    public Transform par;

    public static DollManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for(int i = 0; i < spawnEachOfDoll; i++)
        {
            Instantiate(meleeDollPrefab, par);
        }

        for (int i = 0; i < spawnEachOfDoll; i++)
        {
            Instantiate(longRangeDollPrefab, par);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDoll(bool melee)
    {
        if (activeDolls.Count < dollLimit)
        {
            Doll doll = GetRandomAvaiableDoll(melee);
            doll.Init(melee);
            AudioManager.Instance.Play("doll_added");
        }
        //Check for what kind of hit was executed.

        //Check if the player can queue up anymore dolls. 

        //Spawn in Doll.
    }

    public Transform GetRandomSpawnPosition()
    {
        int ran = Random.Range(0, spawnPositions.Length);
        return spawnPositions[ran];
    }

    Doll GetRandomAvaiableDoll(bool melee)
    {
        int ran = Random.Range(0, GetAviaiableDoll(melee).Count);
        return GetAviaiableDoll(melee)[ran];
    }


    List<Doll> GetAviaiableDoll(bool melee)
    {
        List<Doll> d = new List<Doll>();

        for(int i = 0; i < dolls.Count; i++)
        {
            if(melee && dolls[i].type == Mode.Melee && !dolls[i].gameObject.activeInHierarchy)
            {
                d.Add(dolls[i]);
            }
            else if(!melee && dolls[i].type == Mode.LongRange && !dolls[i].gameObject.activeInHierarchy)
            {
                d.Add(dolls[i]);
            }
        }

        return d;
    }
}
