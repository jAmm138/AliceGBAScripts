using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("States")]
    public bool isDead;

    public float health;
    public float healthMax;

    public bool iFrames;
    public float iFramesTime;
    public float iFramesTimeMax;
    public AiMovement aim;
    public AiAttack aia;
    public float destroy;
    public float destroyTime =2f;
    public GameObject healthPickup;
    public LockOnTarget lockOnTarget;
    public ParticleSystem damageParticles;

    // Start is called before the first frame update
    void Start()
    {
        health = healthMax;

        aim = GetComponent<AiMovement>();
        aia = GetComponent<AiAttack>();
        lockOnTarget = GetComponent<LockOnTarget>();

        EnemiesInScene.Instance.enemiesInScene.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            aim.enabled = false;
            aia.enabled = false;
            aim.animator.SetBool("isDead", isDead);

            if (destroy < destroyTime)
                destroy += Time.deltaTime;
            else
            {
                if (lockOnTarget.transform.position == PlayerCharacter.Instance.m.target.transform.position
                    && PlayerCharacter.Instance.m.isLockedOn)
                    LockOnManager.Instance.RemoveTarget();

                lockOnTarget.Remove();
                int ran = Random.Range(0, 100);
                float chance = 50f;

                if (PlayerCharacter.Instance.health < 4)
                    chance = 50f + 15f;
                else
                    chance = 50f;

                if (ran > chance)
                {
                    Debug.Log(ran);
                    GameObject hpu = Instantiate(healthPickup);
                    hpu.transform.position = this.transform.position;
                    Destroy(this.gameObject);
                }
                else
                {
                    Debug.Log(ran);
                    Destroy(this.gameObject);
                }
            }
            //Play Animation
            return;
        }

        IFrames();
    }

    void IFrames()
    {
        if(iFrames)
        {
            aia.enabled = false;
            aim.enabled = false;
            iFramesTime += Time.deltaTime;

            if(iFramesTime >= iFramesTimeMax)
            {
                iFramesTime = 0;

                aia.enabled = true;
                aim.enabled = true;
                iFrames = false; 
            }
        }
    }

    public void TakeDamage(float damage, bool melee, bool isPlayer)
    {
        if(!iFrames)
        {
            health -= damage;

            if (health > 0)
            {
                damageParticles.Play();
                AudioManager.Instance.Play("enemy_damage");

                if (isPlayer)
                    DollManager.Instance.AddDoll(melee);

                iFrames = true;
            }
            else
            {
                damageParticles.Play();
                AudioManager.Instance.Play("enemy_death");
                Death();
            }
        }
    }

    void Death()
    {
        health = 0;
        isDead = true;
        EnemiesInScene.Instance.enemiesInScene.Remove(this);
    }
}
