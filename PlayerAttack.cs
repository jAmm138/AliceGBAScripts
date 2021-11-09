using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerCharacter pc;

    [Header("Refernces")]
    public DamageCollider meleeCollider;
    public List<DamageCollider> projectiles;
    public Transform shootPosition;
    public Transform projectileParent;

    [Header("Doll Attack")]
    public int dollIndex;

    [Header("Melee Attack Options")]
    public bool disableRotateDuringAttack;
    public bool stopMovement;
    public float attackRotSpeed;


    [Header("Projecitle Attack Options")]
    public float shootForce;
    public float cooldown;
    public float longRangeCooldown;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerCharacter>();

        if (meleeCollider != null)
        {
            meleeCollider.gameObject.SetActive(false);
            meleeCollider.Init();
        }

        if (projectiles.Count > 0)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Init();
                projectiles[i].gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.Instance.dialogueActive)
            return;

        Cooldown();
        PlayerAttackInput();
        DollAttackInput();
    }

    void Cooldown()
    {
        if(pc.isCoolingDown)
        {
            cooldown += Time.deltaTime;

            if(cooldown >= longRangeCooldown)
            {
                cooldown = 0;
                pc.isCoolingDown = false;
            }
        }
    }

    void PlayerAttackInput()
    {
        //Performs checks before executing attack.
        if(!pc.changingMode && !pc.isCoolingDown && !pc.isAttacking && Input.GetButtonDown("Player Attack"))
        {
            //Checks which attack to perform.
            switch (pc.mode)
            {
                case Mode.Melee:
                    MeleeAttack();
                    break;
                case Mode.LongRange:
                    MeleeAttack();
                    break;
            }
        }
    }

    void DollAttackInput()
    {
        if (Input.GetButtonDown("Doll Attack") && pc.dm.activeDolls.Count > 0)
        {
            AudioManager.Instance.Play("doll_call");
            pc.animator.CrossFade("Throw Doll", 0.2f);
            pc.dm.activeDolls[dollIndex].AttackMode();
            dollIndex++;

            if (dollIndex > pc.dm.activeDolls.Count - 1)
            {
                dollIndex = 0;
            }
        }
    }

    void MeleeAttack()
    {
        //Play Animation
        pc.animator.CrossFade("Kick", 0.2f);
        AudioManager.Instance.Play("player_kick");

        if (stopMovement)
            pc.m.rb.constraints = RigidbodyConstraints.FreezePositionX;

        pc.isAttacking = true;
    }

    void LongRangeAttack()
    {
        DamageCollider projectileToLaunch = GetRandomAvaiableProjectile();

        if (projectileToLaunch == null)
            return;

        projectileToLaunch.gameObject.SetActive(true);
        projectileToLaunch.transform.SetParent(null);
        projectileToLaunch.transform.position = shootPosition.position;
        projectileToLaunch.rb.velocity = transform.forward * shootForce;
        pc.isCoolingDown = true;
        projectileToLaunch.active = true;
    }

    #region Animator Events

    public void AttackingStatus(int i)
    {
        if(i == 0)
            pc.isAttacking = false;

        if (i == 1)
            pc.isAttacking = true;
    }

    public void GuardStatus(int i)
    {
        if (i == 0)
            pc.guard = false;

        if (i == 1)
            pc.guard = true;
    }

    public void MeleeColliderStatus(int i)
    {
        if (i == 0)
            meleeCollider.gameObject.SetActive(false);

        if (i == 1)
            meleeCollider.gameObject.SetActive(true);
    }

    #endregion

    DamageCollider GetRandomAvaiableProjectile()
    {
        int ran = Random.Range(0, GetAvaiableProjectiles().Count);
        return GetAvaiableProjectiles()[ran];
    }

    List<DamageCollider> GetAvaiableProjectiles()
    {
        List<DamageCollider> dc = new List<DamageCollider>();

        for(int i = 0; i < projectiles.Count; i++)
        {
            if(!projectiles[i].gameObject.activeInHierarchy)
            {
                dc.Add(projectiles[i]);
            }
        }

        return dc;
    }
    
}
