using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttack : MonoBehaviour
{
    [Header("Type")]
    public Mode attackType;
    public float attackDistance = 2f;
    public List<DamageCollider> dc;
    public AiMovement aim;
    public Transform shootPosition;
    public bool isAttacking;
    public bool isCharging;
    public ParticleSystem chargeParticles;
    public float cancelAttack;
    public float cancelAttackTime = 5f;

    [Header("Projectiles")]
    public float shootForce;

    [Header("Cooldown")]
    public float cooldown;
    public float cooldownMax;
    public float coolDownMin;
    public float cheatCounterTime;
    public float cheatCounterTimerMax;
    public int cheatCounter;
    public int cheatCounterMax;
    public float cheatCooldownEnabled; //Turns on if Enemy hits player in a combo.
    public float currentCooldownMax;
    public bool isCoolingdown;
    public bool hasFired;

    // Start is called before the first frame update
    void Start()
    {
        if (dc.Count > 0)
        {
            for (int i = 0; i < dc.Count; i++)
            {
                dc[i].Init();
                dc[i].gameObject.SetActive(false);
            }
        }

        aim = GetComponent<AiMovement>();

    }

    public void Return()
    {
        isAttacking = false;
        isCoolingdown = false;
        hasFired = false;
        cooldownMax = 0;
        if (dc.Count > 0)
        {
            for (int i = 0; i < dc.Count; i++)
            {
                dc[i].gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Cooldown();

        if(isAttacking)
        {
            cancelAttack += Time.deltaTime;

            if(cancelAttack >= cancelAttackTime)
            {
                cancelAttack = 0;
                isAttacking = false;
            }
        }
    }

    void Cooldown()
    {
        if (isCoolingdown)
        {
            cooldown += Time.deltaTime;

            if (cooldown >= currentCooldownMax)
            {
                cooldown = 0;

                if (hasFired)
                    hasFired = false;

                isCoolingdown = false;

            }
        }
    }

    public void Attack()
    {
        if (isCoolingdown)
            return;

        switch(attackType)
        {
            case Mode.Melee:
                MeleeAttack();
                break;
            case Mode.LongRange:
                LongRangeAttack();
                break;
        }
    }

    public void MeleeAttack()
    {
        isAttacking = true;
        aim.animator.Play("Kick");
        AudioManager.Instance.Play("doll_kick");
        //isCoolingdown = true;
    }

    public void LongRangeAttack()
    {
        if (!isAttacking)
        {
            if (aim.type == CharacterType.Doll)
                AudioManager.Instance.Play("doll_charge");
            else if(aim.type == CharacterType.Enemy)
                AudioManager.Instance.Play("kedama_charge");

            aim.animator.Play("Charge");
            isAttacking = true;
        }
            //if (!isAttacking)
            //{
            /*if (aim.type == CharacterType.Doll)
            {
                aim.animator.Play("Charge");
                FireProjectile();
            }
            else if (aim.type == CharacterType.Enemy)
                FireProjectile();*/
            //}
            /*DamageCollider projectileToLaunch = GetRandomAvaiableProjectile();

                if (projectileToLaunch == null)
                    return;

            if(!hasFired)
            {
                projectileToLaunch.gameObject.SetActive(true);
                projectileToLaunch.transform.SetParent(null);
                projectileToLaunch.transform.position = shootPosition.position;
                projectileToLaunch.rb.velocity = transform.forward * shootForce;
                isCoolingdown = true;
                projectileToLaunch.active = true;
                hasFired = true;
            }*/

            //  isAttacking = true;
            //}
        }

    public void ChargeParticles(int i)
    {
        if (i == 0)
            chargeParticles.Stop();

        if (i == 1)
            chargeParticles.Play();
    }

    public void EnableCooldown()
    {
        isCoolingdown = true;
    }

    public void AttackingStatus(int i)
    {
        if (i == 0)
            isAttacking = false;

        if (i == 1)
            isAttacking = true;
    }

    public void MeleeColliderStatus(int i)
    {
        if (i == 0)
            dc[0].gameObject.SetActive(false);

        if (i == 1)
            dc[0].gameObject.SetActive(true);
    }

    public void FireProjectile()
    {
        isAttacking = false;
        DamageCollider projectileToLaunch = GetRandomAvaiableProjectile();

        if (projectileToLaunch == null)
            return;

        //if (!hasFired)
        //{
            projectileToLaunch.gameObject.SetActive(true);
            projectileToLaunch.transform.SetParent(null);
            projectileToLaunch.transform.position = shootPosition.position;
            projectileToLaunch.rb.velocity = transform.forward * shootForce;
            projectileToLaunch.active = true;

            float ran = Random.Range(0, 1);

            if (ran > 0)
                currentCooldownMax = coolDownMin;
            else
                currentCooldownMax = cooldownMax;

        if (aim.type == CharacterType.Doll)
            AudioManager.Instance.Play("doll_shoot");
        else if(aim.type == CharacterType.Enemy)
            AudioManager.Instance.Play("kedama_shoot");

        isCoolingdown = true;

           // hasFired = true;
        //}
    }

    DamageCollider GetRandomAvaiableProjectile()
    {
        int ran = Random.Range(0, GetAvaiableProjectiles().Count);
        return GetAvaiableProjectiles()[ran];
    }

    List<DamageCollider> GetAvaiableProjectiles()
    {
        List<DamageCollider> r = new List<DamageCollider>();

        for (int i = 0; i < dc.Count; i++)
        {
            if (!dc[i].gameObject.activeInHierarchy)
            {
                r.Add(dc[i]);
            }
        }

        return r;
    }
}
